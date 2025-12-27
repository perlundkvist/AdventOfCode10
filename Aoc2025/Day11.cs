using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day11 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_11");

            var total = 0L;

            var devices = new Dictionary<string, List<string>>();

            foreach (var line in input)
            {
                var parts = line.Split(":");
                devices[parts[0]] = parts[1].Trim().Split(" ").ToList();
            }

            if (devices.ContainsKey("you"))
            {
                var start = devices["you"];
                var visited = new Dictionary<string, long>();
                total = GetPaths(start, devices, "out", visited);

                Console.WriteLine($"Result in {sw}");
                Console.WriteLine($"Total: {total}.");
            }

            Console.WriteLine();

            sw.Restart();
            total = 0L;
            if (devices.ContainsKey("svr"))
            {
                var visited = new Dictionary<string, long>();
                var start = devices["svr"];
                var end = "fft";
                var paths1 = GetPaths(start, devices, end, visited);
                Console.WriteLine($"To {end}: {paths1}.");

                start = devices[end];
                end = "dac";
                visited.Clear();
                var paths2 = GetPaths(start, devices, end, visited);
                Console.WriteLine($"To {end}: {paths2}.");

                start = devices["dac"];
                end = "out";
                visited.Clear();
                var paths3 = GetPaths(start, devices, end, visited);
                Console.WriteLine($"To {end}: {paths3}.");

                Console.WriteLine($"Total: {paths1 * paths2 * paths3}.");
            }

            Console.WriteLine($"Result in {sw}");

        }

        private long GetPaths(List<string> start, Dictionary<string, List<string>> devices, string target, Dictionary<string, long> visited)
        {
            var total = 0L;
            foreach (var device in start)
            {
                var paths = 0L;
                if (device == target)
                {
                    paths++;
                }
                else if (device == "out")
                {
                    return 0;
                }
                else
                {
                    if (visited.ContainsKey(device))
                    {
                        paths = visited[device];
                    }
                    else
                    {
                        paths = GetPaths(devices[device], devices, target, visited);
                        visited[device] = paths;
                    }
                }
                total += paths;
            }

            return total;
        }
    }
}