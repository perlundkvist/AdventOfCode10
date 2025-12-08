using System.Diagnostics;
using static AdventOfCode10.DayBase;

namespace AdventOfCode10.Aoc2025
{
    internal class Day08 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_08s");

            var boxes = new List<Vec3>();
            var neighbours = new Dictionary<Vec3, (Vec3, double)>();
            foreach (var line in input)
            {
                var parts = line.Split(','); 
                boxes.Add(new Vec3(parts[0], parts[1], parts[2]));
            }

            for (var i = 0; i < boxes.Count; i++)
            {
                double distance = 0;
                Vec3 closest = boxes[i];
                for(var j = 0; j < boxes.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (neighbours.Any(n => n.Value.Item1 == boxes[j]))
                        continue;
                    var dist = boxes[i].DistanceTo(boxes[j]);
                    if (distance == 0 || dist < distance)
                    {
                        distance = dist;
                        closest = boxes[j];
                    }
                    neighbours[boxes[i]] = (closest, distance);
                }
            }

            var circuits = new HashSet<List<Vec3>>();
            var count = 0;
            var stop = input.Count < 100 ? 10 : 1000;
            foreach (var nearest in neighbours.OrderBy(c => c.Value.Item2))
            {
                var circuit1 = circuits.FirstOrDefault(c => c.Contains(nearest.Key));
                var circuit2 = circuits.FirstOrDefault(c => c.Contains(nearest.Value.Item1));
                if (circuit1 == null && circuit2 == null)
                {
                    circuits.Add(new List<Vec3> { nearest.Key, nearest.Value.Item1 });
                }
                else if (circuit1 != null && circuit2 == null)
                {
                    circuit1.Add(nearest.Value.Item1);
                }
                else if (circuit1 == null && circuit2 != null)
                {
                    circuit2.Add(nearest.Key);
                }
                else if (circuit1 != circuit2)
                {
                    circuit1.AddRange(circuit2);
                    circuits.Remove(circuit2);
                }
                count++;
                //if (count == stop)
                //    break;
            }

            var closest3 = circuits.OrderByDescending(c => c.Count).Take(3).ToList();

            var size = 1L;
            foreach (var box in closest3)
            {
                size *= box.Count;
            }

            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Sizes: {size}");

            sw.Restart();


            //Console.WriteLine($"Result in {sw}");

        }

    }
}
