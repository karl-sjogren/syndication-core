using System;
using System.Xml.Linq;

namespace SyndicationCore {
    public class Rss20SyndicationGenerator : ISyndicationGenerator {
        public XDocument Generate(SyndicationFeed feed) {
            var root = new XElement("channel", new XAttribute("version", "2.0"));
            var channel = new XDocument(root);

            root.Add(new XElement("title", feed.Title));
            root.Add(new XElement("link", feed.SiteUrl));
            root.Add(new XElement("description", feed.Description));
            root.Add(new XElement("ttl", feed.TimeToLive.TotalMinutes));
            root.Add(new XElement("category", string.Join(" / ", feed.Categories)));
            root.Add(new XElement("lastBuildDate", DateTime.Now.ToRFC822()));
            root.Add(new XElement("description", feed.Description));

            if(feed.Language != null)
                root.Add(new XElement("language", feed.Language.ToString()));

            if(feed.Image != null)
                root.Add(new XElement("image", feed.Image.ToString()));
            
            root.Add(new XElement("generator", "SyndicationCore/0.0"));

            foreach (var item in feed.Items) {
                var element = new XElement("item");

                element.Add(new XElement("title", item.Title));
                element.Add(new XElement("link", item.Link));
                element.Add(new XElement("author", item.Author?.Email));
                element.Add(new XElement("pubDate", item.PublishDate.ToRFC822()));
                element.Add(new XElement("category", string.Join(" / ", item.Categories)));
                element.Add(new XElement("guid", item.Permalink));

                if(!string.IsNullOrWhiteSpace(item.Body))
                    element.Add(new XElement("description", new XCData(item.Body)));

                if(!string.IsNullOrWhiteSpace(feed.Title) && feed.FeedUrl != null)
                    element.Add(new XElement("source", feed.Title, new XAttribute("url", feed.FeedUrl)));

                root.Add(element);
            }

            return channel;
        }
    }
}