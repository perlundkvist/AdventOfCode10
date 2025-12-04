using System.Diagnostics;

namespace AdventOfCode10.Aoc2025
{
    internal class Day04 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_04");
            var map = new List<Position<char>>();
            for (var line = 0; line < input.Count; line++)
            {
                for (var col = 0; col < input[line].Length; col++)
                {
                    if (input[line][col] == '@')
                        map.Add(new Position<char>(line, col, input[line][col]));
                }
            }
            Console.WriteLine($"Parsed in {sw}");
            sw.Restart();
            var papers = 0;
            foreach (var pos in map)
            {
                var (up, down, left, right) = pos.GetSurrounding(map);
                var corners = pos.GetSurrounding45<char>(map);
                var adjacent = (up != null ? 1 : 0) + ((down != null ? 1 : 0)) + (left != null ? 1 : 0) + (right != null ? 1 : 0) +
                    (corners.upRight != null ? 1 : 0) + (corners.upLeft != null ? 1 : 0) + (corners.downRight != null ? 1 : 0) + (corners.downLeft != null ? 1 : 0);
                if (adjacent < 4)
                    papers++;
            }

            Console.WriteLine($"First result in {sw}");
            Console.WriteLine($"Rolls: {papers}");

            sw.Restart();
            var papers2 = 0;
            var found = true;
            while (found)
            {
                found = false;
                Console.WriteLine($"Remaining: {map.Count}");
                foreach (var pos in map.ToList())
                {
                    var (up, down, left, right) = pos.GetSurrounding(map);
                    var corners = pos.GetSurrounding45<char>(map);
                    var adjacent = (up != null ? 1 : 0) + ((down != null ? 1 : 0)) + (left != null ? 1 : 0) + (right != null ? 1 : 0) +
                        (corners.upRight != null ? 1 : 0) + (corners.upLeft != null ? 1 : 0) + (corners.downRight != null ? 1 : 0) + (corners.downLeft != null ? 1 : 0);
                    if (adjacent >= 4)
                        continue;
                    papers2++;
                    found = true;
                    map.Remove(pos);
                }
            }

            Console.WriteLine($"Second result in {sw}");
            Console.WriteLine($"Rolls: {papers2}");

        }

    }
}
