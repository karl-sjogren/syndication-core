using System;
using System.Globalization;
using System.Linq;
using Xunit;
using SyndicationCore;

namespace SyndicationCore.Tests {
    public class ExtensionTests {
        [Theory()]
        [InlineData("2013-10-11T14:35:21Z", "Fri, 11 Oct 2013 14:35:21 +0000")]
        [InlineData("2017-05-10T20:30:00Z", "Wed, 10 May 2017 20:30:00 +0000")]
        public void RFC822StringsAreGeneratedCorrectly(string input, string expected) {
            var date = DateTime.Parse(input).ToUniversalTime();
            
            Assert.Equal(expected, date.ToRFC822());
        }

        [Theory()]
        [InlineData("2013-10-11T14:35:21Z", "Fri, 11 Oct 2013 14:35:21 +0000")]
        [InlineData("2017-05-10T20:30:00Z", "Wed, 10 May 2017 20:30:00 +0000")]
        public void RFC822StringsAreGeneratedCorrectlyWithSwedishCultureInfo(string input, string expected) {
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("sv-SE");

            var date = DateTime.Parse(input).ToUniversalTime();
            
            Assert.Equal(expected, date.ToRFC822());
        }

        [Theory()]
        [InlineData("2013-10-11T14:35:21Z", "2013-10-11T14:35:21+00:00")]
        [InlineData("2017-05-10T20:30:00Z", "2017-05-10T20:30:00+00:00")]
        public void RFC3339StringsAreGeneratedCorrectly(string input, string expected) {
            var date = DateTime.Parse(input).ToUniversalTime();
            
            Assert.Equal(expected, date.ToRFC3339());
        }
    }
}
