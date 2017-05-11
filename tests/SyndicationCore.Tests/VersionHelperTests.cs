using System;
using System.Linq;
using Xunit;
using SyndicationCore;

namespace SyndicationCore.Tests {
    public class VersionHelperTests {
        [Fact]
        public void VersionForType() {
            var version = VersionHelper.ForType<VersionHelperTests>();
            
            Assert.Equal("1.0.0-tests", version);
        }
    }
}
