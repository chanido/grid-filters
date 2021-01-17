using GridFilters.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Linq;

namespace GridFilters.Tests
{
    public class Pagination
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
        public void ReturnValuesWithEmptyOptions()
        {
            var result = filterService.Filter(allItems, new FilterOptions());

            Assert.IsTrue(allItems.Count() > 0);
            Assert.AreEqual(allItems.Count(), result.TotalItems);
            Assert.AreEqual(allItems.Count(), result.Items.Count());
        }

        [Test]
        public void ReturnValuesPaged()
        {
            var options = new FilterOptions
            { StartRow = 2, EndRow = 3 };

            var result = filterService.Filter(allItems, options);


            Assert.AreEqual(allItems.Count(), result.TotalItems);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(result.Items.First(), allItems.ElementAt(2));
        }
    }
}