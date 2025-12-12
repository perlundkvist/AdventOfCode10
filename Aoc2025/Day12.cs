using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day12 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_12");

            var total = 0L;

            var boxes = new List<int>();

            var size = 0;
            var doingBoxes = true;

            foreach (var line in input)
            {
                if (doingBoxes && line.Contains("x"))
                    doingBoxes = false;

                if (doingBoxes)
                {
                    if (line.Contains(":"))
                        continue;
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        boxes.Add(size);
                        size = 0;
                    }
                    else
                    {
                        size += line.Count(c => c == '#');
                    }
                    continue;
                }

                var parts = line.Split(":", StringSplitOptions.TrimEntries);
                var area = int.Parse(parts[0].Split("x")[0])* int.Parse(parts[0].Split("x")[1]);
                var idx = 0;
                foreach (var need in parts[1].Split(" ").Select(p => int.Parse(p)))
                {
                    var boxArea = boxes[idx] * need;
                    area -= boxArea;
                    idx++;
                }
                if (area >= 0)
                    total++;
            }

            Console.WriteLine($"Total {total}");
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