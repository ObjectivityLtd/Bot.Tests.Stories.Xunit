namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.User
{
    using System;
    using Microsoft.Bot.Builder;

    public class DemoUserStateAccessors
    {
        public DemoUserStateAccessors(UserState userState)
        {
            this.UserState = userState ?? throw new ArgumentNullException(nameof(userState));

            this.DemoUserState =
                userState.CreateProperty<DemoUserState>(DemoUserStateAccessorKey);
        }

        public static string DemoUserStateAccessorKey { get; } = $"{nameof(DemoUserStateAccessors)}.{nameof(DemoUserState)}";

        public IStatePropertyAccessor<DemoUserState> DemoUserState { get; set;  }

        public UserState UserState { get; }
    }
}
