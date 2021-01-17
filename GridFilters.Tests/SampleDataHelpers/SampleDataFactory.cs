using System;
using System.Collections.Generic;
using System.Linq;

namespace GridFilters.Tests.SampleDataHelpers
{
    public class SampleDataFactory
    {
        public static IQueryable<SampleData> GetSampleData() => new List<SampleData>
            {
                new SampleData(Guid.NewGuid(), "Test1", 1, 3.25, Constants.LastMonth),
                new SampleData(Guid.NewGuid(), "Test2", 2, 3.75, Constants.Yesterday),
                new SampleData(Guid.NewGuid(), "Test3", 3, 4, Constants.Today),
                new SampleData(Guid.NewGuid(), "Different", 0, 4e5, Constants.NextYear),
                new SampleData(Guid.NewGuid(), "Different2", 100, 40, Constants.NextMonth)
            }.AsQueryable();

    }
}
