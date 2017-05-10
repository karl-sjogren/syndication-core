using System;
using System.Xml.Linq;

namespace SyndicationCore {
    public interface ISyndicationGenerator {
        XDocument Generate(SyndicationFeed feed);
    }
}