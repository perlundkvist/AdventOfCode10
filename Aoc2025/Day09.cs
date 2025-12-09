using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day09 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_09");
            var tiles = new List<Vec2>();

            foreach (var line in input)
            {
                var parts = line.Split(','); 
                tiles.Add(new Vec2(parts[0], parts[1]));
            }

            var neighbours = new Dictionary<Vec2, List<Vec2>>();

            foreach (var tile in tiles)
            {
                neighbours.Add(tile, tiles.Where(b => b != tile).OrderByDescending(b => b.Area(tile)).ToList());
            }

            var tile1 = neighbours.OrderByDescending(n => n.Key.Area(n.Value.First())).First().Key;
            var tile2 = neighbours[tile1].First();

            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Tile1: {tile1}");
            Console.WriteLine($"Tile2: {tile2}");
            Console.WriteLine($"Area: {tile1.Area(tile2)}. 4351395516 is wrong");

            sw.Restart();


            Console.WriteLine($"Result in {sw}");

        }
    }
}
