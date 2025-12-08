using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day08 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_08");

            var boxes = new List<Vec3>();
            var neighbours = new Dictionary<Vec3, List<Vec3>>();
            foreach (var line in input)
            {
                var parts = line.Split(','); 
                boxes.Add(new Vec3(parts[0], parts[1], parts[2]));
            }

            foreach (var box in boxes)
            {
                //var otherBoxes = 
                neighbours.Add(box, boxes.Where(b => b != box).OrderBy(b => b.DistanceTo(box)).ToList());
            }


            var circuits = new HashSet<List<Vec3>>();
            //var count = 0;
            var stop = input.Count < 100 ? 10 : 1000;
            for (var count = 0; count < stop; count++) {
                var box = neighbours.OrderBy(n => n.Key.DistanceTo(n.Value.First())).First();
                var box2 = box.Value.First();
                box.Value.Remove(box2);
                neighbours[box2].Remove(box.Key);
                var circuit1 = circuits.FirstOrDefault(c => c.Contains(box.Key));
                var circuit2 = circuits.FirstOrDefault(c => c.Contains(box2));
                if (circuit1 == null && circuit2 == null)
                {
                    circuits.Add([box.Key, box2]);
                }
                else if (circuit1 != null && circuit2 == null)
                {
                    circuit1.Add(box2);
                }
                else if (circuit1 == null && circuit2 != null)
                {
                    circuit2.Add(box.Key);
                }
                else if (circuit1 != circuit2)
                {
                    circuit1.AddRange(circuit2);
                    circuits.Remove(circuit2);
                }
            }

            var closest3 = circuits.OrderByDescending(c => c.Count).Take(3).ToList();
            var size = closest3.Aggregate(1L, (current, circuit) => current * circuit.Count);

            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Sizes: {size}");

            sw.Restart();

            while (true)
            {
                var box = neighbours.OrderBy(n => n.Key.DistanceTo(n.Value.First())).First();
                var box2 = box.Value.First();
                box.Value.Remove(box2);
                neighbours[box2].Remove(box.Key);
                var circuit1 = circuits.FirstOrDefault(c => c.Contains(box.Key));
                var circuit2 = circuits.FirstOrDefault(c => c.Contains(box2));
                if (circuit1 == null && circuit2 == null)
                {
                    circuits.Add([box.Key, box2]);
                }
                else if (circuit1 != null && circuit2 == null)
                {
                    circuit1.Add(box2);
                }
                else if (circuit1 == null && circuit2 != null)
                {
                    circuit2.Add(box.Key);
                }
                else if (circuit1 != circuit2)
                {
                    circuit1.AddRange(circuit2);
                    circuits.Remove(circuit2);
                }

                if (circuit1?.Count == boxes.Count || circuit2?.Count == boxes.Count)
                {
                    Console.WriteLine($"Result in {sw}");
                    Console.WriteLine($"{box.Key} - {box2}");
                    Console.WriteLine($"{box.Key.X * box2.X}");
                    break;
                }
            }

        }

    }
}
