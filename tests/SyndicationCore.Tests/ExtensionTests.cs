using System;
using System.Linq;
using Xunit;
using SyndicationCore;

namespace SyndicationCore.Tests {
    public class ExtensionTests {
        [Theory]
        [InlineData("2013-10-11T14:35:21", "Fri, 11 Oct 2013 14:35:21")]
        [InlineData("2017-05-10T20:30:00", "Wed, 10 May 2017 20:30:00")]
        public void RFC822StringsAreGeneratedCorrectly(string input, string expected) {
            var date = DateTime.Parse(input);
            
            Assert.Equal(expected, date.ToRFC822().Substring(0, expected.Length));
        }
    }
}
