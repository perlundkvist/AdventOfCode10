using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day09 : DayBase
    {
        public enum Corner { UpLeft, UpRight, DownRight, DownLeft }

        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_09");
            var tiles = new List<Vec2>();
            var corners = new Dictionary<Vec2, Corner>();
            var insideCorner = new Dictionary<Vec2, bool>();

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

            var lines = new List<LineSegment>();
            for (var i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                var next = tiles[(i + 1) % tiles.Count];
                var before = tiles[((i - 1) + tiles.Count) % tiles.Count];
                var line2 = new LineSegment(tile, next);
                lines.Add(line2);
                corners.Add(tile, GetCorner(tile, before, next));
            }

            var line1 = lines[0];
            var lineEnd = lines.Last();
            Console.WriteLine($"First line: {line1}, Last line: {lineEnd}");

            var tile1 = neighbours.OrderByDescending(n => n.Key.Area(n.Value.First())).First().Key;
            var tile2 = neighbours[tile1].First();

            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Tile1: {tile1}");
            Console.WriteLine($"Tile2: {tile2}");
            Console.WriteLine($"Area: {tile1.Area(tile2)}");

            sw.Restart();

            Console.WriteLine($"X: {tiles.MinBy(t => t.X).X} - {tiles.MaxBy(t => t.X).X}");
            Console.WriteLine($"Y: {tiles.MinBy(t => t.Y).Y} - {tiles.MaxBy(t => t.Y).Y}");

            var startTile = tiles.OrderBy(t => t.X).ThenBy(t => t.Y).First();
            var corner = corners[startTile];

            insideCorner[startTile] = true;
            var idx = tiles.IndexOf(startTile);
            while (true)
            {
                var tile = tiles[idx];
                var next = tiles[(idx + 1) % tiles.Count];
                var corner1 = corners[tile];
                var corner2 = corners[next];
                if (corner1 == Corner.UpLeft || corner1 == Corner.DownRight)
                {
                    if (corner2 == Corner.DownLeft || corner2 == Corner.UpRight)
                        insideCorner[next] = insideCorner[tile];
                    else 
                        insideCorner[next] = !insideCorner[tile];
                }
                else if (corner1 == Corner.UpRight || corner1 == Corner.DownLeft)
                {
                    if (corner2 == Corner.DownRight || corner2 == Corner.UpLeft)
                        insideCorner[next] = insideCorner[tile];
                    else
                        insideCorner[next] = !insideCorner[tile];
                }
                if (next == startTile)
                    break;
                idx = tiles.IndexOf(next);
            }

            var incorrect = new List<long>() { 13294375, 3944701260L, 186554000L, 1341397176L, 1343576598L, 1522258700 };
            while (true)
            {
                tile1 = neighbours.OrderByDescending(n => n.Key.Area(n.Value.First())).First().Key;
                tile2 = neighbours[tile1].First();

                var debug = false;
                //if (tile1.ToString() == "(15007,82025)" && tile2.ToString() == "(84454,17061)")
                //    debug = true;
                if (tile1.Area(tile2) == 1343576598L)
                    debug = true;

                var cornerUpLeft = new Vec2(Math.Min(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));
                var cornerDownRight = new Vec2(Math.Max(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));

                if (!IsValid(tile1, tile2, tiles, corners, insideCorner, lines, debug))
                {
                    neighbours[tile1].RemoveAt(0);
                    continue;
                }
                var area = tile1.Area(tile2);
                Console.WriteLine($"Tile1: {tile1}");
                Console.WriteLine($"Tile2: {tile2}");
                Console.WriteLine($"Area: {area}");
                break;
            }


            Console.WriteLine($"Result in {sw}. 78646700 too low, 4370289630 too high. 186554000, 3944701260 not correct");

        }

        private Corner GetCorner(Vec2 tile, Vec2 before, Vec2 next)
        {
            var dir1 = GetDirection(tile, before);
            var dir2 = GetDirection(tile, next);
            if (dir1 == Direction.Up && dir2 == Direction.Left || dir1 == Direction.Left && dir2 == Direction.Up)
                return Corner.UpLeft;
            if (dir1 == Direction.Up && dir2 == Direction.Right || dir1 == Direction.Right && dir2 == Direction.Up)
                return Corner.UpRight;
            if (dir1 == Direction.Down && dir2 == Direction.Right || dir1 == Direction.Right && dir2 == Direction.Down)
                return Corner.DownRight;
            if (dir1 == Direction.Down && dir2 == Direction.Left || dir1 == Direction.Left && dir2 == Direction.Down)
                return Corner.DownLeft; 
            throw new Exception("Invalid corner");
        }

        private Direction GetDirection(Vec2 from, Vec2 to)
        {
            if (from.X == to.X)
            {
                return from.Y < to.Y ? Direction.Up : Direction.Down;
            }
            else if (from.Y == to.Y)
            {
                return from.X < to.X ? Direction.Right : Direction.Left;
            }
            throw new Exception("Not straight line");
        }

        private bool IsValid(Vec2 tile1, Vec2 tile2, List<Vec2> tiles, Dictionary<Vec2, Corner> corners, Dictionary<Vec2, bool> insideCorner, List<LineSegment> lines, bool debug)
        {
            var cornerUpLeft = new Vec2(Math.Min(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));
            var cornerUpRight = new Vec2(Math.Max(tile1.X, tile2.X), Math.Max(tile1.Y, tile2.Y));
            var cornerDownRight = new Vec2(Math.Max(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));
            var cornerDownLeft = new Vec2(Math.Min(tile1.X, tile2.X), Math.Min(tile1.Y, tile2.Y));

            var insideTiles = tiles.Where(t => t.X > cornerUpLeft.X && t.X < cornerUpRight.X && t.Y > cornerDownLeft.Y && t.Y < cornerUpLeft.Y).ToList();

            if (debug)
            {
                Console.WriteLine($"Checking rectangle: {tile1} - {tile2}");
                Console.WriteLine($"Checking rectangle: {cornerDownLeft} - {cornerUpRight}");
                Console.WriteLine($"Inside tiles: {string.Join(", ", insideTiles)}");
                Console.WriteLine($"Corners: {string.Join(", ", insideTiles.Select(c => corners[c]))}");
                Console.WriteLine($"Inside corners: {string.Join(", ", insideTiles.Select(c => insideCorner[c]))}");
            }

            if (insideTiles.Count > 0)
                return false;

            if (lines.Any(l => l.Start.X == l.End.X && l.Start.X > cornerUpLeft.X && l.Start.X < cornerUpRight.X && 
                               ((Math.Min(l.Start.Y, l.End.Y) < cornerDownLeft.Y && Math.Max(l.Start.Y, l.End.Y) >= cornerUpLeft.Y) ||(Math.Min(l.Start.Y, l.End.Y) <= cornerDownLeft.Y && Math.Max(l.Start.Y, l.End.Y) > cornerUpLeft.Y))))
            {
                if (debug)
                {
                    var crossinglLines = lines.Where(l => l.Start.X == l.End.X && l.Start.X > cornerUpLeft.X && l.Start.X < cornerUpRight.X &&
                                   ((Math.Min(l.Start.Y, l.End.Y) < cornerDownLeft.Y && Math.Max(l.Start.Y, l.End.Y) >= cornerUpLeft.Y) || (Math.Min(l.Start.Y, l.End.Y) <= cornerDownLeft.Y && Math.Max(l.Start.Y, l.End.Y) > cornerUpLeft.Y))).ToList();
                    Console.WriteLine($"Vertical line crossing. {string.Join(", ", crossinglLines)}");
                }
                return false;
            }

            if (lines.Any(l => l.Start.Y == l.End.Y && l.Start.Y > cornerDownLeft.Y && l.Start.Y < cornerUpLeft.Y &&
                ((l.Start.X < cornerDownLeft.X && l.End.X >= cornerDownRight.X) || (l.Start.X <= cornerDownLeft.X && l.End.X > cornerDownRight.X))))
            {
                if (debug)
                {
                    var crossinglLines = lines.Where(l => l.Start.Y == l.End.Y && l.Start.Y > cornerDownLeft.Y && l.Start.Y < cornerUpLeft.Y &&
                        ((l.Start.X < cornerDownLeft.X && l.End.X >= cornerDownRight.X) || (l.Start.X <= cornerDownLeft.X && l.End.X > cornerDownRight.X))).ToList();
                    Console.WriteLine($"Horisontal line crossing. {string.Join(", ", crossinglLines)}");
                }
                return false;
            }
            
            if (cornerUpLeft == tile1 || cornerUpLeft == tile2)
            {
                var corner = corners[cornerUpLeft];
                var inside = insideCorner[cornerUpLeft];
                if (corner == Corner.DownRight)
                    return insideCorner[cornerUpLeft];
                return !insideCorner[cornerUpLeft];
            }
            if (cornerUpRight == tile1 || cornerUpRight == tile2)
            {
                var corner = corners[cornerUpRight];
                var inside = insideCorner[cornerUpRight];
                if (corner == Corner.DownLeft)
                    return insideCorner[cornerUpRight];
                return !insideCorner[cornerUpRight];
            }
            throw new Exception("Illegal corner found");
        }
    }
}
