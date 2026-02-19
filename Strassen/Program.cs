using System.Diagnostics;

namespace Strassen
{
    class Program
    {
        static void Main(string[] args)
        {
            // We testen met matrices van grootte 2, 4, 8, 16, 32
            // Onthoud: Strassen werkt alleen met machten van 2!
            int[] sizes = { 2, 4, 8, 16, 32, 64, 128, 256, 512 };

            foreach (int size in sizes)
            {
                Console.WriteLine($"\n=== Matrix grootte: {size}x{size} ===");

                // Maak twee willekeurige matrices aan
                int[,] matrixA = GenerateRandomMatrix(size);
                int[,] matrixB = GenerateRandomMatrix(size);

                int numberOfRuns     = 5;
                long normalTotal     = 0;
                long strassenTotal   = 0;

                for (int run = 1; run <= numberOfRuns; run++)
                {
                    // Meet tijd van normale vermenigvuldiging
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    NormalMultiply.Multiply(matrixA, matrixB);
                    stopwatch.Stop();
                    normalTotal += stopwatch.ElapsedTicks / 
                                  (Stopwatch.Frequency / 1_000_000);

                    // Meet tijd van Strassen
                    stopwatch = Stopwatch.StartNew();
                    StrassenMultiply.Multiply(matrixA, matrixB);
                    stopwatch.Stop();
                    strassenTotal += stopwatch.ElapsedTicks / 
                                    (Stopwatch.Frequency / 1_000_000);
                }

                // Bereken gemiddelde over 5 runs
                long normalAverage   = normalTotal   / numberOfRuns;
                long strassenAverage = strassenTotal / numberOfRuns;

                Console.WriteLine($"  Normale methode: {normalAverage} microseconden");
                Console.WriteLine($"  Strassen:        {strassenAverage} microseconden");
            }
        }

        // Genereert een willekeurige n x n matrix met waarden tussen 1 en 10
        static int[,] GenerateRandomMatrix(int size)
        {
            Random random    = new Random();
            int[,] matrix    = new int[size, size];

            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    matrix[row, col] = random.Next(1, 10);

            return matrix;
        }
    }
}