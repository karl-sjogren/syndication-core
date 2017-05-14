using System;
using System.Linq;
using Xunit;
using SyndicationCore;
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SyndicationCore.Tests {
    public class AtomSyndicationGeneratorTests {
        [Fact]
        public void GeneratorThrowsOnMissingTitle() {
            var feed = new SyndicationFeed {
                SiteUrl = new Uri("http://localhost/")
            };

            var generator = new AtomSyndicationGenerator();
            Assert.Throws<ArgumentException>(() => generator.Generate(feed));
        }

        [Fact]
        public void GeneratorThrowsOnMissingSiteUrl() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
            };

            var generator = new AtomSyndicationGenerator();
            Assert.Throws<ArgumentException>(() => generator.Generate(feed));
        }

        [Fact]
        public void GeneratorSetsFeedProperties() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
                Description = "Test description",
                SiteUrl = new Uri("http://localhost/"),
                Image = new Uri("http://localhost/atom.png"),
                TimeToLive = TimeSpan.FromMinutes(30),
                Language = new CultureInfo("en-UK"),
                Categories = new List<string>() { "Articles", "News" },
                FeedUrl = new Uri("http://localhost/atom/")
            };

            var generator = new AtomSyndicationGenerator();
            var result = generator.Generate(feed);

            XNamespace nsAtom = "http://www.w3.org/2005/Atom";

            var element = result.Element(nsAtom + "feed");
            Assert.Equal("Test feed", element.Element(nsAtom + "title")?.Value);
            Assert.Equal("Test description", element.Element(nsAtom + "subtitle")?.Value);
            Assert.Equal("http://localhost/atom.png", element.Element(nsAtom + "logo")?.Value);
            Assert.Equal("http://localhost/atom/", element.Element(nsAtom + "link")?.Attribute("href")?.Value);
        }


        [Fact]
        public void GeneratorSetsItemProperties() {
            var feed = new SyndicationFeed {
                Title = "Test feed",
                Description = "Test description",
                SiteUrl = new Uri("http://localhost/"),
                FeedUrl = new Uri("http://localhost/atom/"),
                Items = new List<SyndicationItem> {
                    new SyndicationItem {
                        Title = "Test item",
                        Author = new Author {
                            Name = "Test person",
                            Email = "test.person@example.com",
                            Link = new Uri("http://localhost/authors/test-person")
                        },
                        Body = "Test body",
                        Permalink = "http://localhost/test-item",
                        PublishDate = DateTime.Parse("2017-05-12 15:08:00")
                    }
                }
            };

            var generator = new AtomSyndicationGenerator();
            var result = generator.Generate(feed);

            XNamespace nsAtom = "http://www.w3.org/2005/Atom";

            var item = result.Descendants(nsAtom + "entry").FirstOrDefault();
            Assert.NotNull(item);

            Assert.Equal("Test item", item.Element(nsAtom + "title")?.Value);

            Assert.Equal("Test body", item.Element(nsAtom + "content")?.Value);
            Assert.Equal("Test person", item.Element(nsAtom + "author")?.Element(nsAtom + "name")?.Value);
            Assert.Equal("test.person@example.com", item.Element(nsAtom + "author")?.Element(nsAtom + "email")?.Value);
            Assert.Equal("http://localhost/authors/test-person", item.Element(nsAtom + "author")?.Element(nsAtom + "uri")?.Value);
        }
    }
}
