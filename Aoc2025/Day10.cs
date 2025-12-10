using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day10 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_10");

            var total = 0L;

            foreach (var line in input)
            {
                Console.WriteLine($"Now testing {input.IndexOf(line)} of {input.Count}");
                var parts = line.Split(' '); 
                var wanted = new List<int>();
                var buttons = new List<List<int>>();
                //var tested = new HashSet<(List<int>, List<int>, long)>();
                foreach (var part in parts)
                {
                    if (part.StartsWith("["))
                    {
                        wanted.AddRange(part[1..^1].AllIndexesOf("#"));
                    }
                    else if (part.StartsWith("("))
                    {
                        var bParts = part[1..^1].Split(',');
                        buttons.Add(bParts.Select(b => int.Parse(b)).ToList());
                    }
                }
                var least = long.MaxValue;
                foreach (var button in buttons)
                {
                    var testing = $"({string.Join(",", button)})";
                    least = GetLeastPresses(wanted, [], button, buttons, least, 0, new HashSet<(List<int>, List<int>, long)>());
                }
                Console.WriteLine($"Least: {least}.");
                total += least;
            }


            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Total: {total}.");

            sw.Restart();

            Console.WriteLine($"Result in {sw}");

        }

        private long GetLeastPresses(List<int> wanted, List<int> lights, List<int> button, List<List<int>> buttons,
            long least, long presses, HashSet<(List<int>, List<int>, long)> tested)
        {
            if (presses >= least || presses > 10)
                return least;

            //var test = tested.FirstOrDefault(t => t.Item1.SequenceEqual(button) && t.Item2.SequenceEqual(lights));
            //if (test != default)
            //    return presses + test.Item3;

            //tested.Add((button.ToList(), lights.ToList(), least));
            //test = tested.First(t => t.Item1.SequenceEqual(button) && t.Item2.SequenceEqual(lights));
            //Console.WriteLine($"({string.Join(",", button)}) ({string.Join(",", lights)})");

            var newLights = new List<int>();

            var stop = Math.Max(lights.Any() ? lights.Max() : 0, button.Max());
            for (var i = 0; i <= stop; i++)
            {
                var light = button.Contains(i) ^ lights.Contains(i);
                if (light)
                    newLights.Add(i);
            }

            if (newLights.SequenceEqual(wanted))
            {
                return presses + 1;
            }

            foreach (var button2 in buttons.Where(b => b != button).ToList())
            {
                var testing = $"({string.Join(",", button2)})";
                least = GetLeastPresses(wanted, newLights.ToList(), button2, buttons, least, presses + 1, tested);
            }
            return least;
        }
    }
}
