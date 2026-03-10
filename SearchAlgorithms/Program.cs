using System.Diagnostics;

namespace SearchAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║   IT Research - Opdracht 2: Zoekalgoritmes      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");

            // ==========================================
            // DEEL 1: Zoekalgoritmes vergelijken
            //   - Binary Search op array      (O(log n))
            //   - Linear Search op linked list (O(n))
            //   - Binary Search op linked list (O(n log n))
            // ==========================================
            Console.WriteLine("\n========== DEEL 1: Zoekalgoritmes ==========\n");
            RunDeel1();

            // ==========================================
            // DEEL 2: Hash Table - Tijd vs Geheugen
            //   - Load factor meten
            //   - Collisions tellen
            //   - Geheugengebruik bijhouden
            // ==========================================
            Console.WriteLine("\n========== DEEL 2: Hash Table ==========\n");
            RunDeel2();
        }

        // -------------------------------------------------------
        // DEEL 1: Vergelijking van zoekalgoritmes
        // We testen op verschillende input groottes en meten de
        // tijd met een Stopwatch, net zoals in de paper.
        // We draaien 5 keer en nemen het gemiddelde.
        // -------------------------------------------------------
        static void RunDeel1()
        {
            int[] inputSizes = { 1000, 10000, 100000, 1000000 };
            int aantalRuns   = 5; // Gemiddelde van 5 runs voor nauwkeurigheid

            foreach (int size in inputSizes)
            {
                Console.WriteLine($"--- Input grootte: {size:N0} ---");

                // Maak een gesorteerde array en linked list met dezelfde data
                int[] sortedArray         = new int[size];
                LinkedList<int> linkedList = new LinkedList<int>();

                for (int i = 0; i < size; i++)
                {
                    sortedArray[i] = i;
                    linkedList.AddLast(i);
                }

                // We zoeken naar het LAATSTE element (worst case voor linear search)
                int target = size - 1;

                // Binary Search op array: meet gemiddelde tijd over 5 runs
                long totalBinaryArray = 0;
                for (int run = 0; run < aantalRuns; run++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    BinarySearchArray.Search(sortedArray, target);
                    sw.Stop();
                    totalBinaryArray += sw.ElapsedTicks;
                }

                // Linear Search op linked list: meet gemiddelde tijd over 5 runs
                long totalLinearLinked = 0;
                for (int run = 0; run < aantalRuns; run++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    LinearSearchLinkedList.Search(linkedList, target);
                    sw.Stop();
                    totalLinearLinked += sw.ElapsedTicks;
                }

                // Binary Search op linked list: meet gemiddelde tijd over 5 runs
                // Bij grote input (1.000.000) slaan we dit over want het duurt te lang
                long totalBinaryLinked      = 0;
                bool skippedBinaryLinked     = false;

                if (size <= 100000)
                {
                    for (int run = 0; run < aantalRuns; run++)
                    {
                        Stopwatch sw = Stopwatch.StartNew();
                        BinarySearchLinkedList.Search(linkedList, target);
                        sw.Stop();
                        totalBinaryLinked += sw.ElapsedTicks;
                    }
                }
                else
                {
                    skippedBinaryLinked = true;
                }

                // Bereken gemiddelden en toon resultaten
                double avgBinaryArray  = (double)totalBinaryArray / aantalRuns;
                double avgLinearLinked  = (double)totalLinearLinked / aantalRuns;
                double avgBinaryLinked  = (double)totalBinaryLinked / aantalRuns;

                // Converteer ticks naar microseconden voor leesbaarheid
                double ticksPerMicro = Stopwatch.Frequency / 1_000_000.0;

                Console.WriteLine($"  Binary Search (array):       {avgBinaryArray / ticksPerMicro,10:F2} µs");
                Console.WriteLine($"  Linear Search (linked list): {avgLinearLinked / ticksPerMicro,10:F2} µs");

                if (!skippedBinaryLinked)
                {
                    Console.WriteLine($"  Binary Search (linked list): {avgBinaryLinked / ticksPerMicro,10:F2} µs");
                }
                else
                {
                    Console.WriteLine($"  Binary Search (linked list): overgeslagen (te traag bij {size:N0})");
                }

                Console.WriteLine();
            }
        }

        // -------------------------------------------------------
        // DEEL 2: Hash Table - Load Factor en Collisions
        // We vullen hash tables van verschillende groottes en
        // meten hoeveel collisions er ontstaan naarmate de tabel
        // voller wordt. Dit toont de trade-off tussen geheugen
        // (tabelgrootte) en snelheid (aantal collisions).
        // -------------------------------------------------------
        static void RunDeel2()
        {
            int[] tableSizes = { 10, 50, 100 };

            foreach (int size in tableSizes)
            {
                Console.WriteLine($"--- Hash Table grootte: {size} ---");

                SimpleHashTable hashTable = new SimpleHashTable(size);

                // Vaste seed zodat resultaten reproduceerbaar zijn
                // Seed 42 geeft elke keer dezelfde "willekeurige" getallen
                Random random = new Random(42);

                int totalInserted = 0;
                for (int i = 1; i <= size; i++)
                {
                    // Willekeurige waarden uit een groter bereik
                    // Dit zorgt ervoor dat meerdere waarden dezelfde hash krijgen
                    // Bv: bij tabelgrootte 10 geven 3 en 13 allebei hash-index 3
                    int value = random.Next(0, size * 10);
                    bool success = hashTable.Insert(value);

                    if (!success)
                    {
                        Console.WriteLine($"  Tabel vol na {totalInserted} elementen!");
                        break;
                    }

                    totalInserted++;

                    // Print status na elke 25% vulling
                    if (i == size / 4 || i == size / 2 ||
                        i == size * 3 / 4 || i == size)
                    {
                        Console.WriteLine($"\n  Na {totalInserted} elementen:");
                        hashTable.PrintStatus();
                    }
                }

                Console.WriteLine();
            }
        }
    }
}