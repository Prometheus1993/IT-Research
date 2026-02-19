namespace Strassen
{
    public class NormalMultiply
    {
        // Normale matrix vermenigvuldiging O(n³)
        // Voor elk vakje in het resultaat:
        // neem een rij van A en een kolom van B,
        // vermenigvuldig paarsgewijs en tel op
        public static int[,] Multiply(int[,] matrixA, int[,] matrixB)
        {
            int size = matrixA.GetLength(0);
            int[,] result = new int[size, size];

            // Buitenste loop: elke rij van A
            for (int row = 0; row < size; row++)
            {
                // Middelste loop: elke kolom van B
                for (int col = 0; col < size; col++)
                {
                    // Binnenste loop: vermenigvuldig en tel op
                    // Dit is wat jij met de hand deed eerder!
                    // (1x5) + (2x7) = 19
                    for (int k = 0; k < size; k++)
                    {
                        result[row, col] += matrixA[row, k] * matrixB[k, col];
                    }
                }
            }
            return result;
        }
    }
}