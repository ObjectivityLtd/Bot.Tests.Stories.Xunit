// Based on: https://github.com/Microsoft/BotBuilder/blob/3.0/CSharp/Tests/Microsoft.Bot.Builder.Tests/LuisTests.cs
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Moq;

    public abstract class LuisTestBase : DialogTestBase
    {
        public static IntentRecommendation[] IntentsFor<TDialog>(Expression<Func<TDialog, Task>> expression, double? score)
        {
            if (expression == null)
            {
                return null;
            }

            var body = (MethodCallExpression)expression.Body;
            var attributes = body.Method.GetCustomAttributes<LuisIntentAttribute>();
            var intents = attributes
                .Select(attribute => new IntentRecommendation(attribute.IntentName, score))
                .ToArray();
            return intents;
        }

        public static EntityRecommendation EntityFor(string type, string entity, IDictionary<string, object> resolution = null)
        {
            return new EntityRecommendation(type) { Entity = entity, Resolution = resolution };
        }

        public static EntityRecommendation EntityForDate(string type, DateTime date)
        {
            return EntityFor(
                type,
                date.ToString("d", DateTimeFormatInfo.InvariantInfo),
                new Dictionary<string, object>
                {
                    { "resolution_type", "builtin.datetime.date" },
                    { "date", date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) }
                });
        }

        public static EntityRecommendation EntityForTime(string type, DateTime time)
        {
            return EntityFor(
                type,
                time.ToString("t", DateTimeFormatInfo.InvariantInfo),
                new Dictionary<string, object>
                {
                    { "resolution_type", "builtin.datetime.time" },
                    { "time", time.ToString("THH:mm:ss", DateTimeFormatInfo.InvariantInfo) }
                });
        }

        public static void SetupLuis<TDialog>(
            Mock<ILuisService> luis,
            Expression<Func<TDialog, Task>> expression,
            double? score,
            params EntityRecommendation[] entities)
        {
            luis?.Setup(l => l.QueryAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LuisResult
                {
                    Intents = IntentsFor(expression, score),
                    Entities = entities
                });
        }

        public static void SetupLuis<TDialog>(
            Mock<ILuisService> luis,
            string utterance,
            Expression<Func<TDialog, Task>> expression,
            double? score,
            params EntityRecommendation[] entities)
        {
            var uri = new UriBuilder { Query = utterance }.Uri;

            if (luis != null)
            {
                luis
                    .Setup(l => l.BuildUri(It.Is<LuisRequest>(r => r.Query == utterance)))
                    .Returns(uri);

                luis.Setup(l => l.ModifyRequest(It.IsAny<LuisRequest>()))
                    .Returns<LuisRequest>(r => r);

                luis
                    .Setup(l => l.QueryAsync(uri, It.IsAny<CancellationToken>()))
                    .Returns<Uri, CancellationToken>(async (_, token) => new LuisResult
                    {
                        Intents = IntentsFor(expression, score),
                        Entities = entities
                    });
            }
        }
    }
}
