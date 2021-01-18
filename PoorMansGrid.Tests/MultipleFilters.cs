using PoorMansGrid.Tests.SampleDataHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace PoorMansGrid.Tests
{
    public class MultipleFilters
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
        public void DateGreaterThanOrEqualAndNameContains()
        {
            var options = new FilterOptions
            {
                FilterModels = new Dictionary<string, FilterModel> { 
                    { "Creation", new FilterModel { FieldType = "date", Type = "greaterThanOrEqual", Filter = Constants.Today } },
                    { "Name", new FilterModel { FieldType = "text", Type = "contains", Filter = "if" } },
                }
            };

            var result = filterService.Filter(allItems, options);


            var expectedValues = allItems.Where(x => x.Creation >= Constants.Today && x.Name.Contains("if"));
            Assert.AreEqual(expectedValues.Count(), result.Items.Count());

            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.LastYear));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Yesterday));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Today));
            Assert.IsFalse(result.Items.Any(x => x.Creation == Constants.Tomorrow));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different" && x.Creation == Constants.NextMonth));
            Assert.IsTrue(result.Items.Any(x => x.Name == "Different2" && x.Creation == Constants.NextYear));
        }
    }
}