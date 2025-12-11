using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day09 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_09s");
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
            Console.WriteLine($"Area: {tile1.Area(tile2)}");

            sw.Restart();

            while (true)
            {
                tile1 = neighbours.OrderByDescending(n => n.Key.Area(n.Value.First())).First().Key;
                tile2 = neighbours[tile1].First();

                var idx = tiles.IndexOf(tile1) - 1;
                if (idx < 0)
                    idx = tiles.Count - 1;
                var before1 = tiles[idx];
                idx = (tiles.IndexOf(tile1) + 1) % tiles.Count;
                var after1 = tiles[idx];

                idx = tiles.IndexOf(tile2) - 1;
                if (idx < 0)
                    idx = tiles.Count - 1;
                var before2 = tiles[idx];
                idx = (tiles.IndexOf(tile2) + 1) % tiles.Count;
                var after2 = tiles[idx];

                var corner1 = new Vec2(Math.Min(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));
                var corner2 = new Vec2(Math.Max(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));
                var corner3 = new Vec2(Math.Max(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));
                var corner4 = new Vec2(Math.Min(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));

            }


            Console.WriteLine($"Result in {sw}");

        }
    }
}
