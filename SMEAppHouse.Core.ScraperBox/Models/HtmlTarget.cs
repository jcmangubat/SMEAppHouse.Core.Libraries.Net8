namespace SMEAppHouse.Core.ScraperBox.Models
{
    public class HtmlTarget
    {
        public int PageNo { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public bool PageIsInvalid { get; set; }
        public HtmlSource Source { get; set; }

        public HtmlTarget()
        {
        }

        public HtmlTarget(int pageNo, string url, string content)
        {
            PageNo = pageNo;
            Url = url;
            Content = content;
            PageIsInvalid = false;
        }
    }
}

