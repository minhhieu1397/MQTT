using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project3TestMqtt.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private static IManagedMqttClient client;

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public static async Task ConnectAsync()
        {
            string clientId = Guid.NewGuid().ToString();
            string mqttURI = "tailor.cloudmqtt.com";
            string mqttUser = "gjzfitml";
            string mqttPassword = "HsBFHWQqr7dt";
            int mqttPort = 15923;
            bool mqttSecure = false;
            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttURI, mqttPort)
                .WithCleanSession();
            var options = mqttSecure
              ? messageBuilder
                .WithTls()
                .Build()
              : messageBuilder
                .Build();
            var managedOptions = new ManagedMqttClientOptionsBuilder()
              .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
              .WithClientOptions(options)
              .Build();
            client = new MqttFactory().CreateManagedMqttClient();
            await client.StartAsync(managedOptions);
        }

        public static async Task PublishAsync(string topic, string payload, bool retainFlag = true, int qos = 1) =>
          await client.PublishAsync(new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .WithRetainFlag(retainFlag)
            .Build());

        public static async Task SubscribeAsync(string topic, int qos = 1) =>
          await client.SubscribeAsync(new TopicFilterBuilder()
            .WithTopic(topic)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .Build());
    }
}