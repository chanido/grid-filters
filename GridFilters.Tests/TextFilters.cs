using GridFilters.Tests.SampleData;
using NUnit.Framework;
using System.Linq;

namespace GridFilters.Tests
{
    public class Tests
    {
        private IFilterService filterService;

        [SetUp]
        public void Setup()
        {
            filterService = new FilterService();
        }

        [Test]
        public void ReturnValuesWithEmptyOptions()
        {
            var allItems = SampleDataFactory.GetSampleData();
            var result = filterService.Filter(allItems, new FilterOptions());

            Assert.IsTrue(allItems.Count() > 0);
            Assert.IsTrue(allItems.Count() == result.TotalItems);
            Assert.IsTrue(allItems.Count() == result.Items.Count());
        }
    }
}