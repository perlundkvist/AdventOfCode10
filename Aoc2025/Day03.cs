



namespace AdventOfCode10.Aoc2025
{
    internal class Day03 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2025_03");
            var jolts = 0L;
            var jolts2 = 0L;
            foreach (var bank in input)
            {
                jolts += GetJolts(bank);
                jolts2 += GetJolts2(bank, 12);
            }


            Console.WriteLine($"Jolts: {jolts}");
            Console.WriteLine($"Jolts2: {jolts2}");

        }

        private long GetJolts(string bank)
        {
            var jolts = 0;
            for (char c = '9'; c >= '0'; c--)
            {
                var index = bank.IndexOf(c);
                if (index == -1) continue;
                if (index == bank.Length - 1) continue;
                var c2 = bank[(index+1)..].Max(c => c);
                jolts = (c - '0') * 10 + (c2 - '0');
                break;
            }
            return jolts;
        }

        private long GetJolts2(string bank, int length)
        {
            var selection = bank[..^(length-1)];
            var max = selection.Max(c => c);
            if (length == 1)
                return (max - '0');
            bank = bank[(bank.IndexOf(max)+1)..];
            return (max - '0') * (long)Math.Pow(10, (length-1)) + GetJolts2(bank, length - 1);
        }
    }
}
