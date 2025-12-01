
namespace AdventOfCode10.Aoc2025
{
    internal class Day01 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2025_01");

            var dial = 50;
            var zeroes = 0;
            var zeroes2 = 0;

            foreach (var line in input)
            {
                zeroes2 += GetZeroes(dial, line);
                switch (line[0])
                {
                    case 'L':
                       dial -= int.Parse(line[1..]);
                        break;
                    case 'R':
                        dial += int.Parse(line[1..]);
                        break;
                }
                dial = (dial + 100) % 100;
                Console.WriteLine($"The dial is rotated {line} to point at {dial}. Zeroes2 {zeroes2}");
                if (dial == 0)
                    zeroes++;
            }


            Console.WriteLine($"Code: {zeroes}");
            Console.WriteLine($"Code2: {zeroes2}");

        }

        private int GetZeroes(int dial, string line)
        {
            var stop = int.Parse(line[1..]);
            var zeroes = 0;
            for (int i = 1; i <= stop; i++)
            {
                switch (line[0])
                {
                    case 'L':
                        dial--;
                        break;
                    case 'R':
                        dial++;
                        break;
                }
                dial = (dial + 100) % 100;
                if (dial == 0)
                    zeroes++;
            }
            return zeroes;
        }
    }
}
