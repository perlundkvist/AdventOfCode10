using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day07 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_07");

            Position start = new Position(0, input[0].IndexOf("S"));
            var splitters = new HashSet<Position>();
            var beams = new HashSet<Position>
            {
                start
            };
            for (var line = 1; line < input.Count; line++)
            {
                foreach(var col in input[line].AllIndexesOf("^"))
                {
                    splitters.Add(new Position(line, col));
                }
            }

            var splits = 0;
            for (var line = 0; line < input.Count; line++)
            {
                foreach (var beam in beams.Where(s => s.Line == line).ToList())
                {
                    var down = new Position(line + 1, beam.Col);
                    if (splitters.Contains(down))
                    {
                        splits++;
                        beams.Add(new Position(line + 1, beam.Col - 1));
                        beams.Add(new Position(line + 1, beam.Col + 1));
                    }
                    else
                    {
                        beams.Add(down);
                    }
                }
            }

            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Splits: {splits}");

            sw.Restart();
            var timelineSet = new Dictionary<Position, long>();
            var timelines = GetTimelines(start, splitters.ToList(), timelineSet);

            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Timelines: {timelines}");

        }

        private long GetTimelines(Position start, List<Position> splitters, Dictionary<Position, long> timelineSet)
        {
            if (!splitters.Any(b => b.Line > start.Line))
                return 1;

            if (timelineSet.ContainsKey(start))
                return timelineSet[start];

            var timelineCount = 0L;
            var down = new Position(start.Line + 1, start.Col);
            if (!splitters.Contains(down))
            {
                timelineCount = GetTimelines(down, splitters, timelineSet);
            }
            else
            {
                down = new Position(start.Line + 1, start.Col - 1);
                timelineCount = GetTimelines(down, splitters, timelineSet); ;
                down = new Position(start.Line + 1, start.Col + 1);
                timelineCount += GetTimelines(down, splitters, timelineSet);
            }
            timelineSet[start] = timelineCount;
            return timelineCount;
        }
    }
}
