using Microsoft.Owin;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Owin;
using Project3TestMqtt.Services;
using System;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(Project3TestMqtt.Startup))]
namespace Project3TestMqtt
{
    public partial class Startup
    {
        public static IManagedMqttClient client;

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.Run(async (context) =>
            {
                await MQTT.ConnectAsync();
            });
        }

    }
}
