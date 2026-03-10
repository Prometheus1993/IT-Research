namespace SearchAlgorithms
{
    public class BinarySearchArray
    {
        // Zoekt een getal in een gesorteerde array
        // Geeft de index terug als het gevonden is, anders -1
        public static int Search(int[] sortedArray, int target)
        {
            int low    = 0;
            int high   = sortedArray.Length - 1;

            // Blijf halveren zolang er nog een zoekgebied is
            while (low <= high)
            {
                // Bereken het midden van het huidige zoekgebied
                // We gebruiken low + (high - low) / 2 in plaats van (low + high) / 2
                // om integer overflow te voorkomen bij grote arrays
                int middle = low + (high - low) / 2;

                if (sortedArray[middle] == target)
                {
                    // Gevonden! Geef de positie terug
                    return middle;
                }
                else if (sortedArray[middle] < target)
                {
                    // Target ligt in de rechterhelft
                    // Gooi de linkerhelft weg
                    low = middle + 1;
                }
                else
                {
                    // Target ligt in de linkerhelft
                    // Gooi de rechterhelft weg
                    high = middle - 1;
                }
            }

            // Niet gevonden
            return -1;
        }
    }
}