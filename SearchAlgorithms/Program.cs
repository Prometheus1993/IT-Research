using System.Diagnostics;

namespace SearchAlgorithms
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

                // Maak gesorteerde array en linked list aan
                int[] sortedArray          = GenerateSortedArray(size);
                LinkedList<int> linkedList = GenerateSortedLinkedList(size);

                // Zoek een getal dat bestaat (worst case: laatste element)
                int target = size;

                RunTest("Binary Search (Array)",       size, target, sortedArray, linkedList);
                RunTest("Linear Search (Linked List)", size, target, sortedArray, linkedList);
                RunTest("Binary Search (Linked List)", size, target, sortedArray, linkedList);
            }
        }

        static void RunTest(string name, int size, int target,
                            int[] sortedArray, LinkedList<int> linkedList)
        {
            int  numberOfRuns = 5;
            long totalTime    = 0;

            for (int run = 1; run <= numberOfRuns; run++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                if (name.Contains("Array"))
                    BinarySearchArray.Search(sortedArray, target);
                else if (name.Contains("Linear"))
                    LinearSearchLinkedList.Search(linkedList, target);
                else
                    BinarySearchLinkedList.Search(linkedList, target);

                stopwatch.Stop();
                totalTime += stopwatch.ElapsedTicks /
                             (Stopwatch.Frequency / 1_000_000);
            }

            long average = totalTime / numberOfRuns;
            Console.WriteLine($"  {name}: {average} microseconden");
        }

        // Genereert een gesorteerde array [1, 2, 3, ..., size]
        static int[] GenerateSortedArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = i + 1;
            return array;
        }

        // Genereert een gesorteerde linked list [1, 2, 3, ..., size]
        static LinkedList<int> GenerateSortedLinkedList(int size)
        {
            LinkedList<int> list = new LinkedList<int>();
            for (int i = 1; i <= size; i++)
                list.AddLast(i);
            return list;
        }
    }
}