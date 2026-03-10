namespace SearchAlgorithms
{
    public class BinarySearchLinkedList
    {
        // Simuleert binary search op een linked list
        // Dit is inefficient omdat we geen directe toegang hebben tot posities!
        public static bool Search(LinkedList<int> linkedList, int target)
        {
            int low  = 0;
            int high = linkedList.Count - 1;

            while (low <= high)
            {
                // Overflow-veilige berekening van het midden
                int middleIndex = low + (high - low) / 2;

                // Dit is het probleem! We kunnen niet zeggen list[middleIndex]
                // We moeten vanaf het begin lopen om het midden te vinden
                // Dit kost middleIndex stappen elke keer opnieuw!
                int middleValue = GetElementAtIndex(linkedList, middleIndex);

                if (middleValue == target)
                {
                    return true;
                }
                else if (middleValue < target)
                {
                    low = middleIndex + 1;
                }
                else
                {
                    high = middleIndex - 1;
                }
            }

            return false;
        }

        // Deze methode toont het probleem: om positie n te vinden
        // moeten we n stappen zetten vanaf het begin
        // Bij een array zou dit gewoon array[index] zijn, één stap!
        private static int GetElementAtIndex(LinkedList<int> linkedList, int index)
        {
            int currentIndex = 0;

            // Loop door de linked list tot we op de juiste positie zijn
            foreach (int number in linkedList)
            {
                if (currentIndex == index)
                {
                    return number;
                }
                currentIndex++;
            }

            // Zou normaal nooit gebeuren als index geldig is
            return -1;
        }
    }
}