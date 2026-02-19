namespace SortComparison
{
    public class TournamentSort
    {
        // De Sort methode neemt een array en geeft een gesorteerde array terug
        public static int[] Sort(int[] inputArray)
        {
            // Maak een kopie zodat we de originele array niet aanpassen
            List<int> remainingNumbers = new List<int>(inputArray);
            int[] sortedArray = new int[inputArray.Length];
            int currentPosition = 0;

            // Blijf toernooien spelen zolang er nog getallen over zijn
            while (remainingNumbers.Count > 0)
            {
                // Speel een toernooi en zoek de winnaar (kleinste getal)
                int winner = PlayTournament(remainingNumbers);

                // Zet de winnaar op de juiste positie in het resultaat
                sortedArray[currentPosition] = winner;
                currentPosition++;

                // Verwijder de winnaar uit de lijst voor de volgende ronde
                remainingNumbers.Remove(winner);
            }

            return sortedArray;
        }

        // PlayTournament vergelijkt getallen zoals in een echt toernooi
        // De kleinste waarde wint elke match
        private static int PlayTournament(List<int> numbers)
        {
            // Begin met de eerste speler als voorlopige winnaar
            int currentWinner = numbers[0];

            // Vergelijk de winnaar met elke andere speler
            foreach (int number in numbers)
            {
                // Als een speler kleiner is, wordt die de nieuwe winnaar
                if (number < currentWinner)
                {
                    currentWinner = number;
                }
            }

            return currentWinner;
        }
    }
}