using System.Diagnostics;

namespace AdventOfCode10.Aoc2025
{
    internal class Day05 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_05");
            var ranges = new List<(long, long)>();
            var ranges2 = new List<(long, long)>();
            var fresh = 0L;
            var test = true;

            foreach (var line in input)
            {
                if (line.Contains('-'))
                {
                    var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    var start = long.Parse(parts[0]);
                    var end = long.Parse(parts[1]);
                    ranges.Add((start, end));
                    var overlaps = ranges2.Where(r => (start >= r.Item1 && start <= r.Item2) || (end >= r.Item1 && end <= r.Item2) || (start <= r.Item1 && end >= r.Item2));
                    if (!overlaps.Any())
                    {
                        ranges2.Add((start, end));
                        continue;
                    }
                    var newStart = overlaps.OrderBy(r => r.Item1).First().Item1;
                    var newEnd = overlaps.OrderBy(r => r.Item2).Last().Item2;
                    var newRange = (Math.Min(start, newStart), Math.Max(end, newEnd));
                    overlaps.ToList().ForEach(r => ranges2.Remove(r));
                    ranges2.Add(newRange);
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    var ingredient = long.Parse(line);
                    var inRange = ranges.Any(r => ingredient >= r.Item1 && ingredient <= r.Item2);
                    if (inRange)
                        fresh++;
                }
            }

            Console.WriteLine($"First result in {sw}");
            Console.WriteLine($"Fresh: {fresh}");

            sw.Restart();
            fresh = 0L;
            foreach (var range in ranges2.OrderBy(r => r.Item1))
            {
                if (test) 
                    Console.WriteLine($"Range {range}. Diff {(range.Item2 - range.Item1 + 1)}");
                fresh += (range.Item2 - range.Item1 + 1);
            }


            Console.WriteLine($"Second result in {sw}");
            Console.WriteLine($"Fresh: {fresh} Not 357070595481281 ");

        }

    }
}
