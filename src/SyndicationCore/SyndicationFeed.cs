using System;
using System.Collections.Generic;
using System.Globalization;

namespace SyndicationCore {
    public class SyndicationFeed {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri SiteUrl { get; set; }
        public Uri Image { get; set; }
        public TimeSpan TimeToLive { get; set; } = TimeSpan.Zero;
        public CultureInfo Language { get; set; }
        public string Copyright { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public Uri FeedUrl { get; set; }

        public List<SyndicationItem> Items { get; set; } = new List<SyndicationItem>();
    }

    public class SyndicationItem {
        public string Title { get; set; }
        public Author Author { get; set; }
        public string Body { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public Uri Comments { get; set; }
        public Uri Link { get; set; }
        public string Permalink { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public class Author {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Link { get; set; }
    }
}
