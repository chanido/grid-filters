using PoorMansGrid.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PoorMansGrid.Extensions;

namespace PoorMansGrid.Tests
{
    public class FilterText
    {
        private ServiceProvider serviceProvider;
        private IFilterService filterService;
        private IQueryable<SampleData> allItems;

        [SetUp]
        public void Setup()
        {
            serviceProvider = new ServiceCollection()
                .AddPoorMansGrid()
                //.AddPoorMansGrid(PoorMansGridOptions.ForceCaseInsensitive) if you want to force the text queries to be case insensitive
                .BuildServiceProvider();

            filterService = serviceProvider.GetService<FilterService>();

            //Alternatively you can create a new instance of the BlitzCache directly without dependency injection
            //filterService = new FilterService();
            //filterService = new FilterService(PoorMansGridOptions.ForceCaseInsensitive);

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
        public void EqualsCaseInsensitive()
        {
            filterService = new FilterService(PoorMansGridOptions.ForceCaseInsensitive);

            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "equals", Filter = "test1" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name.ToLower() == "test1");
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Test2"));
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
        public void ContainsCaseInsensitive()
        {
            filterService = new FilterService(PoorMansGridOptions.ForceCaseInsensitive);

            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Name", new FilterModel { FieldType = "text", Type = "contains", Filter = "test" } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Name.ToLower().Contains("test"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test1"));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Test2"));
            Assert.IsFalse(result.Items.Any(x => x.Name == "Different"));
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