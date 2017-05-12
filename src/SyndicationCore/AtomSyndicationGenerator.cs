using System;
using System.Xml.Linq;

namespace SyndicationCore {
    /// <summary>
    /// Generates a feed according to https://validator.w3.org/feed/docs/atom.html
    /// </summary>
    public class AtomSyndicationGenerator : ISyndicationGenerator {
        public XDocument Generate(SyndicationFeed feed) {
            if(string.IsNullOrWhiteSpace(feed.Title))
                throw new ArgumentException("Title needs to be set.");
            
            if(feed.SiteUrl == null)
                throw new ArgumentException("SiteUrl needs to be set.");

            XNamespace nsAtom = "http://www.w3.org/2005/Atom";

            var feedElement = new XElement(nsAtom + "feed");
            //var rss = new XElement("rss", new XAttribute("version", "2.0"), new XAttribute(XNamespace.Xmlns + "atom", nsAtom), channel);
            var document = new XDocument(feedElement);
            document.Declaration = new XDeclaration("1.0", "utf-8", null);

            // Required fields
            feedElement.Add(new XElement(nsAtom + "id", feed.SiteUrl.AbsoluteUri));
            feedElement.Add(new XElement(nsAtom + "title", feed.Title));

            // Calculated fields
            feedElement.Add(new XElement(nsAtom + "updated", DateTime.Now.ToRFC3339())); // Should probably be the highest item value instead
            feedElement.Add(new XElement(nsAtom + "generator", "SyndicationCore",
                new XAttribute("version", VersionHelper.ForType<AtomSyndicationGenerator>()),
                new XAttribute("uri", "https://github.com/karl-sjogren/syndication-core")));

            // Optional fields
            feedElement.AddOptionalElement(nsAtom + "logo", feed.Image?.AbsoluteUri);
            feedElement.AddOptionalElement(nsAtom + "subtitle", feed.Description);

            foreach(var category in feed.Categories)
                feedElement.Add(new XElement(nsAtom + "category", new XAttribute("term", category)));

            if(feed.FeedUrl != null)
                feedElement.Add(new XElement(nsAtom + "link",
                    new XAttribute("href", feed.FeedUrl.AbsoluteUri),
                    new XAttribute("rel", "self"),
                    new XAttribute("type", "application/atom+xml")));

            if(feed.SiteUrl != null)
                feedElement.Add(new XElement(nsAtom + "link",
                    new XAttribute("href", feed.SiteUrl.AbsoluteUri),
                    new XAttribute("rel", "alternate"),
                    new XAttribute("type", "text/html")));

            foreach (var item in feed.Items) {
                var element = new XElement(nsAtom + "entry");

                if(string.IsNullOrWhiteSpace(item.Title) || string.IsNullOrWhiteSpace(item.Body) || string.IsNullOrWhiteSpace(item.Permalink) || !item.PublishDate.HasValue) {
                    // Title, Body and PublishDate must be set for an item to be valid. There are alternatives for this
                    // in the specification, but those won't be supported for now.
                    // A logger would be nice here
                    continue;
                }

                if(item.Author?.Name != null) {
                    var author = new XElement(nsAtom + "author", new XElement(nsAtom + "name", item.Author.Name));

                    author.AddOptionalElement(nsAtom + "uri", item.Author.Link);
                    author.AddOptionalElement(nsAtom + "email", item.Author.Email);

                    element.Add(author);
                }

                
                element.Add(new XElement(nsAtom + "title", item.Title));
                element.Add(new XElement(nsAtom + "updated", item.PublishDate.ToRFC3339()));
                element.Add(new XElement(nsAtom + "id", item.Permalink));
                element.Add(new XElement(nsAtom + "content", item.Body, new XAttribute("type", "html")));

                element.AddOptionalElement(nsAtom + "link", item.Link);
                element.AddOptionalElement(nsAtom + "published", item.PublishDate.ToRFC3339());

                foreach(var category in feed.Categories)
                    feedElement.Add(new XElement(nsAtom + "category", new XAttribute("term", category)));

                feedElement.Add(element);
            }

            return document;
        }
    }
}