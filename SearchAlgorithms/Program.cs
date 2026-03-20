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
            // ==========================================
            Console.WriteLine("\n========== DEEL 1: Zoekalgoritmes ==========\n");
            RunDeel1();

            // ==========================================
            // DEEL 2: Hash Table - Tijd vs Geheugen
            // ==========================================
            Console.WriteLine("\n========== DEEL 2: Hash Table ==========\n");
            RunDeel2();

            // ==========================================
            // DEEL 3: Caching - Performantie verbeteren
            //   - Zonder cache
            //   - Met LRU in-memory cache
            //   - Met Redis cache
            // ==========================================
            Console.WriteLine("\n========== DEEL 3: Caching ==========\n");
            RunDeel3();
        }

        // -------------------------------------------------------
        // DEEL 1: Vergelijking van zoekalgoritmes
        // -------------------------------------------------------
        static void RunDeel1()
        {
            int[] inputSizes = { 1000, 10000, 100000, 1000000 };
            int aantalRuns   = 5;

            foreach (int size in inputSizes)
            {
                Console.WriteLine($"--- Input grootte: {size:N0} ---");

                int[] sortedArray         = new int[size];
                LinkedList<int> linkedList = new LinkedList<int>();

                for (int i = 0; i < size; i++)
                {
                    sortedArray[i] = i;
                    linkedList.AddLast(i);
                }

                int target = size - 1;

                // Binary Search op array
                long totalBinaryArray = 0;
                for (int run = 0; run < aantalRuns; run++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    BinarySearchArray.Search(sortedArray, target);
                    sw.Stop();
                    totalBinaryArray += sw.ElapsedTicks;
                }

                // Linear Search op linked list
                long totalLinearLinked = 0;
                for (int run = 0; run < aantalRuns; run++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    LinearSearchLinkedList.Search(linkedList, target);
                    sw.Stop();
                    totalLinearLinked += sw.ElapsedTicks;
                }

                // Binary Search op linked list
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

                double avgBinaryArray  = (double)totalBinaryArray / aantalRuns;
                double avgLinearLinked  = (double)totalLinearLinked / aantalRuns;
                double avgBinaryLinked  = (double)totalBinaryLinked / aantalRuns;
                double ticksPerMicro   = Stopwatch.Frequency / 1_000_000.0;

                Console.WriteLine($"  Binary Search (array):       {avgBinaryArray / ticksPerMicro,10:F2} µs");
                Console.WriteLine($"  Linear Search (linked list): {avgLinearLinked / ticksPerMicro,10:F2} µs");

                if (!skippedBinaryLinked)
                    Console.WriteLine($"  Binary Search (linked list): {avgBinaryLinked / ticksPerMicro,10:F2} µs");
                else
                    Console.WriteLine($"  Binary Search (linked list): overgeslagen (te traag bij {size:N0})");

                Console.WriteLine();
            }
        }

        // -------------------------------------------------------
        // DEEL 2: Hash Table - Load Factor en Collisions
        // -------------------------------------------------------
        static void RunDeel2()
        {
            int[] tableSizes = { 10, 50, 100 };

            foreach (int size in tableSizes)
            {
                Console.WriteLine($"--- Hash Table grootte: {size} ---");

                SimpleHashTable hashTable = new SimpleHashTable(size);
                Random random = new Random(42);

                int totalInserted = 0;
                for (int i = 1; i <= size; i++)
                {
                    int value = random.Next(0, size * 10);
                    bool success = hashTable.Insert(value);

                    if (!success)
                    {
                        Console.WriteLine($"  Tabel vol na {totalInserted} elementen!");
                        break;
                    }

                    totalInserted++;

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

        // -------------------------------------------------------
        // DEEL 3: Caching - Vergelijk geen cache, LRU, en Redis
        //
        // We simuleren een situatie waarbij dezelfde zoekacties
        // herhaald worden, zoals een gebruiker die steeds dezelfde
        // dingen opzoekt. Precies de situatie waar caching helpt.
        // -------------------------------------------------------
        static void RunDeel3()
        {
            int arraySize = 1_000_000;

            // Maak een gesorteerde array en linked list
            int[] sortedArray         = new int[arraySize];
            LinkedList<int> linkedList = new LinkedList<int>();

            for (int i = 0; i < arraySize; i++)
            {
                sortedArray[i] = i;
                linkedList.AddLast(i);
            }

            // Simuleer herhaalde zoekopdrachten
            // In het echt zoeken gebruikers vaak dezelfde dingen op
            // We maken een mix: een paar populaire targets die vaak terugkomen
            // en af en toe een nieuw target (zoals in de echte wereld)
            Random random = new Random(42);
            int[] popularTargets = { 999_999, 500_000, 250_000, 750_000, 100_000 };
            int totalSearches = 500;

            // Maak de lijst van zoekopdrachten: 80% populaire targets, 20% willekeurig
            int[] searchTargets = new int[totalSearches];
            for (int i = 0; i < totalSearches; i++)
            {
                if (random.Next(100) < 80)
                {
                    // 80% kans: kies een populair target
                    searchTargets[i] = popularTargets[random.Next(popularTargets.Length)];
                }
                else
                {
                    // 20% kans: willekeurig target
                    searchTargets[i] = random.Next(0, arraySize);
                }
            }

            double ticksPerMicro = Stopwatch.Frequency / 1_000_000.0;

            // ---- TEST 1: Zonder cache (Linear Search) ----
            Console.WriteLine("--- Test 1: Linear Search ZONDER cache ---");
            {
                Stopwatch sw = Stopwatch.StartNew();
                foreach (int target in searchTargets)
                {
                    LinearSearchLinkedList.Search(linkedList, target);
                }
                sw.Stop();

                double totalMs = sw.ElapsedTicks / ticksPerMicro / 1000.0;
                Console.WriteLine($"  {totalSearches} zoekopdrachten in {totalMs:F2} ms");
                Console.WriteLine($"  Gemiddeld per zoekopdracht: {sw.ElapsedTicks / ticksPerMicro / totalSearches:F2} µs");
                Console.WriteLine();
            }

            // ---- TEST 2: Met LRU in-memory cache ----
            Console.WriteLine("--- Test 2: Linear Search MET LRU cache (max 10 items) ---");
            {
                LruCache<int, bool> lruCache = new LruCache<int, bool>(10);

                Stopwatch sw = Stopwatch.StartNew();
                foreach (int target in searchTargets)
                {
                    // Eerst kijken of het resultaat al in de cache zit
                    if (!lruCache.TryGet(target, out bool found))
                    {
                        // Cache miss! Moet echt gaan zoeken
                        found = LinearSearchLinkedList.Search(linkedList, target);

                        // Resultaat opslaan in de cache voor de volgende keer
                        lruCache.Put(target, found);
                    }
                }
                sw.Stop();

                double totalMs = sw.ElapsedTicks / ticksPerMicro / 1000.0;
                Console.WriteLine($"  {totalSearches} zoekopdrachten in {totalMs:F2} ms");
                Console.WriteLine($"  Gemiddeld per zoekopdracht: {sw.ElapsedTicks / ticksPerMicro / totalSearches:F2} µs");
                Console.WriteLine($"  Cache hits: {lruCache.Hits}, misses: {lruCache.Misses}");
                Console.WriteLine($"  Hit ratio: {(double)lruCache.Hits / totalSearches:P0}");
                Console.WriteLine();
            }

            // ---- TEST 3: Met Redis cache ----
            Console.WriteLine("--- Test 3: Linear Search MET Redis cache ---");
            {
                RedisCacheHelper redisCache = new RedisCacheHelper();
                redisCache.Clear(); // Begin met een schone cache

                Stopwatch sw = Stopwatch.StartNew();
                foreach (int target in searchTargets)
                {
                    string key = $"search:{target}";

                    // Eerst kijken of het resultaat al in Redis zit
                    if (!redisCache.TryGet(key, out int cachedResult))
                    {
                        // Cache miss! Moet echt gaan zoeken
                        bool found = LinearSearchLinkedList.Search(linkedList, target);

                        // Resultaat opslaan in Redis (30 seconden geldig)
                        redisCache.Put(key, found ? 1 : 0, TimeSpan.FromSeconds(30));
                    }
                }
                sw.Stop();

                double totalMs = sw.ElapsedTicks / ticksPerMicro / 1000.0;
                Console.WriteLine($"  {totalSearches} zoekopdrachten in {totalMs:F2} ms");
                Console.WriteLine($"  Gemiddeld per zoekopdracht: {sw.ElapsedTicks / ticksPerMicro / totalSearches:F2} µs");
                Console.WriteLine($"  Cache hits: {redisCache.Hits}, misses: {redisCache.Misses}");
                Console.WriteLine($"  Hit ratio: {(double)redisCache.Hits / totalSearches:P0}");

                redisCache.Clear();
                redisCache.Dispose();
                Console.WriteLine();
            }

            // ---- TEST 4: Binary Search op array (als referentie) ----
            Console.WriteLine("--- Test 4: Binary Search op array (ter vergelijking) ---");
            {
                Stopwatch sw = Stopwatch.StartNew();
                foreach (int target in searchTargets)
                {
                    BinarySearchArray.Search(sortedArray, target);
                }
                sw.Stop();

                double totalMs = sw.ElapsedTicks / ticksPerMicro / 1000.0;
                Console.WriteLine($"  {totalSearches} zoekopdrachten in {totalMs:F2} ms");
                Console.WriteLine($"  Gemiddeld per zoekopdracht: {sw.ElapsedTicks / ticksPerMicro / totalSearches:F2} µs");
                Console.WriteLine($"  (geen cache nodig, Binary Search is al supersnel)");
                Console.WriteLine();
            }
            
            // ---- TEST: Worst case - alles uniek, caching is zinloos ----
            Console.WriteLine("--- Test FOUT: LRU cache met 100% unieke zoekopdrachten ---");
            {
                LruCache<int, bool> lruCache = new LruCache<int, bool>(10);

                // Elke zoekopdracht is uniek, niets herhaalt zich
                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < totalSearches; i++)
                {
                    int target = i * (arraySize / totalSearches); // verspreid over hele lijst

                    if (!lruCache.TryGet(target, out bool found))
                    {
                        found = LinearSearchLinkedList.Search(linkedList, target);
                        lruCache.Put(target, found);
                    }
                }
                sw.Stop();

                double totalMs = sw.ElapsedTicks / ticksPerMicro / 1000.0;
                Console.WriteLine($"  {totalSearches} zoekopdrachten in {totalMs:F2} ms");
                Console.WriteLine($"  Gemiddeld per zoekopdracht: {sw.ElapsedTicks / ticksPerMicro / totalSearches:F2} µs");
                Console.WriteLine($"  Cache hits: {lruCache.Hits}, misses: {lruCache.Misses}");
                Console.WriteLine($"  Hit ratio: {(double)lruCache.Hits / totalSearches:P0}");
                Console.WriteLine();
            }
            
            // ---- TEST: Cache thrashing - cache te klein voor het patroon ----
            Console.WriteLine("--- Test FOUT: LRU cache te klein (1 vakje, 2 targets) ---");
            {
                LruCache<int, bool> lruCache = new LruCache<int, bool>(1);
                int[] twoTargets = { 750_000, 500_000 };

                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < totalSearches; i++)
                {
                    int target = twoTargets[i % 2]; // wissel steeds

                    if (!lruCache.TryGet(target, out bool found))
                    {
                        found = LinearSearchLinkedList.Search(linkedList, target);
                        lruCache.Put(target, found);
                    }
                }
                sw.Stop();

                double totalMs = sw.ElapsedTicks / ticksPerMicro / 1000.0;
                Console.WriteLine($"  {totalSearches} zoekopdrachten in {totalMs:F2} ms");
                Console.WriteLine($"  Gemiddeld per zoekopdracht: {sw.ElapsedTicks / ticksPerMicro / totalSearches:F2} µs");
                Console.WriteLine($"  Cache hits: {lruCache.Hits}, misses: {lruCache.Misses}");
                Console.WriteLine($"  Hit ratio: {(double)lruCache.Hits / totalSearches:P0}");
                Console.WriteLine();
            }
        }
    }
}