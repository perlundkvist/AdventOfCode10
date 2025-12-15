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
            Console.WriteLine($"Area: {tile1.Area(tile2)}");

            sw.Restart();

            Console.WriteLine($"X: {tiles.MinBy(t => t.X).X} - {tiles.MaxBy(t => t.X).X}");
            Console.WriteLine($"Y: {tiles.MinBy(t => t.Y).Y} - {tiles.MaxBy(t => t.Y).Y}");

            while (true)
            {
                tile1 = neighbours.OrderByDescending(n => n.Key.Area(n.Value.First())).First().Key;
                tile2 = neighbours[tile1].First();

                var debug = false;
                if (tile1.ToString() == "(15007,82025)" && tile2.ToString() == "(84454,17061)")
                    debug = true;

                var cornerUpLeft = new Vec2(Math.Min(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));
                var cornerDownRight = new Vec2(Math.Max(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));

                if (AnyInside(tile1, tile2, tiles, debug))
                {
                    neighbours[tile1].RemoveAt(0);
                    continue;
                }
                Console.WriteLine($"Tile1: {tile1}");
                Console.WriteLine($"Tile2: {tile2}");
                var area = tile1.Area(tile2);
                Console.WriteLine($"Area: {area} {area < 4370289630L}");
                break;
            }


            Console.WriteLine($"Result in {sw}. 4370289630 too high");

        }

        private bool AnyInside(Vec2 tile1, Vec2 tile2, List<Vec2> tiles, bool debug)
        {
            var cornerUpLeft = new Vec2(Math.Min(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));
            var cornerUpRight = new Vec2(Math.Max(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));
            var cornerDownRight = new Vec2(Math.Max(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));
            var cornerDownLeft = new Vec2(Math.Min(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));

            var insideTiles = tiles.Where(t => t.X > cornerUpLeft.X && t.X < cornerUpRight.X && t.Y > cornerDownLeft.Y && t.Y < cornerUpLeft.Y).ToList();
            insideTiles.AddRange(tiles.Where(t => t.X == cornerUpLeft.X && t.Y > cornerDownLeft.Y && t.Y < cornerUpLeft.Y));
            insideTiles.AddRange(tiles.Where(t => t.X == cornerUpRight.X && t.Y > cornerDownLeft.Y && t.Y < cornerUpLeft.Y));

            if (insideTiles.Count > 0)
                return true;

            if (debug)
            {
                Console.WriteLine($"Checking rectangle: {tile1} - {tile2}");
                Console.WriteLine($"Checking rectangle: {cornerDownLeft} - {cornerUpRight}");
            }

            for (var i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                var next = tiles[(i + 1) % tiles.Count];
                if (tile.X == next.X) // Vertical line
                {
                    var downY = Math.Min(tile.Y, next.Y);
                    var upY = Math.Max(tile.Y, next.Y);
                    if (tile.X >= cornerUpLeft.X && tile.X <= cornerUpRight.X)
                    {
                        if (downY == cornerDownLeft.Y && upY < cornerUpLeft.Y)
                            return true;
                        if (downY > cornerDownLeft.Y && upY <= cornerUpLeft.Y)
                            return true;
                        if (debug)
                        {
                            Console.WriteLine($"Checking vertical line: {cornerDownLeft.Y} < {cornerUpLeft.Y} : {downY} < {upY}");
                        }
                    }
                }
                else // Horizontal line
                {
                    var leftX = Math.Min(tile.X, next.X);
                    var rightX = Math.Max(tile.X, next.X);
                    if (tile.Y >= cornerDownLeft.Y && tile.Y <= cornerUpLeft.Y)
                    {
                        if (leftX == cornerUpLeft.X && rightX < cornerUpRight.X)
                            return true;
                        if (leftX > cornerUpLeft.X && rightX <= cornerUpRight.X)
                            return true;
                        if (debug)
                        {
                            Console.WriteLine($"Checking horizontal line: {cornerUpLeft.X} < {cornerUpRight.X} : {leftX} < {rightX}");
                        }
                    }
                }
            }

            return false;
        }
    }
}
