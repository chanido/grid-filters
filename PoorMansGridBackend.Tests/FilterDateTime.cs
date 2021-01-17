using PoorMansGridBackend.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace PoorMansGridBackend.Tests
{
    public class FilterDateTime
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
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "equals", Filter = Constants.Today } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation == Constants.Today);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.NextYear));
        }

        [Test]
        public void NotEquals()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "notEquals", Filter = Constants.Today } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation != Constants.Today);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.NextYear));
        }

        [Test]
        public void LessThan()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "lessThan", Filter = Constants.Today } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation < Constants.Today);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());
            
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.NextYear));
        }

        [Test]
        public void LessThanOrEqual()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "lessThanOrEqual", Filter = Constants.Today } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation <= Constants.Today);
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.NextYear));
        }

        [Test]
        public void GreaterThan()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "greaterThan", Filter = Constants.Today } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation < Constants.Today);

            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.NextYear));
        }

        [Test]
        public void GreaterThanOrEqual()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "greaterThanOrEqual", Filter = Constants.Today } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation < Constants.Today);

            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.NextYear));
        }

        [Test]
        public void InRange()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { { "Creation", new FilterModel { FieldType = "date", Type = "inRange", Filter = Constants.Yesterday, FilterTo = Constants.NextMonth } } }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation >= Constants.Yesterday && x.Creation <= Constants.NextMonth);

            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsTrue(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.NextYear));
        }
    }
}