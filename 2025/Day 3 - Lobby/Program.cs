using System.Net;

const string testDataSet = "VoltagesTestDataSet.txt";
const string realDataSet = "VoltagesDataSet.txt";

List<string> voltageBanks = [];

Console.WriteLine("Reading Voltage Banks file...");
ReadVoltagesFile();
Console.WriteLine("Calculating Part 1");
// Result: 17158
var part1Sum = voltageBanks.Sum(row => BuildMaxBankVoltage(row, 2));
Console.WriteLine($"Voltage Bank Total: {part1Sum}");

Console.WriteLine("Calculating Part 2");
// Result: 170449335646486
var part2Sum = voltageBanks.Sum(row => BuildMaxBankVoltage(row, 12));
Console.WriteLine($"Voltage Bank Total: {part2Sum}");
Console.WriteLine("Done!");
return;

long BuildMaxBankVoltage(string row, int length)
{
    var max = 0L;
    var current = row.Select(c => int.Parse(c.ToString())).ToArray();
    
    for (var i = 1; i <= length; i++)
    {
        // Check all the way up to the second to last position for a max value.
        var digit = current[..^(length - i)].Max();
        max = max * 10 + digit;
        // Set the int[] to the new position, to cut down the search area.
        var index = Array.IndexOf(current, digit);
        current = current[(index + 1)..];
    }
    return max;
}

void ReadVoltagesFile()
{
    var reader = new StringReader(File.ReadAllText(realDataSet));

    while (reader.Peek() >= 0)
        voltageBanks.Add(reader.ReadLine() ?? string.Empty);
}
