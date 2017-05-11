using System;
using System.Xml.Linq;

namespace SyndicationCore {
    public class Rss20SyndicationGenerator : ISyndicationGenerator {
        public XDocument Generate(SyndicationFeed feed) {
            if(string.IsNullOrWhiteSpace(feed.Title))
                throw new ArgumentException("Title needs to be set.");
            
            if(feed.SiteUrl == null)
                throw new ArgumentException("SiteUrl needs to be set.");

            if(string.IsNullOrWhiteSpace(feed.Description))
                throw new ArgumentException("Description needs to be set.");

            var channel = new XElement("channel");
            var rss = new XElement("rss", new XAttribute("version", "2.0"), channel);
            var document = new XDocument(rss);

            // Required fields
            channel.Add(new XElement("title", feed.Title));
            channel.Add(new XElement("link", feed.SiteUrl));
            channel.Add(new XElement("description", feed.Description));

            // Calculated fields
            channel.Add(new XElement("lastBuildDate", DateTime.Now.ToRFC822()));
            channel.Add(new XElement("generator", "SyndicationCore/" + VersionHelper.ForType<Rss20SyndicationGenerator>()));

            // Optional fields
            channel.AddOptionalElement("category", string.Join(" / ", feed.Categories));
            channel.AddOptionalElement("description", feed.Description);
            channel.AddOptionalElement("language", feed.Language?.ToString());
            channel.AddOptionalElement("image", feed.Image?.ToString());

            if(feed.TimeToLive != TimeSpan.Zero)
                channel.Add(new XElement("ttl", feed.TimeToLive.TotalMinutes));

            foreach (var item in feed.Items) {
                var element = new XElement("item");

                if(string.IsNullOrWhiteSpace(item.Title) && string.IsNullOrWhiteSpace(item.Body)) {
                    // Either Title or Description must be set for an item to be valid
                    // A logger would be nice here
                    continue;
                }

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

                channel.Add(element);
            }

            return document;
        }
    }
}