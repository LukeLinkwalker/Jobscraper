using JobScraper.Model.Data;
using JobScraper.Model.Filter;
using JobScraper.Utils;
using JobScraper.ViewModel;
using JobScraper.ViewModel.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper_Test
{
    public class Filter
    {
        List<Keyword>? keywords = null;
        List<Ad>? ads = null;
        JobScraper.ViewModel.Filter? filter = null;

        [SetUp]
        public void Setup()
        {
            keywords = new List<Keyword>();
            ads = new List<Ad>();
            filter = new JobScraper.ViewModel.Filter(null, ads);

            ads.Add(new Ad { 
                Content = "Lorem ipsum dolor sit amet",
                Timestamp = DateTime.Now.ToString()
            });

            ads.Add(new Ad {
                Content = "Maecenas pharetra convallis posuere morbi leo urna molestie at.",
                Timestamp = DateTime.Now.ToString()
            });

            ads.Add(new Ad {
                Content = "Placerat orci nulla pellentesque dignissim.",
                Timestamp = DateTime.Now.ToString()
            });

            ads.Add(new Ad {
                Content = "Amet risus nullam eget felis eget nunc lobortis.",
                Timestamp = DateTime.Now.ToString()
            });
        }

        [Test, Order(0)]
        public void KeywordAdding()
        {
            Assert.That(filter.GetKeywords().Count, Is.EqualTo(0));

            FilterArgs args = new FilterArgs();

            args.keyword = new Keyword
            {
                text = "Testing",
                type = Keyword.Type.None
            };

            filter.AddKeyword(args);

            Assert.That(filter.GetKeywords().Count, Is.EqualTo(1));
        }

        [Test, Order(1)]
        public void KeywordOverwriting()
        {
            FilterArgs args = new FilterArgs();

            args.keyword = new Keyword
            {
                text = "Testing",
                type = Keyword.Type.None
            };

            filter.AddKeyword(args);

            var keywords = filter.GetKeywords();
            Keyword? keyword = keywords.Where(keyword => keyword.type == Keyword.Type.None).SingleOrDefault();
        
            Assert.That(keyword.type, Is.EqualTo(Keyword.Type.None));
        
            args.keyword = new Keyword
            {
                text = "Testing",
                type = Keyword.Type.Must
            };
        
            filter.AddKeyword(args);
        
            keywords = filter.GetKeywords();
            keyword = keywords.Where(keyword => keyword.type == Keyword.Type.Must).SingleOrDefault();
        
            Assert.That(keyword.type, Is.EqualTo(Keyword.Type.Must));
        }

        [Test, Order(2)]
        public void KeywordRemoving()
        {
            Assert.That(filter.GetKeywords().Count, Is.EqualTo(0));

            FilterArgs args = new FilterArgs();

            args.keyword = new Keyword
            {
                text = "Testing",
                type = Keyword.Type.None
            };

            filter.AddKeyword(args);

            Assert.That(filter.GetKeywords().Count, Is.EqualTo(1));

            filter.RemoveKeyword(args);

            Assert.That(filter.GetKeywords().Count, Is.EqualTo(0));
        }

        [Test, Order(100)]
        public void FilteringShould()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Should
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(4));

            List<Ad> adsWithKeyword = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Should).Count() > 0).ToList();
            Assert.That(adsWithKeyword.Count, Is.EqualTo(2));
        }

        [Test, Order(101)]
        public void FilteringCannot()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Cannot
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(2));

            List<Ad> adsWithKeyword = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Cannot).Count() > 0).ToList();
            Assert.That(adsWithKeyword.Count, Is.EqualTo(0));
        }

        [Test, Order(102)]
        public void FilteringMust()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Must
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(2));

            List<Ad> adsWithKeyword = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Must).Count() > 0).ToList();
            Assert.That(adsWithKeyword.Count, Is.EqualTo(2));
        }

        [Test, Order(103)]
        public void FilteringMustCannot()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Must
            };
            keywords.Add(keyword);

            keyword = new Keyword
            {
                text = "Lorem",
                type = Keyword.Type.Cannot
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(1));
        }

        [Test, Order(104)]
        public void FilteringMustShould()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Must
            };
            keywords.Add(keyword);

            keyword = new Keyword
            {
                text = "Lorem",
                type = Keyword.Type.Should
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(2));

            List<Ad> adsWithKeyword = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Should).Count() > 0).ToList();
            Assert.That(adsWithKeyword.Count, Is.EqualTo(1));
        }

        [Test, Order(104)]
        public void FilteringCannotShould()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Should
            };
            keywords.Add(keyword);

            keyword = new Keyword
            {
                text = "Lorem",
                type = Keyword.Type.Cannot
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(3));

            List<Ad> adsWithKeyword = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Should).Count() > 0).ToList();
            Assert.That(adsWithKeyword.Count, Is.EqualTo(1));
        }

        [Test, Order(200)]
        public void FilteringWithoutKeywords()
        {
            List<Ad> filteredAds = filter.FilterAds(ads, keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(4));
        }

        [Test, Order(201)]
        public void FilteringWithoutAds()
        {
            Keyword keyword = new Keyword
            {
                text = "Amet",
                type = Keyword.Type.Should
            };
            keywords.Add(keyword);

            List<Ad> filteredAds = filter.FilterAds(new List<Ad>(), keywords);
            Assert.That(filteredAds.Count, Is.EqualTo(0));
        }

        [Test, Order(202)]
        public void FilteringWithoutAdsAndKeywords()
        {
            List<Ad> filteredAds = filter.FilterAds(new List<Ad>(), new List<Keyword>());
            Assert.That(filteredAds.Count, Is.EqualTo(0));
        }
    }
}