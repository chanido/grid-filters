# Poor Man's Grid
In the last two weeks I have been involved in three projects where I needed a way to process the requests of a grid and return the results paginated.

The problem is easily solved if you go to [devexpress](https://www.devexpress.com/) and buy their [grid controls](https://js.devexpress.com/Demos/WidgetsGallery/Demo/DataGrid/Overview/Angular/Light/). I have used them in other projects and they are absolutely gorgeous and I recommend them 100%... but the company I was developing for didn't want to spend any money on it. (Now the **Poor Man's** name makes sense, doesn't it?)

So I started reading the whole internet and asking colleagues and finally, after mixing and matching everything we found, we got to something generic enough to be used in all the three projects.

It is really simple to install and use. Give it a go! maybe this is all you need.

## Installation
- First download the package from nuget `Install-Package PoorMansGrid -Version 1.0.1`
- Once you have it go to your `Startup.cs` and put this line in your services `services.AddPoorMansGridFilterService();`
- If you have Automapper: Go to your `MappingProfile.cs` and add `CreateMap(typeof(FilteredResult<>), typeof(FilteredResult<>));`

And you are ready to go!


## Generating the FilterOptions
The FilterOptions is what we use request the backend a page of items.

### From the frontend
You need to create a FilterOptions object and do it properly, there are examples in the Test project but, more or less this is how it goes:

#### FilterOptions
```
startRow: number; // The index of the first item of your page
endRow: number;   // The index of the last item of your page
filterModels: {{FilterModel}[]}; // The keys of the object will be the name of the field and the value the FilterModel
sortModel: SortModel[]; // An array of SortModel
```
The key in the filterModels and the colId can be joins as in "contact.name"

#### SortModel
```
colId: string; // The name of the field we will sort for (it can be a join like "contact.name")
sort: string; // This can be ["asc" or "desc"]
```

#### FilterModel
```
fieldType:string; //  The type of the field we will be filtering ["date", "text", "number"]
type:string; // The type of filter we will be applying 
             // For text ["equals", "notEquals", "contains", "notContains", "startsWith", "notStartsWith", "endsWith", "notEndsWith"]
             // For date and numbers ["equals", "notEquals", "lessThan", "lessThanOrEqual", "greaterThan", "greaterThanOrEqual", "inRange"]
filter: any; // The value to compare the filter to (in "inRange" filters it is used as From)
filterTo: any; // In "inRange" filters it is the highest value you can reach
```

### From the backend
There are cases in which you cannot modify what the grid in the frontend is sending you. If this is your case you can extend the FilterOptions class, transform the input in order to comply with a FilterOptions file and use your class as the option parameter for the FilterService.

In fact this was my case. I will put the code here as an example.

```
    [ModelBinder(typeof(MyProjectFilterOptionsBinder))] //This is completely optional, for transforming automagically the type from the method in the controller
    public class MyProjectFilterOptions : FilterOptions
    {
        public IEnumerable<string> ExcelColumnsToExport { get; set; }

        public MyProjectFilterOptions(string agGridOptions)
        {
            var options = agGridOptions.ToDynamic();
            var filters = new RouteValueDictionary(options);

            FilterModels = new Dictionary<string, FilterModel>();
            foreach (var (key, value) in filters)
            {
                if (key == "orderBy") SortModel = new[] { new SortModel() { ColId = options.orderBy.field, Sort = options.orderBy.direction } };
                else if (key == "returnFields") ExcelColumnsToExport = DynamicExtensions.ToList<string>(options.returnFields);
                else
                {
                    if ((key.StartsWith("from") || key.StartsWith("to")) && value.IsDate()) AddDateRestriction(filters, key, value);
                    else FilterModels.Add(key, new MyProjectFilterModel(value));
                }
            }
        }

        private void AddDateRestriction(RouteValueDictionary filters, string key, object value)
        {
            var newKey = key.StartsWith("from") ? key[4..] : key[2..];
            if (FilterModels.ContainsKey(newKey)) return;

            var dateFrom = filters.Where(x => x.Key == $"from{newKey}").Select(x => x.Value).FirstOrDefault();
            var dateTo = filters.Where(x => x.Key == $"to{newKey}").Select(x => x.Value).FirstOrDefault();

            FilterModels.Add(newKey, MyProjectFilterModel.CreateDate(dateFrom, dateTo));
        }
    }
```

```
    public class MyProjectFilterModel : FilterModel
    {
        public MyProjectFilterModel() { }
        public MyProjectFilterModel(object value) : this()
        {
            FieldType = "text";
            Type = "contains";
            Filter = value;
        }

        internal static FilterModel CreateDate(object dateFrom, object dateTo) => new FilterModel
        {
            FieldType = "date",
            Type = "inRange",
            Filter = dateFrom.ToDateTime(),
            FilterTo = dateTo.ToDateTime()
        };
    }
```

```
    public class MyProjectFilterOptionsBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            using var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body);

            var valueFromBody = await streamReader.ReadToEndAsync();

            if (valueFromBody.IsNullOrEmpty()) ModelBindingResult.Success(null);

            bindingContext.Result = ModelBindingResult.Success(new MyProjectFilterOptions(valueFromBody));
        }
    }
```

Finally from the controller I can do this:

```
[HttpPost("search")]
        public IActionResult Search([FromBody] MyProjectFilterOptions options) =>
            Ok(mapper.Map<FilteredResult<RandomItemDTO>>(filterService.Filter(randomItemsRepository.GetAll(), options)));
```

## Conclusion
The generation (or translation) of the FilterOptions might be a bit complicated but hey, look at that oneliner in the controller!


