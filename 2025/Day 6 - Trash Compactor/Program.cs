using System.Text;

const string testDataSet = "CephalopodTestDataSet.txt";
const string realDataSet = "CephalopodDataSet.txt";

DoWork();
return;

void DoWork()
{
    Console.WriteLine("==============Test Data Set==============");
    // Part 1: 4277556
    // Part 2: 3263827
    ProcessTestData();
    Console.WriteLine("==============Live Data Set==============");
    // Part 1: 5524274308182
    // Part 2: 8843673199391
    ProcessLiveData();
    Console.WriteLine("==================Done!==================");
}

void ProcessTestData() => Process(ReadAllFilesLines(testDataSet));

void ProcessLiveData() => Process(ReadAllFilesLines(realDataSet));

void Process(string[] input)
{
    Console.WriteLine($"Part 1 - Cephalopod Math Total:        {PartOne(input)}");
    Console.WriteLine($"Part 2 - Cephalopod Actual Math Total: {PartTwo(input)}");
}

long PartOne(string[] input)
{
    var data = input.Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToList();
    var length = data.First().Length;
    long total = 0;

    for (var col = 0; col < length; col++)
    {
        var calculation = data
            .Select(line => line.Skip(col).First())
            .ToList();
        total += Calculate(calculation);
    }
    return total;
}

long PartTwo(string[] data)
{
    var length = data.First().Length;

    long total = 0;
    List<int> nums = [];
    var op = ' ';

    for (var i = 0; i < length; i++)
    {
        var result = GetValueByColumn(data, i, out var foundOp);
        
        if (foundOp != ' ')
            op = foundOp;
        // Check if we're in the blank column, if so we can calculate the previous set of numbers.
        if (result == -1)
        {
            total += op == '*' 
                ? nums.Aggregate(1L, (a, b) => a * b) 
                : nums.Sum();
            // Reset the operator and the found numbers.
            op = ' ';
            nums.Clear();
        }
        else
        {
            nums.Add(result);
        }
        // Finally , we're at the end of the file.
        if (i == length - 1)
        {
            total += op == '*' ? nums.Aggregate(1, (a, b) => a * b) : nums.Sum();
        }
    }

    return total;
}

string[] ReadAllFilesLines(string file) => File.ReadAllLines(file);

long Calculate(IList<string> calculation)
{
    var op = calculation[^1];
    long total = op == "*" ? 1 : 0;
    
    foreach (var num in calculation)
    {
        if (!int.TryParse(num, out var n)) 
            continue;
        
        if (op == "*")
            total *= n;
        else
            total += n;
    }
    return total;
}

int GetValueByColumn(string[] data, int index, out char op)
{
    // Attempt to read the operator at the end of the column, if it's there.
    op = data[^1][index];
    // Are we in an empty column?
    if (data.All(line => line[index] == ' '))
        return -1;
    var num = new StringBuilder();
    // Build the correct number, top to bottom in the current column.
    foreach (var line in data)
    {
        if (line[index] == ' ' || line[index] == '*' || line[index] == '+')
            continue;
        num.Append(line[index]);
    }
    return int.Parse(num.ToString());
}
