using PoorMansGrid.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Linq;

namespace PoorMansGrid.Tests
{
    public class Sorting
    {
        private IFilterService filterService;
        private IQueryable<SampleData> allItems;

        [SetUp]
        public void Setup()
        {
            filterService = new FilterService();
            allItems = SampleDataFactory.GetSampleData();
        }

        [Test]
        public void ReturnValuesSorted()
        {
            var options = new FilterOptions
            {
                SortModel = new[] { new SortModel { ColId = "Name" } }
            };

            var result = filterService.Filter(allItems, options);

            
            Assert.IsTrue(allItems.OrderBy(x=>x.Name).First() == result.Items.First());
        }

        [Test]
        public void ReturnValuesSortedAsc()
        {
            var options = new FilterOptions
            {
                SortModel = new[] { new SortModel { ColId = "Name", Sort = "asc" } }
            };

            var result = filterService.Filter(allItems, options);


            Assert.IsTrue(allItems.OrderBy(x => x.Name).First() == result.Items.First());
        }

        [Test]
        public void ReturnValuesSortedDesc()
        {
            var options = new FilterOptions
            {
                SortModel = new[] { new SortModel { ColId = "Name", Sort = "desc" } }
            };

            var result = filterService.Filter(allItems, options);


            Assert.IsTrue(allItems.OrderByDescending(x => x.Name).First() == result.Items.First());
        }
    }
}