using PoorMansGrid.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using PoorMansGrid.FilterTypes;

namespace PoorMansGrid.Tests
{
    public class FilterNumber
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
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "equals", Filter = 3 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount == 3);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsFalse(result.Items.Any(x => x.Amount == -300));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 1));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 2));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 3));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 4));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 100));
        }

        [Test]
        public void NotEquals()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "notEquals", Filter = 3 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount != 3);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsTrue(result.Items.Any(x => x.Amount == -300));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 1));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 2));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 3));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 4));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 100));
        }

        [Test]
        public void lessThan()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "lessThan", Filter = 3 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount < 3);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsTrue(result.Items.Any(x => x.Amount == -300));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 1));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 2));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 3));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 4));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 100));
        }

        [Test]
        public void lessThanOrEqual()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "lessThanOrEqual", Filter = 3 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount <= 3);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsTrue(result.Items.Any(x => x.Amount == -300));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 1));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 2));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 3));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 4));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 100));
        }

        [Test]
        public void greaterThan()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "greaterThan", Filter = 3 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount > 3);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsFalse(result.Items.Any(x => x.Amount == -300));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 1));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 2));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 3));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 4));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 100));
        }

        [Test]
        public void greaterThanOrEqual()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "greaterThanOrEqual", Filter = 3 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount >= 3);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsFalse(result.Items.Any(x => x.Amount == -300));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 1));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 2));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 3));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 4));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 100));
        }

        [Test]
        public void inRange()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Amount", new FilterModel { FieldType = "number", Type = "inRange", Filter = 2, FilterTo = 10 } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Amount >= 2 && x.Amount <= 10);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsFalse(result.Items.Any(x => x.Amount == -300));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 1));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 2));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 3));
            Assert.IsTrue(result.Items.Any(x => x.Amount == 4));
            Assert.IsFalse(result.Items.Any(x => x.Amount == 100));
        }

    }
}