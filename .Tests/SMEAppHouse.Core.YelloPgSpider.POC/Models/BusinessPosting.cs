using SMEAppHouse.Core.CodeKits;
using System;
using System.Collections.Generic;

namespace SMEAppHouse.Core.YelloPgSpider.POC.Models
{
    public enum SocialMediaLinksEnum
    {
        Facebook, LinkedIn, GitHub, Twitter,
    }

    public enum CalendarDaysEnum
    {
        Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
    }

    public class BusinessPosting
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string LocationAddress { get; set; }

        public string EmailAddress { get; set; }
        public int AvrgRating { get; set; }
        public string WebSite { get; set; }
        public string MobilePhone { get; set; }
        public string LinePhone { get; set; }
        public Tuple<double, double> Coordinate { get; set; }

        public string BusinessLogo { get; set; }
        public string BusinessOverview { get; set; }

        public List<string> Tags { get; set; }
        public List<string> Offers { get; set; }
        public List<string> Credentials { get; set; }
        public List<string> Associations { get; set; }
        public List<string> Qualifications { get; set; }
        public List<Tuple<SocialMediaLinksEnum, string>> SocialMediaLinks { get; set; }

        public List<Tuple<CalendarDaysEnum, TimeSpan, TimeSpan>> BusinessHours { get; set; }
        public List<string> PaymentMethods { get; set; }

        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string Remarks { get; set; }
        public int Rating { get; set; }
    }

    public class YellowPageReferences
    {
        public YellowPageReferences() {
        }

        public class Reference
        {
            public Rules.WorldCountriesEnum CountryYellowPage { get; set; }
        }

        //https://www.yellowpages.com.au
    }

}
