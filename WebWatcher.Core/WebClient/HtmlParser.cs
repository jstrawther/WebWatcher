using HtmlAgilityPack;

namespace WebWatcher.Core.WebClient
{
    public class HtmlParser
    {
        public static string SelectNode(string html, string xpath)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var selectedNode = htmlDoc.DocumentNode.SelectSingleNode(xpath);

            return selectedNode.OuterHtml.ToString();
        }
    }
}
