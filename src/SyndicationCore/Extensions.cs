using System;
using System.Xml.Linq;

namespace SyndicationCore {
    public static class Extensions {
        public static string GenerateString(this ISyndicationGenerator generator, SyndicationFeed feed) {
            var doc = generator.Generate(feed);
            return doc.ToString();
        }

        public static string ToRFC822(this DateTime date) {
            var offset = TimeZoneInfo.Local.Base​Utc​Offset.Hours;
            var timeZone = "+" + offset.ToString().PadLeft(2, '0');
 
            if(offset < 0) {
                timeZone = "-" + (offset * -1).ToString().PadLeft(2, '0');
            }
 
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'));
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