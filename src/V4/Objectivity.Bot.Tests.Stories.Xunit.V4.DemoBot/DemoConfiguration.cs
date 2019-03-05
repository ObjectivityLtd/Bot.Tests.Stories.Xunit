namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot
{
    using System;
    using System.Linq;
    using Dialogs.State;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Configuration;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using User;

    public static class DemoConfiguration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            IStorage dataStore = new MemoryStorage();
            var userState = new UserState(dataStore);

            services.AddScoped(sp => new ConversationState(dataStore));
            services.AddScoped(sp => new UserState(dataStore));

            services.AddBot<DemoBot>(options =>
            {
                var botConfig = BotConfiguration.Load(@".\demo.bot", string.Empty);
                services.AddSingleton(sp => botConfig);

                var service = botConfig.Services.FirstOrDefault(s => s.Type == "endpoint" && s.Name == "development");

                if (!(service is EndpointService endpointService))
                {
                    throw new InvalidOperationException("The .bot file does not contain a development endpoint.");
                }

                options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

                options.OnTurnError = async (context, exception) =>
                {
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };
            });

            if (services.All(x => x.ServiceType != typeof(IRoomService)))
            {
                services.AddScoped<IRoomService, RoomService>();
            }


            services.AddScoped(sp => new DemoUserStateAccessors(userState));
            services.AddScoped(sp => new DemoDialogStateAccessors(sp.GetService<ConversationState>()));
        }
    }
}
