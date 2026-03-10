namespace SearchAlgorithms
{
    public class SimpleHashTable
    {
        // De vaste grootte van de hash table
        private int[] buckets;
        private bool[] isOccupied;  // bijhouden welke vakjes bezet zijn
        private int size;
        private int numberOfCollisions;
        private int numberOfElements;

        public SimpleHashTable(int size)
        {
            this.size         = size;
            buckets           = new int[size];
            isOccupied        = new bool[size];
            numberOfCollisions = 0;
            numberOfElements  = 0;
        }

        // Hash functie: geeft een vakje terug voor een waarde
        private int Hash(int value)
        {
            return value % size;
        }

        // Voegt een waarde toe aan de hash table
        // Gebruikt open addressing met linear probing:
        // als het gewenste vakje bezet is, probeer het volgende
        public bool Insert(int value)
        {
            int startIndex   = Hash(value);
            int currentIndex = startIndex;

            // Check of het originele gewenste vakje al bezet is
            // Als dat zo is, is dat EEN collision
            // We tellen niet elke probing-stap apart, want dat zou overcounten
            bool hadCollision = false;

            for (int i = 0; i < size; i++)
            {
                currentIndex = (startIndex + i) % size;

                if (!isOccupied[currentIndex])
                {
                    // Vakje is leeg, voeg toe
                    buckets[currentIndex]    = value;
                    isOccupied[currentIndex] = true;
                    numberOfElements++;

                    // Tel de collision pas als het element succesvol is ingevoegd
                    if (hadCollision)
                    {
                        numberOfCollisions++;
                    }

                    return true;
                }
                else if (i == 0)
                {
                    // Het ORIGINELE gewenste vakje is bezet = collision
                    // We markeren dit, maar tellen pas bij succesvolle insert
                    hadCollision = true;
                }
            }

            // Tabel is vol
            return false;
        }

        // Berekent de load factor: hoe vol zit de tabel?
        public double GetLoadFactor()
        {
            return (double)numberOfElements / size;
        }

        // Geeft het aantal collisions terug
        public int GetCollisions()
        {
            return numberOfCollisions;
        }

        // Berekent hoeveel geheugen de tabel gebruikt in bytes
        // Een int in C# is 4 bytes, een bool is 1 byte
        public int GetMemoryUsage()
        {
            int bucketsMemory  = size * 4;  // int array
            int occupiedMemory = size * 1;  // bool array
            return bucketsMemory + occupiedMemory;
        }

        // Toont de huidige staat van de tabel
        public void PrintStatus()
        {
            Console.WriteLine($"  Grootte:      {size} vakjes");
            Console.WriteLine($"  Elementen:    {numberOfElements}");
            Console.WriteLine($"  Load factor:  {GetLoadFactor():P0}");
            Console.WriteLine($"  Collisions:   {numberOfCollisions}");
            Console.WriteLine($"  Geheugen:     {GetMemoryUsage()} bytes");
        }
    }
}