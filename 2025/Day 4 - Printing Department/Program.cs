const string testDataSet = "PaperTestDataSet.txt";
const string realDataSet = "PaperDataSet.txt";

const char paperRollChar = '@';
const char emptySlotChar = '.';

Console.WriteLine("Reading Paper Storage file...");
var input = ReadPaperRollData();

Console.WriteLine("Calculating Part 1");
// Result: 1457
var paperCount = Part1(input);
Console.WriteLine($"{paperCount} valid paper rolls.");

Console.WriteLine("Calculating Part 2");
// Result: 8310
var paperCount2 = Part2(input);
Console.WriteLine($"{paperCount2} valid paper rolls.");
Console.WriteLine("Done!");
return;

int Part1(string[] data)
{
    var grid = ParsePaperGrid(data);
    var height = grid.Length;
    var width = grid[0].Length;
    var validPaperCount = 0;

    for (var col = 0; col < height; col++)
    {
        for (var row = 0; row < width; row++)
        {
            if (IsAccessible(grid, col, row))
                validPaperCount++;
        }
    }
    return validPaperCount;
}

int Part2(string[] data)
{
    var grid = ParsePaperGrid(data);
    var height = grid.Length;
    var width = grid[0].Length;
    var totalRemoved = 0;
    var toBeRemoved = new List<(int col, int row)>();

    do
    {
        for (var col = 0; col < height; col++)
        {
            for (var row = 0; row < width; row++)
            {
                // Same as Part1(), but we're now marking for removal.
                if (IsAccessible(grid, col, row))
                    toBeRemoved.Add((col, row));
            }
        }
        // At this point, no more paper rolls can be marked for removal.
        if (toBeRemoved.Count == 0) 
            break;
        totalRemoved += toBeRemoved.Count;
        // Change the 'to be removed' rolls into empty slots, then run it again.
        foreach (var (col, row) in toBeRemoved)
            grid[col][row] = emptySlotChar;
        toBeRemoved.Clear();

    } while (true);

    return totalRemoved;
}

string[] ReadPaperRollData() => File.ReadAllLines(testDataSet);

char[][] ParsePaperGrid(string[] lines) => [.. lines.Select(line => line.ToCharArray())];

static bool IsAccessible(char[][] grid, int row, int col) => 
    grid[row][col] == paperRollChar && CountSurrounding(grid, row, col) < 4;

static int CountSurrounding(char[][] grid, int col, int row)
{
    // Get the absolute minimum and maximum bounds of the paper storage area.
    var dy = col - 1 < 0 ? 0 : col - 1;
    var uy = col + 1 >= grid.Length ? grid.Length - 1 : col + 1;
    var lx = row - 1 < 0 ? 0 : row - 1;
    var rx = row + 1 >= grid[0].Length ? grid[0].Length - 1 : row + 1;

    var rolls = 0;

    for (var iy = dy; iy <= uy; iy++)
    {
        for (var ix = lx; ix <= rx; ix++)
        {
            // Ignore centre (self).
            if (iy == col && ix == row)
                continue;
            // Have we found a paper roll?
            if (grid[iy][ix] == paperRollChar)
                rolls++;
        }
    }
    return rolls;
}
