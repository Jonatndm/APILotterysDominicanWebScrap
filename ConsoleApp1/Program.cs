
using HtmlAgilityPack;
using ScrapySharp.Extensions;

HtmlWeb oweb = new HtmlWeb();
HtmlDocument doc = oweb.Load("https://www.leidsa.com/");

//HtmlNode body = doc.DocumentNode.CssSelect("css-vwyhfc").First();

List<string> numeros = new List<string>();
foreach (var node in doc.DocumentNode.CssSelect(".css-vwyhfc"))
{
    numeros.Add(node.InnerHtml);
}