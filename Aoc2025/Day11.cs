using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day11 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_11ss");

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

                total = GetPaths(start, devices);

                Console.WriteLine($"Result in {sw}");
                Console.WriteLine($"Total: {total}.");
            }

            sw.Restart();
            total = 0L;
            if (devices.ContainsKey("svr"))
            {
                var start = devices["svr"];
                var visited = new Dictionary<string, long>();

                var paths = GetPaths2(start, devices, visited);

                Console.WriteLine($"Result in {sw}");
                Console.WriteLine($"Total: {paths.Count}.");
            }

            Console.WriteLine($"Result in {sw}");

        }

        private long GetPaths(List<string> start, Dictionary<string, List<string>> devices)
        {
            var paths = 0L;
            foreach (var device in start)
            {
                if (device == "out")
                {
                    paths++;
                }
                else
                {
                    paths += GetPaths(devices[device], devices);
                }
            }

            return paths;
        }

        private List<string> GetPaths2(List<string> start, Dictionary<string, List<string>> devices, Dictionary<string, long> visited)
        {
            if (start.Contains("out"))
            {
                return start;
            }
            //foreach (var device in start)
            //{
            //    if (visited.ContainsKey(device))
            //    {
            //        total += visited[device];
            //        continue;
            //    }
            //    var correct = GetPaths2(devices[device], devices, visited);
            //    visited[device] = correct;
            //    total += correct;
            //}

            return [];
        }
    }
}