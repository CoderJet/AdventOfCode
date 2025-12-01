const int minDial = 0;
const int maxDial = 100;
const int startingDial = 50;

List<string> combinations = [];

Console.WriteLine("Reading combination file...");
ReadCombinationFile();
Console.WriteLine("Calculating Part 1");
// Result: 1105
Part1();
Console.WriteLine("Calculating Part 2");
// Result: 6599
Part2();
Console.WriteLine("Done!");
return;

void Part1()
{
    var dial = startingDial;
    var zeroCounter = 0;
    
    foreach (var line in combinations)
    {
        var dir = GetDirection(line[0]);
        var turnValue = GetDialValue(line[1..]);
        
        if (turnValue <= 0)
            continue;
        dial += dir * turnValue;
        dial %= maxDial;
        
        if (dial < minDial) 
            dial += maxDial;
        if (dial == minDial) 
            zeroCounter++;
    }
    Console.WriteLine($"Part 1 - Zeroes Counted: {zeroCounter}");
}

void Part2()
{
    var dial = startingDial;
    var zeroCounter = 0;
    
    foreach (var line in combinations)
    {
        var dir = GetDirection(line[0]);
        var turnValue = GetDialValue(line[1..]);
        
        if (turnValue <= 0)
            continue;
        var distToZero = dir == 1 ? maxDial - dial : dial;
        
        if (distToZero > 0 && turnValue >= distToZero) 
            zeroCounter++;
        zeroCounter += (turnValue - distToZero) / maxDial;
            
        dial += dir * turnValue;
        dial %= maxDial;
        
        if (dial < minDial)
            dial += maxDial;
    }
    Console.WriteLine($"Part 2 - Zeroes Counted: {zeroCounter}");
}

#region Supporting Functions

void ReadCombinationFile()
{
    var reader = new StringReader(File.ReadAllText("Combinations.txt"));

    while (reader.Peek() >= 0)
        combinations.Add(reader.ReadLine() ?? string.Empty);
}

int GetDirection(char value) => value == 'R' ? 1 : -1;

int GetDialValue(string value)
{
    if (!int.TryParse(value, out var turnValue)) 
        Console.WriteLine($"Skipping line value: {value}");
    return turnValue;
}

#endregion