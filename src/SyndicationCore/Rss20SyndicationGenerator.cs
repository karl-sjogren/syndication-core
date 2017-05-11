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

            XNamespace nsAtom = "http://www.w3.org/2005/Atom";

            var channel = new XElement("channel");
            var rss = new XElement("rss", new XAttribute("version", "2.0"), new XAttribute(XNamespace.Xmlns + "atom", nsAtom), channel);
            var document = new XDocument(rss);
            document.Declaration = new XDeclaration("1.0", "utf-8", null);

            // Required fields
            channel.Add(new XElement("title", feed.Title));
            channel.Add(new XElement("link", feed.SiteUrl));
            channel.Add(new XElement("description", feed.Description));

            // Calculated fields
            channel.Add(new XElement("lastBuildDate", DateTime.Now.ToRFC822()));
            channel.Add(new XElement("generator", "SyndicationCore/" + VersionHelper.ForType<Rss20SyndicationGenerator>()));

            // Optional fields
            channel.AddOptionalElement("category", string.Join(" / ", feed.Categories));
            channel.AddOptionalElement("language", feed.Language?.ToString());
            channel.AddOptionalElement("image", feed.Image?.ToString());

            channel.Add(new XElement(nsAtom + "link",
                new XAttribute("href", feed.FeedUrl),
                new XAttribute("rel", "self"),
                new XAttribute("type", "application/rss+xml")));

            if(feed.TimeToLive != TimeSpan.Zero)
                channel.Add(new XElement("ttl", feed.TimeToLive.TotalMinutes));

            foreach (var item in feed.Items) {
                var element = new XElement("item");

                if(string.IsNullOrWhiteSpace(item.Title) && string.IsNullOrWhiteSpace(item.Body)) {
                    // Either Title or Description must be set for an item to be valid
                    // A logger would be nice here
                    continue;
                }

                var author = item.Author?.Email;
                if(author != null && !string.IsNullOrWhiteSpace(item.Author?.Name))
                    author += $" ({item.Author.Name})";

                element.AddOptionalElement("title", item.Title);
                element.AddOptionalElement("link", item.Link);
                element.AddOptionalElement("author", author);
                element.AddOptionalElement("pubDate", item.PublishDate.ToRFC822());
                element.AddOptionalElement("category", string.Join(" / ", item.Categories));
                element.AddOptionalElement("guid", item.Permalink);

                if(!string.IsNullOrWhiteSpace(item.Body))
                    element.Add(new XElement("description", item.Body));

                if(!string.IsNullOrWhiteSpace(feed.Title) && feed.FeedUrl != null)
                    element.Add(new XElement("source", feed.Title, new XAttribute("url", feed.FeedUrl)));

                channel.Add(element);
            }

            return document;
        }
    }
}