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
                var foundPaths = new Dictionary<string, List<string>>();

                GetPaths2(start, devices, foundPaths);
                total = foundPaths.Count;

                Console.WriteLine($"Result in {sw}");
                Console.WriteLine($"Total: {total}.");
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

        private List<string> GetPaths2(List<string> start, Dictionary<string, List<string>> devices, Dictionary<string, List<string>> foundPaths)
        {
            var total = 0L;
            foreach (var device in start)
            {
                if (device == "out")
                    return ["out"];

                if (foundPaths.ContainsKey(device))
                {
                    return foundPaths[device];
                }

                //var paths = GetPaths2(devices[device], devices, foundDac, foundFft, foundPaths);
                //foundPaths[device] = (paths, foundDac, foundFft);
                //total += paths;
            }

            return [];
        }
    }
}