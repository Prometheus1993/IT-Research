namespace Strassen
{
    public class MatrixHelper
    {
        // Twee matrices optellen
        // Elk vakje van A + het overeenkomstige vakje van B
        public static int[,] Add(int[,] matrixA, int[,] matrixB)
        {
            int size = matrixA.GetLength(0);
            int[,] result = new int[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    result[row, col] = matrixA[row, col] + matrixB[row, col];
                }
            }
            return result;
        }

        // Twee matrices aftrekken
        // Elk vakje van A - het overeenkomstige vakje van B
        public static int[,] Subtract(int[,] matrixA, int[,] matrixB)
        {
            int size = matrixA.GetLength(0);
            int[,] result = new int[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    result[row, col] = matrixA[row, col] - matrixB[row, col];
                }
            }
            return result;
        }

        // Splits een grote matrix in 4 gelijke kleinere matrices
        // Zoals een pizza in 4 gelijke stukken snijden
        //
        // | A  B |
        // | C  D |
        //
        public static void Split(int[,] matrix, 
                                  out int[,] topLeft,     // A
                                  out int[,] topRight,    // B
                                  out int[,] bottomLeft,  // C
                                  out int[,] bottomRight) // D
        {
            int size    = matrix.GetLength(0) / 2;
            topLeft     = new int[size, size];
            topRight    = new int[size, size];
            bottomLeft  = new int[size, size];
            bottomRight = new int[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    topLeft    [row, col] = matrix[row,        col       ];
                    topRight   [row, col] = matrix[row,        col + size];
                    bottomLeft [row, col] = matrix[row + size, col       ];
                    bottomRight[row, col] = matrix[row + size, col + size];
                }
            }
        }

        // Voeg 4 kleine matrices samen tot 1 grote matrix
        // Het omgekeerde van Split
        public static int[,] Combine(int[,] topLeft,    // A
                                      int[,] topRight,   // B
                                      int[,] bottomLeft, // C
                                      int[,] bottomRight)// D
        {
            int size   = topLeft.GetLength(0);
            int[,] result = new int[size * 2, size * 2];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    result[row,        col       ] = topLeft    [row, col];
                    result[row,        col + size] = topRight   [row, col];
                    result[row + size, col       ] = bottomLeft [row, col];
                    result[row + size, col + size] = bottomRight[row, col];
                }
            }
            return result;
        }
    }
}