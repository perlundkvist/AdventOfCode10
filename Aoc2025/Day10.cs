using Google.OrTools.LinearSolver;
using System.Diagnostics;

namespace AdventOfCode10.Aoc2025
{
    internal class Day10 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_10");

            var total = 0L;

            var doPart1 = false;

            foreach (var line in input)
            {
                var parts = line.Split(' ');
                var wanted = new List<int>();
                var buttons = new List<List<int>>();
                var goals = new List<int>();
                foreach (var part in parts)
                {
                    if (part.StartsWith("["))
                    {
                        wanted.AddRange(part[1..^1].AllIndexesOf("#"));
                    }
                    else if (part.StartsWith("("))
                    {
                        var bParts = part[1..^1].Split(',');
                        buttons.Add(bParts.Select(b => int.Parse(b)).ToList());
                    }
                    else if (part.StartsWith("{"))
                    {
                        var gParts = part[1..^1].Split(',');
                        goals.AddRange(gParts.Select(g => int.Parse(g)).ToList());
                    }
                }
                var least = long.MaxValue;
                if (doPart1)
                {
                    foreach (var button in buttons)
                    {
                        var testing = $"({string.Join(",", button)})";
                        least = GetLeastPresses(wanted, [], button, buttons, least, 0);
                    }
                    Console.WriteLine($"Least: {least}. Line {input.IndexOf(line) + 1} of {input.Count}");
                }
                else
                {
                    least = DoSolver(buttons, goals);
                }
                total += least;
            }


            Console.WriteLine($"Result in {sw}");
            Console.WriteLine($"Total: {total}.");
        }

        private long GetLeastPresses(List<int> wanted, List<int> lights, List<int> button, List<List<int>> buttons,
            long least, long presses)
        {
            if (presses >= least || presses > 10)
                return least;

            var newLights = new List<int>();

            var stop = Math.Max(lights.Any() ? lights.Max() : 0, button.Max());
            for (var i = 0; i <= stop; i++)
            {
                var light = button.Contains(i) ^ lights.Contains(i);
                if (light)
                    newLights.Add(i);
            }

            if (newLights.SequenceEqual(wanted))
            {
                return presses + 1;
            }

            foreach (var button2 in buttons.Where(b => b != button).ToList())
            {
                var testing = $"({string.Join(",", button2)})";
                least = GetLeastPresses(wanted, newLights.ToList(), button2, buttons, least, presses + 1);
            }
            return least;
        }

        // https://developers.google.com/optimization/mip/mip_example
        private long DoSolver(List<List<int>> buttons, List<int> goals)
        {
            var solver = Solver.CreateSolver("SCIP");
            var variables = new List<Variable>();
            var sumExpr = new LinearExpr();
            for (var i = 0; i < buttons.Count; i++)
            {
                var v = solver.MakeIntVar(0.0, double.PositiveInfinity, $"b{i}");
                variables.Add(v);
                sumExpr += v;
            }

            // Constraints
            for (var g = 0; g < goals.Count; g++)
            {
                var expr = new LinearExpr();
                for (var b = 0; b < buttons.Count; b++)
                {
                    var button = buttons[b];
                    if (button.Contains(g))
                    {
                        expr += variables[b];
                    }
                }
                var goal = goals[g];
                solver.Add(expr == goal);
            }

            solver.Minimize(sumExpr);

            var status = solver.Solve();

            if (status != Solver.ResultStatus.OPTIMAL)
            {
                Console.WriteLine("The problem does not have an optimal solution!");
                return 0;
            }

            var sum = variables.Sum(v => (long) v.SolutionValue());
            //Console.WriteLine($"Sum: {sum}");

            return sum;

        }

    }
}
