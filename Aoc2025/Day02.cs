namespace AdventOfCode10.Aoc2025
{
    internal class Day02 : DayBase
    {
        internal void Run()
        {
            var input = GetInput("2025_02");
            var sequences = input[0].Split(',').ToList();
            var invalid = 0L;
            var invalid2 = 0L;
            foreach (var sequence in sequences)
            {
                invalid += GetInvalid(sequence);
                invalid2 += GetInvalid2(sequence);
            }


            Console.WriteLine($"Invalid: {invalid}");
            Console.WriteLine($"Invalid 2: {invalid2}");

        }

        private long GetInvalid(string sequence)
        {
            var start  = long.Parse(sequence.Split("-")[0]);
            var end  = long.Parse(sequence.Split("-")[1]);
            var invalid = 0L;
            for (var i = start; i <= end; i++)
            {
                var id = i.ToString();
                if (id.Length % 2 == 1)
                    continue;
                var half = id[(id.Length / 2)..];
                var half2 = id[..(id.Length / 2)];
                if (half == half2)
                    invalid += i;
            }
            return invalid;
        }

        private long GetInvalid2(string sequence)
        {
            var start = long.Parse(sequence.Split("-")[0]);
            var end = long.Parse(sequence.Split("-")[1]);
            var invalid = 0L;
            for (var i = start; i <= end; i++)
            {
                var id = i.ToString();
                if (IsInvalid(i))
                    invalid += i;
            }
            return invalid;
        }

        private bool IsInvalid(long i)
        {
            var id = i.ToString();
            for (var j = 1; j <= id.Length/2; j++)
            {
                var part = id[..j];
                var id2 = part;
                while (id2.Length < id.Length)
                {
                    id2 += part;
                }
                if (id2 == id)
                    return true;
            }
            return false;
        }
    }
}
