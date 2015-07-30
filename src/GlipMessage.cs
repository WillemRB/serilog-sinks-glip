using System;

namespace Serilog.Sinks.Glip
{
    public class GlipMessage
    {
        public string Activity { get; set; }

        public string Body { get; set; }

        public Uri Icon { get; set; }

        public string Title { get; set; }

        public GlipMessage(string body)
        {
            Body = body;
        }
    }
}
