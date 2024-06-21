using SMEAppHouse.ScraperBox.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
// ReSharper disable EmptyGeneralCatchClause
#pragma warning disable 168

namespace SMEAppHouse.ScraperBox.Common
{
    public static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string ToJson(this Dictionary<int, List<int>> dict)
        {
            var entries = dict.Select(d => $"\"{d.Key}\": [{string.Join(",", d.Value)}]");
            return "{" + string.Join(",", entries) + "}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pgInstruction"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public static string PageNo(this PageInstruction pgInstruction, int pageNo)
        {
            if (pageNo.ToString().Length > pgInstruction.PadLength)
                throw new InvalidOperationException(
                    "Page number character length exceeds page instruction pad length.");
            var pgNo = pageNo.ToString();
            return pgInstruction.PaddingDirection == PageInstruction.PaddingDirectionsEnum.ToLeft
                ? pgNo.PadLeft(pgInstruction.PadLength, pgInstruction.PadCharacter)
                : pgNo.PadRight(pgInstruction.PadLength, pgInstruction.PadCharacter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ResolveHttpUrl(string url)
        {
            return url.Substring(0, 2).Contains("//")
                ? $"http:{url}"
                : url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="allTrim"></param>
        /// <param name="otherElementsToClear"></param>
        /// <returns></returns>
        public static string Resolve(string val, bool allTrim = false, params string[] otherElementsToClear)
        {
            val = val.Replace("&amp;", "&");
            val = val.TrimEnd(',');
            val = val.Replace("%3A", ":").Replace("%2F", "/");
            val = val.Replace("&#034;", "\"");
            val = val.Replace("&#039;", "'");
            val = val.Replace("<br />", "\n");
            val = val.Replace("<br/>", "\n");
            val = val.Replace("&nbsp;", " ");

            if (otherElementsToClear.Any())
            {
                otherElementsToClear.ToList().ForEach(e => { val = val.Replace(e, ""); });
            }

            if (allTrim) val = val.Trim();
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="allTrim"></param>
        /// <returns></returns>
        public static string CleanupHtmlStrains(string val, bool allTrim = false)
        {
            val = val.Replace("&amp;", "");
            val = val.TrimEnd(',');
            val = val.Replace("%3A", "");
            val = val.Replace("%2F", "");
            val = val.Replace("&#034;", "");
            val = val.Replace("&#039;", "");
            val = val.Replace("<br />", "");
            val = val.Replace("<br/>", "");
            val = val.Replace("&nbsp;", "");
            val = val.Replace("amp;", "");
            val = val.Replace("#shId", "");
            val = val.Replace(System.Environment.NewLine, "");
            val = val.Replace("\n", "");

            if (allTrim) val = val.Trim();
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string EncodeQueryStringSegment(string query)
        {
            return query.ToLower()
                .Trim()
                .Replace("&", "%26")
                .Replace(" ", "%20");
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="retainHttPrefix"></param>
        /// <returns></returns>
        public static string ExtractDomainNameFromUrl(string url, bool retainHttPrefix = false)
        {
            var httPrefix = string.Empty;

            if (!url.Contains(@"://"))
                return retainHttPrefix
                    ? $"{httPrefix}://{url.Split('/')[0]}"
                    : url.Split('/')[0];

            httPrefix = url.Split(new string[] { "://" }, StringSplitOptions.None)[0];
            url = url.Split(new string[] { "://" }, 2, StringSplitOptions.None)[1];

            return retainHttPrefix
                ? $"{httPrefix}://{url.Split('/')[0]}"
                : url.Split('/')[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <param name="altProxy"></param>
        /// <returns></returns>
        public static string GetPage(string targetUrl, IPProxy altProxy = null)
        {
            return GetPage(targetUrl, out _, false, altProxy, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <param name="responseHeaders"></param>
        /// <param name="useDefaultBrowserProxyIfAny"></param>
        /// <param name="altProxy"></param>
        /// <param name="proxyNetworkCredential"></param>
        /// <returns></returns>
        public static string GetPage(string targetUrl,
        out WebHeaderCollection responseHeaders,
        bool useDefaultBrowserProxyIfAny = false,
        IPProxy altProxy = null,
        NetworkCredential proxyNetworkCredential = null)
        {
            // Create a new request to the mentioned URL.				
            var myWebRequest = (HttpWebRequest)WebRequest.Create(targetUrl);
            IWebProxy myProxy = null;

            if (useDefaultBrowserProxyIfAny)
            {
                // Obtain the 'Proxy' of the  Default browser. Else, Proxy is null; no proxy will be used 
                var defProxy = myWebRequest.Proxy;
                var proxyAddress = defProxy?.GetProxy(myWebRequest.RequestUri).AbsoluteUri;
                myProxy = new WebProxy(proxyAddress);
                if (proxyNetworkCredential != null)
                    myProxy.Credentials = proxyNetworkCredential;
            }
            else if (altProxy != null)
            {
                myProxy = altProxy.ToWebProxy();
                myProxy.Credentials = altProxy.GetNetworkCredential();
            }

            if (myProxy != null)
                myWebRequest.Proxy = myProxy;
            var myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            responseHeaders = myWebResponse.Headers;

            var encoding = Encoding.ASCII;

            using (var reader =
                new System.IO.StreamReader(myWebResponse.GetResponseStream() ?? throw new InvalidOperationException(),
                    encoding))
            {
                var responseText = reader.ReadToEnd();
                return responseText;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceHtml"></param>
        /// <returns></returns>
        public static string RemoveHtmlComments(string sourceHtml)
        {
            var output = string.Empty;
            var temp = System.Text.RegularExpressions.Regex.Split(sourceHtml, "<!--");
            return (from s in temp
                    let str = string.Empty
                    select !s.Contains("-->") ? s : s.Substring(s.IndexOf("-->", StringComparison.Ordinal) + 3)
                    into str
                    where str.Trim() != string.Empty
                    select str)
                .Aggregate(output, (current, str) => current + str.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RemoveUnwantedTags(string data)
        {
            var acceptableUnwantedTags = new string[] { "strong", "em", "u" };
            return RemoveUnwantedTags(data, acceptableUnwantedTags);
        }


        /// <summary>
        /// http://stackoverflow.com/questions/12787449/html-agility-pack-removing-unwanted-tags-without-removing-content
        /// </summary>
        /// <param name="data"></param>
        /// <param name="acceptableTags"></param>
        /// <returns></returns>
        public static string RemoveUnwantedTags(string data, string[] acceptableTags)
        {
            var document = new HtmlDocument();
            document.LoadHtml(data);

            var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                if (acceptableTags.Contains(node.Name) || node.Name == "#text")
                    continue;

                var childNodes = node.SelectNodes("./*|./text()");

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {
                        nodes.Enqueue(child);
                        parentNode.InsertBefore(child, node);
                    }
                }

                parentNode.RemoveChild(node);
            }

            return document.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="valuePartial"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetInnerText(HtmlNode sourceNode,
            string element,
            string attribute,
            string valuePartial,
            string defaultValue = "")
        {
            var node = sourceNode
                .Descendants(element)
                .FirstOrDefault(d =>
                    d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(valuePartial));

            return node == null ? defaultValue : node.InnerText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tagsToRemove"></param>
        /// <returns></returns>
        public static string GetInnerText(HtmlNode node, params string[] tagsToRemove)
        {
            return GetInnerText(node, false, tagsToRemove);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="removeCommentTags"></param>
        /// <param name="tagsToRemove"></param>
        /// <returns></returns>
        public static string GetInnerText(HtmlNode node, bool removeCommentTags = true, params string[] tagsToRemove)
        {
            node.Descendants()
                //.Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n =>
                {
                    if (tagsToRemove.Contains(n.Name))
                        n.Remove();
                });

            if (!removeCommentTags) return node.InnerText;
            try
            {
                foreach (var comment in node.SelectNodes("//comment()"))
                {
                    comment.ParentNode.RemoveChild(comment);
                }
            }
            catch (Exception ex)
            {
            }

            return node.InnerText;
        }

        public static string GetInnerText(string sourceHtml, params string[] tagsToRemove)
        {
            return GetInnerText(sourceHtml, false, tagsToRemove);
        }

        public static string GetInnerText(string sourceHtml, bool removeCommentTags = true,
            params string[] tagsToRemove)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(sourceHtml);
            return GetInnerText(doc.DocumentNode, removeCommentTags, tagsToRemove);
        }

        public static HtmlNode GetNode(HtmlNode htmlNode,
            string element,
            string attribute,
            string valuePortion,
            bool tryNull = false)
        {
            if (tryNull)
            {
                try
                {
                    var nodes = htmlNode.Descendants(element)
                        .Where(d => d.Attributes.Contains(attribute) &&
                                    d.Attributes[attribute].Value.Contains(valuePortion));

                    var htmlNodes = nodes as HtmlNode[] ?? nodes.ToArray();
                    if (htmlNodes.Any())
                    {
                        var node = htmlNodes.ToArray()[0]; //.SingleOrDefault();
                        return node;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                var nodes = htmlNode.Descendants(element)
                    .Where(d => d.Attributes.Contains(attribute) &&
                                d.Attributes[attribute].Value.Contains(valuePortion));

                var htmlNodes = nodes as HtmlNode[] ?? nodes.ToArray();
                if (!htmlNodes.Any()) return null;
                var node = htmlNodes.ToArray()[0]; //.SingleOrDefault();
                return node;
            }

            return null;
        }

        public static HtmlNode GetNodeByInnerHtml(HtmlNode node,
            string element,
            //string innerHtml,
            string valuePartial)
        {
            var targetNode = node.Descendants(element).FirstOrDefault(d => d.InnerHtml.Contains(valuePartial));
            return targetNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="valuePartial"></param>
        /// <param name="tryNull"></param>
        /// <returns></returns>
        public static HtmlNode GetNodeByAttribute(HtmlNode htmlNode,
            string element,
            string attribute,
            string valuePartial,
            bool tryNull = false)
        {
            var nodes = htmlNode.Descendants(element)
                .Where(d => d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(valuePartial));

            if (tryNull)
            {
                try
                {
                    var htmlNodes = nodes as HtmlNode[] ?? nodes.ToArray();
                    if (!htmlNodes.Any()) return null;

                    var node = htmlNodes.ToArray()[0]; //.SingleOrDefault();
                    return node;

                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                var htmlNodes = nodes as HtmlNode[] ?? nodes.ToArray();
                if (!htmlNodes.Any()) return null;

                var node = htmlNodes.ToArray()[0]; //.SingleOrDefault();
                return node;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="valuePartial"></param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodeCollection(HtmlNode node,
            string element,
            string attribute,
            string valuePartial)
        {
            var nodes = node.Descendants(element)
                .Where(d => d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(valuePartial));
            return nodes;
        }

        public static IEnumerable<HtmlNode> GetNodeCollection(HtmlNode node, params string[] element)
        {
            var elements = node.Descendants();
            var nodes = elements.Where(p => element.Contains(p.Name));
            //var _nodes = node.Descendants(element).Where(d => d.Name.Contains(element));
            return nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="brute"></param>
        /// <returns></returns>
        public static bool IsUrlValid(string url, bool brute = false)
        {
            if (!brute)
            {
                return Uri.TryCreate(url, UriKind.Absolute, out _);
            }
            else
            {
                // using MyClient from linked post
                using (var client = new MyClient() {HeadOnly = true})
                {
                    try
                    {
                        _ = client.DownloadString(url);

                        // no error has occured so,
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class MyClient : WebClient
        {
            public bool HeadOnly { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var req = base.GetWebRequest(address);
                if (req != null && (HeadOnly && req.Method == "GET"))
                {
                    req.Method = "HEAD";
                }

                return req;
            }
        }
    }
}