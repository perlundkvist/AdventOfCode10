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
                var lights = new List<int>();
                var buttons = new List<List<int>>();
                foreach (var part in parts)
                {
                    if (part.StartsWith("["))
                    {
                        lights.AddRange(part[1..^1].AllIndexesOf("#"));
                    }
                    else if (part.StartsWith("("))
                    {
                        var bParts = part[1..^1].Split(',');
                        buttons.Add(bParts.Select(b => int.Parse(b)).ToList());
                    }
                }
                var least = long.MaxValue;
                var wanted = lights.ToList();
                foreach (var button in buttons)
                {
                    least = GetLeastPresses(wanted, lights, button, buttons, least, 0);
                }
            }


            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Total: {total}.");

            sw.Restart();

            Console.WriteLine($"Result in {sw}");

        }

        private long GetLeastPresses(List<int> wanted, List<int> lights, List<int> button, List<List<int>> buttons, long least, long presses)
        {
            if (presses >= least)
                return least;

            if (button.All(b => !wanted.Contains(b)))
            {
                return least;
            }

            var newLights = new List<int>();

            foreach (var press in button)
            {
            }

            if (newLights.All(n => wanted.Contains(n)))
            {
                return presses + 1;
            }
            foreach (var button2 in buttons.Where(b => b != button).ToList())
            {
                least = GetLeastPresses(wanted, newLights, button2, buttons, least, presses + 1);
            }
            return least;
        }
    }
}
