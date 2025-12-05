const string testDataSet = "IngredientIdTestDataSet.txt";
const string realDataSet = "IngredientIdDataSet.txt";

DoWork();
return;

void DoWork()
{
    Console.WriteLine("==============Test Data Set==============");
    ProcessTestData();
    Console.WriteLine("==============Live Data Set==============");
    ProcessLiveData();
    Console.WriteLine("==================Done!==================");
}

void ProcessTestData() => Process(ReadIngredientIdData(testDataSet));

void ProcessLiveData() => Process(ReadIngredientIdData(realDataSet));

void Process(string input)
{
    var processor = new IngredientsProcessor(input);
 
    Console.WriteLine($"Part 1 - Total Fresh Ingredients: {processor.CountFreshIngredients()}");
    Console.WriteLine($"Part 2 - Total Fresh Id Ranges  : {processor.CountUniqueFreshIngredients()}");
}

string ReadIngredientIdData(string file) => File.ReadAllText(file);

internal class IngredientsContainer
{
    internal IReadOnlyList<(long Start, long End)> IdRanges => _idRanges.AsReadOnly();
    internal IReadOnlyList<long> Ids => _ids.AsReadOnly();
    
    private readonly List<(long Start, long End)> _idRanges;
    private readonly List<long> _ids;
    
    private const string BlankRowCharacters = "\r\n\r\n";

    public IngredientsContainer(string rawData)
    {
        var input = rawData.Split([BlankRowCharacters], StringSplitOptions.RemoveEmptyEntries);
    
        _idRanges = input[0].Split(Environment.NewLine).Select(r => {
            var section = r.Split('-');
            return (Start: long.Parse(section[0]), End: long.Parse(section[1]));
        }).ToList();
        _ids = input[1].Split(Environment.NewLine).Select(long.Parse).ToList();
    }
}

internal class IngredientsProcessor(string rawData)
{
    private readonly IngredientsContainer _container = new(rawData);

    public int CountFreshIngredients() => 
        _container.Ids.Count(number => _container.IdRanges.Any(r => number >= r.Start && number <= r.End));

    public long CountUniqueFreshIngredients()
    {
        var sortedIds = _container.IdRanges.ToList() 
            ?? throw new ArgumentNullException($"{nameof(IngredientsContainer)}.IsRanges");

        sortedIds.Sort((a, b) => a.Start.CompareTo(b.Start));
        var concatenatedRanges = new List<(long Start, long End)>();
 
        foreach (var range in sortedIds)
        {
            // Check for a unique range, either add it or update the last band's end value.
            if (concatenatedRanges.Count == 0 || concatenatedRanges[^1].End < range.Start)
            {
                concatenatedRanges.Add(range);
            }
            else
            {
                var last = concatenatedRanges[^1];
                concatenatedRanges[^1] = (last.Start, Math.Max(last.End, range.End));
            }
        }
        return concatenatedRanges.Sum(r => r.End - r.Start + 1);
    }
}
