using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib.Web.Helpers;

namespace MyBlog.Tests.MyLibWeb.Helpers
{
    [TestClass]
    public class UriHelperTests
    {
        [TestMethod]
        public void ToFriendly01()
        {
            String source = "Déplacer Visual Studio Team Services d’une zone géographique à une autre";
            String expected = "deplacer-visual-studio-team-services-dune-zone-geographique-a-une-autre";
            String actual = UriHelper.ToFriendly(source);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFriendly02()
        {
            String source = "D&eacute;placer Visual Studio Team Services d&acute;une zone g&eacute;ographique &agrave; une autre";
            String expected = "deplacer-visual-studio-team-services-d-une-zone-geographique-a-une-autre";
            String actual = UriHelper.ToFriendly(source);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToFriendly03()
        {
            String source = "J&#233;r&#233;my Jeanson";
            String expected = "jeremy-jeanson";
            String actual = UriHelper.ToFriendly(source);
            Assert.AreEqual(expected, actual);
        }
    }
}
