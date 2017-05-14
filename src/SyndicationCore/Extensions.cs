using System;
using System.Globalization;
using System.Xml.Linq;

namespace SyndicationCore {
    public static class Extensions {
        private static IFormatProvider _formatProvider = new CultureInfo("en-US"); 

        public static string GenerateString(this ISyndicationGenerator generator, SyndicationFeed feed) {
            return generator.Generate(feed)?.ToString();
        }

        public static string ToRFC822(this DateTime? date) {
            if(!date.HasValue)
                return null;
            
            return date.Value.ToRFC822();
        }

        public static string ToRFC822(this DateTime date) {
            var offset = TimeZoneInfo.Local.Base​Utc​Offset.Hours;
            
            var timeZone = "+" + offset.ToString().PadLeft(2, '0');
 
            if(offset < 0) {
                timeZone = "-" + (offset * -1).ToString().PadLeft(2, '0');
            }

            if(date.Kind == DateTimeKind.Utc)
                timeZone = "+00";
 
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), _formatProvider);
        }

        public static string ToRFC3339(this DateTime? date) {
            if(!date.HasValue)
                return null;
            
            return date.Value.ToRFC3339();
        }

        public static string ToRFC3339(this DateTime date) {
            return date.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        public static void AddOptionalElement(this XElement element, XName name, object content) {
            if(content == null)
                return;
            
            if(content is string && string.IsNullOrWhiteSpace((content as string)))
                return;
            
            element.Add(new XElement(name, content));
        }
    }
}