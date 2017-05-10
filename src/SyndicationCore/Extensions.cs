using System;

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
    }
}