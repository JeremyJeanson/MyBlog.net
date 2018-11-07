using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLib.Web.Helpers;
using System;

namespace MyBlog.Tests.MyLibWeb
{
    [TestClass]
    public class WebpageHelperTests
    {
        private void GetMetaDescrition(String html, String expected)
        {
            String actual = WebpageHelper.GetMetaDescrition(html);
            Assert.IsTrue(actual.Length <= 160);
            Assert.AreEqual(expected, actual);            
        }

        [TestMethod]
        public void GetMetaDescrition1()
        {
            GetMetaDescrition("<p>Ceci est un test.</p>", "Ceci est un test.");
        }

        [TestMethod]
        public void GetMetaDescrition2()
        {
            GetMetaDescrition("<p><span style=\"color:black; font-family:Arial; font-size:10pt; background-color:white\">Le <strong>Lorem Ipsum</strong> est simplement du faux texte employé dans la composition et la mise en page avant impression. Le Lorem Ipsum est le faux texte standard de l'imprimerie depuis les années 1500, quand un peintre anonyme assembla ensemble des morceaux de texte pour réaliser un livre spécimen de polices de texte. Il n'a pas fait que survivre cinq siècles, mais s'est aussi adapté à la bureautique informatique, sans que son contenu n'en soit modifié. Il a été popularisé dans les années 1960 grâce à la vente de feuilles Letraset contenant des passages du Lorem Ipsum, et, plus récemment, par son inclusion dans des applications de mise en page de texte, comme Aldus PageMaker.</span></p>",
                "Le Lorem Ipsum est simplement du faux texte employé dans la composition et la mise en page avant impression. Le Lorem Ipsum est le faux texte standard...");
        }

        [TestMethod]
        public void GetMetaDescrition3()
        {
            GetMetaDescrition("<p><img title=\"Microsoft Most Valuable Professional\" align=\"left\" style=\"margin: 0px 10px 10px 0px; float: left; display: inline;\" alt=\"Logo Microsoft Most Valuable Professional\" src=\"https://bugshunterblog.blob.core.windows.net/posts/MVP_BlueOnly_3A789DAB.png\"></p><p>Ce dimanche, j’ai été récompensé par Microsoft qui m’a décerné un nouveau MVP Award (catégorie Visual Studio and Development Technologies).<p>Un grand merci à Microsoft et à la communauté des développeurs qui me suit. Sans eux cette belle aventure ne serait pas possible.<p>Pour fêter cela, mon blog inaugure <a title=\"Opion accessibilit&eacute; sur bugshunter.net\" href=\"https://www.bugshunter.net/Post/Details/210\" target=\"_blank\">une option d’accessibilité pour les utilisateurs dyslexiques</a>.",
                "Ce dimanche, j’ai été récompensé par Microsoft qui m’a décerné un nouveau MVP Award (catégorie Visual Studio and Development Technologies).Un grand merci...");
        }

        [TestMethod]
        public void GetMetaDescrition4()
        {
            GetMetaDescrition(" ",
                String.Empty);
        }

        [TestMethod]
        public void GetMetaDescrition5()
        {
            GetMetaDescrition(null,
                String.Empty);
        }
    }
}
