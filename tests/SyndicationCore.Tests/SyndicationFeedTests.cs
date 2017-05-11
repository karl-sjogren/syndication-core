using System;
using System.Linq;
using Xunit;
using SyndicationCore;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SyndicationCore.Tests {
    public class SyndicationFeedTests {
        [Fact]
        public void RssGeneratorThrowsOnMissingTitle() {
            var feed = new SyndicationFeed {
                Description = "Test description",
                SiteUrl = new Uri("http://localhost/")
            };

            var rssGenerator = new Rss20SyndicationGenerator();
            Assert.Throws<ArgumentException>(() => rssGenerator.Generate(feed));
        }

        [Fact]
        public void RssGeneratorThrowsOnMissingDescription() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
                SiteUrl = new Uri("http://localhost/")
            };

            var rssGenerator = new Rss20SyndicationGenerator();
            Assert.Throws<ArgumentException>(() => rssGenerator.Generate(feed));
        }

        [Fact]
        public void RssGeneratorThrowsOnMissingSiteUrl() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
                Description = "Test description"
            };

            var rssGenerator = new Rss20SyndicationGenerator();
            Assert.Throws<ArgumentException>(() => rssGenerator.Generate(feed));
        }

        [Fact]
        public void RssGeneratorSetsChannelProperties() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
                Description = "Test description",
                SiteUrl = new Uri("http://localhost/"),
                Image = new Uri("http://localhost/rss.png"),
                TimeToLive = TimeSpan.FromMinutes(30),
                Language = new CultureInfo("en-UK"),
                Categories = new List<string>() { "Articles", "News" },
                FeedUrl = new Uri("http://localhost/rss/")
            };

            var rssGenerator = new Rss20SyndicationGenerator();
            var result = rssGenerator.Generate(feed);

            XNamespace nsAtom = "http://www.w3.org/2005/Atom";

            var channel = result.Element("rss").Element("channel");
            Assert.Equal("Test feed", channel.Element("title")?.Value);
            Assert.Equal("Test description", channel.Element("description")?.Value);
            Assert.Equal("http://localhost/", channel.Element("link")?.Value);
            Assert.Equal("http://localhost/rss.png", channel.Element("image")?.Value);
            Assert.Equal("30", channel.Element("ttl")?.Value);
            Assert.Equal("en-UK", channel.Element("language")?.Value);
            Assert.Equal("Articles / News", channel.Element("category")?.Value);
            Assert.Equal("http://localhost/rss/", channel.Element(nsAtom + "link")?.Attribute("href")?.Value);
        }

        [Fact]
        public void RssGeneratorSetsItemProperties() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
                Description = "Test description",
                SiteUrl = new Uri("http://localhost/"),
                FeedUrl = new Uri("http://localhost/rss/"),
                Items = new List<SyndicationItem> {
                    new SyndicationItem {
                        Title = "Test item",
                        Author = new Author {
                            Name = "Test person",
                            Email = "test.person@example.com",
                            Link = new Uri("http://localhost/authors/test-person")
                        },
                        Body = "Test body"
                    }
                }
            };

            var rssGenerator = new Rss20SyndicationGenerator();
            var result = rssGenerator.Generate(feed);

            var item = result.Descendants("item").FirstOrDefault();
            Assert.NotNull(item);

            Assert.Equal("Test item", item.Element("title")?.Value);

            Assert.Equal("Test feed", item.Element("source")?.Value);
            Assert.Equal("test.person@example.com (Test person)", item.Element("author")?.Value);
            Assert.Equal("http://localhost/rss/", item.Element("source")?.Attribute("url")?.Value);
        }
    }
}
