using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;

namespace Serilog.Sinks.Glip
{
    public static class SerilogExtensions
    {
        public static string TryGetPropertyValue(this LogEvent logEvent, string key)
        {
            if (!logEvent.Properties.ContainsKey(key))
            {
                return null;
            }

            var property = logEvent.Properties[key];

            if (property is ScalarValue)
            {
                return (string)(property as ScalarValue).Value;
            }

            return null;
        }
    }
}
