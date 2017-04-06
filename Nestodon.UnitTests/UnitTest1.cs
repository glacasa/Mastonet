using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nestodon.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetAccount()
        {
            var client = new NestodonClient("mastodon.cx");
            Assert.IsTrue(true);
        }
    }
}
