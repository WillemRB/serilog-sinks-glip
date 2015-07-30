using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Serilog.Sinks.Glip
{
    public class GlipSink : ILogEventSink
    {
        private readonly JsonSerializerSettings _settings;

        public Uri WebHook
        {
            get;
            private set;
        }

        public GlipSink(Uri webHookUri)
        {
            _settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            WebHook = webHookUri;
        }

        public GlipSink(Guid webhookGuid)
            : this(new Uri(string.Format("https://hooks.glip.com/webhook/{0}", webhookGuid)))
        {
        }

        public void Emit(LogEvent logEvent)
        {
            var activity = logEvent.TryGetPropertyValue("activity");
            var icon = logEvent.TryGetPropertyValue("icon");
            var title = logEvent.TryGetPropertyValue("title");

            var message = new GlipMessage(logEvent.RenderMessage())
            {
                Activity = activity,
                Icon = icon == null ? null : new Uri(icon),
                Title = title,
            };

            SendRequest(message);
        }

        private void SendRequest(GlipMessage message)
        {
            var body = JsonConvert.SerializeObject(message, _settings);

            var request = WebRequest.Create(WebHook);
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var stream = request.GetRequestStream())
            {
                var bytes = Encoding.UTF8.GetBytes(body);
                stream.Write(bytes, 0, bytes.Length);
            }

            var response = request.GetResponse();

            return;
        }
    }
}
