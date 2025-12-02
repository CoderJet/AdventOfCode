const string testDataSet = "IdRangesTestSet.txt";
const string realDataSet = "IdRanges.txt";

List<string> idRanges = [];

Console.WriteLine("Reading id ranges file...");
ReadIdRangesFile();
Console.WriteLine("Calculating Part 1");
// Result: 12599655151
Part1();
Console.WriteLine("Calculating Part 2");
// Result: 20942028255
Part2();
Console.WriteLine("Done!");
return;

void Part1()
{
    var totalSum = 0L;
    
    foreach (var idRange in idRanges)
    {
        var from = GetRangeValue(idRange.Split('-')[0]);
        var to = GetRangeValue(idRange.Split('-')[1]);

        for (var id = from; id <= to; id++)
        {
            var idAsStr = id.ToString();
            var idLength = idAsStr.Length;
            
            if (idLength % 2 != 0)
                continue;
            
            var pattern1 = idAsStr[..(idLength / 2)];
            var pattern2 = idAsStr[(idLength / 2)..];

            if (pattern1 != pattern2) 
                continue;
            // Console.WriteLine($"Invalid Id: {idAsStr}");
            totalSum += Convert.ToInt64(idAsStr);
        }
    }
    Console.WriteLine($"Total invalid id sum: {totalSum}");
}

void Part2()
{
    long totalSum = 0;
    
    foreach (var idRange in idRanges)
    {
        var from = GetRangeValue(idRange.Split('-')[0]);
        var to = GetRangeValue(idRange.Split('-')[1]);
        
        for (var id = from; id <= to; id++)
        {
            var idAsStr = id.ToString();
            var idLength = idAsStr.Length;

            for (var j = 1; j <= idLength / 2; j++)
            {
                var index = j;
                var pattern = idAsStr[..j];
                var found = true;
                
                while (index < idLength) 
                {
                    if (index + j > idLength || !string.Equals(pattern, idAsStr.Substring(index, j)))
                    {
                        found = false;
                        break;
                    }
                    index += j;
                }

                if (!found) 
                    continue;
                // Console.WriteLine($"Invalid Id: {idAsStr}");
                totalSum += id;
                break;
            }
        }
    }
    Console.WriteLine($"Total invalid id sum: {totalSum}");
}

void ReadIdRangesFile()
{
    var reader = new StringReader(File.ReadAllText(realDataSet));

    while (reader.Peek() >= 0)
    {
        var line = reader.ReadLine();
        idRanges.AddRange(line?.Split(',') ?? []);
    }
}

long GetRangeValue(string value)
{
    if (!long.TryParse(value, out var idValue))
        Console.WriteLine($"Invalid product id value: {value}");
    return idValue;
}
