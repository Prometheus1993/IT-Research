namespace SortComparison
{
    public class CountingSort
    {
        // De Sort methode neemt een array van integers en geeft een gesorteerde array terug
        public static int[] Sort(int[] inputArray)
        {
            // Stap 1: Zoek de grootste waarde in de array
            // We hebben dit nodig om te weten hoe groot onze tellijst moet zijn
            int maxValue = inputArray[0];
            foreach (int number in inputArray)
            {
                if (number > maxValue)
                {
                    maxValue = number;
                }
            }

            // Stap 2: Maak een tellijst aan met maxValue + 1 vakjes
            // Elk vakje staat voor een getal en begint op 0
            // Bijvoorbeeld: als maxValue = 4, maken we [0, 0, 0, 0, 0]
            //               voor de getallen        0  1  2  3  4
            int[] countArray = new int[maxValue + 1];

            // Stap 3: Loop door de inputArray en tel elk getal
            // Als we getal 3 tegenkomen, doen we countArray[3]++
            foreach (int number in inputArray)
            {
                countArray[number]++;
            }

            // Stap 4: Bouw de gesorteerde array op uit de tellijst
            // Als countArray[1] = 2, schrijven we twee keer het getal 1
            int[] sortedArray = new int[inputArray.Length];
            int currentPosition = 0;

            for (int i = 0; i < countArray.Length; i++)
            {
                // Schrijf het getal i zo vaak als het voorkomt
                for (int j = 0; j < countArray[i]; j++)
                {
                    sortedArray[currentPosition] = i;
                    currentPosition++;
                }
            }

            return sortedArray;
        }
    }
}