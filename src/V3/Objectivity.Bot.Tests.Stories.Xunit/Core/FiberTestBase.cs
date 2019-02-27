// Based on: https://github.com/Microsoft/BotBuilder/blob/3.0/CSharp/Tests/Microsoft.Bot.Builder.Tests/FiberTests.cs
//
//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Bot Framework: http://botframework.com
//
// Bot Builder SDK Github:
// https://github.com/Microsoft/BotBuilder
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Internals.Fibers;

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Code ported from external source. Unused members left for consistency.")]
    public abstract class FiberTestBase
    {
        public static readonly ContextStruct Context = default(ContextStruct);
        public static readonly CancellationToken Token = new CancellationTokenSource().Token;

        public static Moq.Mock<IMethod> MockMethod()
        {
            var method = new Moq.Mock<IMethod>(Moq.MockBehavior.Loose);
            return method;
        }

        public static Expression<Func<IAwaitable<T>, bool>> Item<T>(T value)
        {
            return item => value.Equals(item.GetAwaiter().GetResult());
        }

        public static bool ExceptionOfType<T, TException>(IAwaitable<T> item)
            where TException : Exception
        {
            try
            {
                item?.GetAwaiter().GetResult();
                return false;
            }
            catch (TException)
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Expression<Func<IAwaitable<T>, bool>> ExceptionOfType<T, TException>()
            where TException : Exception
        {
            return item => ExceptionOfType<T, TException>(item);
        }

        public static async Task PollAsync(IFiberLoop<ContextStruct> fiber)
        {
            IWait wait;
            do
            {
                wait = await fiber.PollAsync(Context, Token);
            }
            while (wait.Need != Need.None && wait.Need != Need.Done);
        }

        public static IContainer Build()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new FiberModule<ContextStruct>());
            return builder.Build();
        }

        public static void AssertSerializable<T>(ILifetimeScope scope, ref T item)
            where T : class
        {
            var formatter = scope.Resolve<IFormatter>();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, item);
                stream.Position = 0;
                item = (T)formatter.Deserialize(stream);
            }
        }
    }
}
