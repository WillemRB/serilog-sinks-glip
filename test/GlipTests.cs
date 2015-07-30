using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Sinks.Glip.Tests
{
    [TestClass]
    public class GlipTests
    {
        private ILogger _logger;

        [TestInitialize]
        public void Initialize()
        {
            var webhook = Guid.Parse("efc66d30-2f14-4acc-b07b-f27e2cf69cd7");

            _logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Sink(new GlipSink(webhook))
                .CreateLogger();
        }

        [TestMethod]
        public void HelloWorldTest()
        {
            _logger.Information("Hello World");
        }

        [TestMethod]
        public void PropertyTest()
        {
            var properties = new List<LogEventProperty>();

            properties.Add(new LogEventProperty("activity", new ScalarValue("Activity")));
            properties.Add(new LogEventProperty("body", new ScalarValue("Body")));
            properties.Add(new LogEventProperty("icon", new ScalarValue("http://tinyurl.com/pn46fgp")));
            properties.Add(new LogEventProperty("title", new ScalarValue("Title")));

            var messageTemplate = new MessageTemplateParser().Parse("The value of the 'body' property: {body}");

            var logEvent = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null, messageTemplate, properties);

            _logger.Write(logEvent);
        }

        [TestMethod]
        public void OtherTest()
        {
            var body = "Message Body";
            var activity = "Running Tests";

            _logger.Write(LogEventLevel.Information, "{body}", body, activity);
        }
    }
}
