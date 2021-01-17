using System;

namespace GridFilters.Tests.SampleDataHelpers
{
    public class SampleData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public DateTime Creation { get; set; }

        public SampleData()
        { }

        public SampleData(Guid id, string name, int amount, double price, DateTime creation)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Price = price;
            Creation = creation;
        }
    }
}
