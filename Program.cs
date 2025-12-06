
using AdventOfCode10;

if (args.Any())
{
    //ElseResults.Create(@"C:\Projekt\AdventOfCode3\Input\Else.json");
    var json = args.First() == "Web" ? ElseResults.GetTopList(args[1]) : File.ReadAllText(args.First());
    if (json != "")
        ElseResults.Create(json, args.Any(a => a.Equals("times", StringComparison.CurrentCulture)));

    Console.WriteLine();
    Console.WriteLine($"Source: {args.First()}");
    return;
}

new AdventOfCode10.Aoc2025.Day06().Run();
