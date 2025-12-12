using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day10 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_10s");

            var total = 0L;

            foreach (var line in input)
            {
                var parts = line.Split(' '); 
                var wanted = new List<int>();
                var buttons = new List<List<int>>();
                var goals = new List<int>();
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
                    else if (part.StartsWith("{"))
                    {
                        var gParts = part[1..^1].Split(',');
                        goals.AddRange(gParts.Select(g => int.Parse(g)).ToList());
                    }
                }
                var least = long.MaxValue;
                foreach (var button in buttons)
                {
                    var testing = $"({string.Join(",", button)})";
                    //least = GetLeastPresses(wanted, [], button, buttons, least, 0, new HashSet<(List<int>, List<int>, long)>());
                    least = GetLeastPresses2(goals.ToArray(), new int[goals.Count], button, buttons, least, 0, new HashSet<(List<int>, int[], long)>());
                }
                Console.WriteLine($"Least: {least}. Line {input.IndexOf(line) + 1} of {input.Count}");
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


        private long GetLeastPresses2(int[] goals, int[] current, List<int> button, List<List<int>> buttons, long least, int presses, HashSet<(List<int>, int[], long)> results)
        {
            if (presses >= least)
                return least;

            var result = results.FirstOrDefault(r => r.Item1.SequenceEqual(button) && r.Item2.SequenceEqual(current));
            if (result != default)
                return presses + result.Item3;

            var newCurrent = new int[current.Length];
            for (var i = 0; i < current.Length; i++)
            {
                newCurrent[i] = current[i] + (button.Contains(i) ? 1 : 0);
                if (newCurrent[i] > goals[i])
                    return least;
            }

            if (newCurrent.SequenceEqual(goals))
            {
                return presses + 1;
            }

            
            foreach (var button2 in buttons)
            {
                least = GetLeastPresses2(goals, newCurrent, button2, buttons, least, presses + 1, results);
                results.Add((button2, newCurrent, least));
            }

            return least;
        }
    }
}
