using System.Diagnostics;

namespace SortComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            // Testgroottes zoals de paper: 150, 300 en 950
            int[] inputSizes = { 150, 300, 950 };

            foreach (int size in inputSizes)
            {
                Console.WriteLine($"\n=== Input grootte: {size} ===");

                // Test alle drie de cases
                RunTest("Best Case",    GenerateBestCase(size),    size);
                RunTest("Worst Case",   GenerateWorstCase(size),   size);
                RunTest("Average Case", GenerateAverageCase(size), size);
            }
        }

        // Voert 5 runs uit en berekent het gemiddelde, zoals de paper deed
        static void RunTest(string caseName, int[] data, int size)
        {
            Console.WriteLine($"\n  {caseName}:");

            long countingTotalTime    = 0;
            long tournamentTotalTime  = 0;
            int  numberOfRuns         = 5;

            for (int run = 1; run <= numberOfRuns; run++)
            {
                // Maak een kopie want het algoritme past de array aan
                int[] countingInput    = (int[])data.Clone();
                int[] tournamentInput  = (int[])data.Clone();

                // Meet tijd van Counting Sort
                Stopwatch stopwatch = Stopwatch.StartNew();
                CountingSort.Sort(countingInput);
                stopwatch.Stop();
                countingTotalTime += stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1_000_000);

                // Meet tijd van Tournament Sort
                stopwatch = Stopwatch.StartNew();
                TournamentSort.Sort(tournamentInput);
                stopwatch.Stop();
                tournamentTotalTime += stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1_000_000);
            }

            // Bereken gemiddelde over 5 runs
            long countingAverage   = countingTotalTime   / numberOfRuns;
            long tournamentAverage = tournamentTotalTime / numberOfRuns;

            Console.WriteLine($"    Counting Sort:   {countingAverage} microseconden");
            Console.WriteLine($"    Tournament Sort: {tournamentAverage} microseconden");
        }

        // Best case: al gesorteerd [1, 2, 3, ..., n]
        static int[] GenerateBestCase(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = i + 1;
            return array;
        }

        // Worst case: omgekeerd gesorteerd [n, n-1, ..., 1]
        static int[] GenerateWorstCase(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = size - i;
            return array;
        }

        // Average case: willekeurige volgorde
        static int[] GenerateAverageCase(int size)
        {
            int[]  array  = new int[size];
            Random random = new Random();
            for (int i = 0; i < size; i++)
                array[i] = random.Next(1, size + 1);
            return array;
        }
    }
}