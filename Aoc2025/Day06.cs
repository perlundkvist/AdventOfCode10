using System.Diagnostics;

namespace AdventOfCode10.Aoc2025
{
    internal class Day06 : DayBase
    {
        internal void Run()
        {
            var sw = Stopwatch.StartNew();
            var input = GetInput("2025_06");
            var operations = input.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var results = new List<long>();

            foreach (var line in input[..^1])
            {
                var operators = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(o => long.Parse(o)).ToList();
                if(results.Count == 0)
                {
                    results.AddRange(operators);
                    continue;
                }
                for (var i = 0; i < operators.Count; i++)
                {
                    switch (operations[i])
                    {
                        case "+":
                            results[i] += operators[i];
                            break;
                        case "*":
                            results[i] *= operators[i];
                            break;
                    }
                }
            }

            Console.WriteLine($"First result in {sw}");
            Console.WriteLine($"Result: {results.Sum()}");

            sw.Restart();
            var total = 0L;
            var operators2 = new long[10];
            var idx = 0;
            for (var col = input[0].Length -1; col >= 0; col--)
            {
                for (var line = 0; line < input.Count; line++)
                {
                    var c = input[line][col];
                    if (c == ' ')
                        continue;
                    if (c == '+')
                    {
                        total += operators2.Sum();
                        operators2 = new long[10];
                        idx = -1;
                    }
                    else if (c == '*')
                    {
                        var result = operators2.Where(o => o > 0).Aggregate(1L, (acc, val) => acc * val);
                        total += result;
                        operators2 = new long[10];
                        idx = -1;
                    }
                    else
                    {
                        operators2[idx] = operators2[idx] * 10 + long.Parse(c.ToString());
                    }
                }
                idx++;

            }

            Console.WriteLine($"Second result in {sw}");
            Console.WriteLine($"Result: {total}");

        }

    }
}
