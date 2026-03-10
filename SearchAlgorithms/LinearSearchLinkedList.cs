namespace SearchAlgorithms
{
    public class LinearSearchLinkedList
    {
        // Zoekt een getal in een linked list door elk element te bekijken
        // Geeft true terug als het gevonden is, anders false
        public static bool Search(LinkedList<int> linkedList, int target)
        {
            // Loop door elk element in de linked list
            // We kunnen geen index gebruiken zoals bij een array!
            foreach (int number in linkedList)
            {
                if (number == target)
                {
                    // Gevonden!
                    return true;
                }
            }

            // Niet gevonden na heel de lijst te doorlopen
            return false;
        }
    }
}