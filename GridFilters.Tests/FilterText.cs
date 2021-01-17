using GridFilters.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GridFilters.Tests
{
    public class FilterText
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
        public void Equals()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "equals", Filter = "Test1" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name == "Test1");
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsFalse(result.Items.Any(x=>x.Name == "Test2"));
        }

        [Test]
        public void NotEquals()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "notEquals", Filter = "Test1" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name != "Test1");
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different"));
        }

        [Test]
        public void Contains()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "contains", Filter = "Test" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name.Contains("Test"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Different"));
        }

        [Test]
        public void NotContains()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "notContains", Filter = "Test" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => !x.Name.Contains("Test"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different"));
        }

        [Test]
        public void StartsWith()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "startsWith", Filter = "Test" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name.StartsWith("Test"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Different"));
        }

        [Test]
        public void NotStartsWith()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "notStartsWith", Filter = "Test" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => !x.Name.StartsWith("Test"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different"));
        }

        [Test]
        public void EndsWith()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "endsWith", Filter = "2" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name.EndsWith("2"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Different"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different2"));
        }

        [Test]
        public void NotEndsWith()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "notEndsWith", Filter = "2" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => !x.Name.EndsWith("2"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Different2"));
        }
    }
}