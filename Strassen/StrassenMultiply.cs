namespace Strassen
{
    public class StrassenMultiply
    {
        public static int[,] Multiply(int[,] matrixA, int[,] matrixB)
        {
            int size = matrixA.GetLength(0);

            // Stopconditie: als de matrix 1x1 is, gewoon vermenigvuldigen
            // Dit is het kleinste geval, geen splitsing meer nodig
            if (size == 1)
            {
                return new int[,] { { matrixA[0, 0] * matrixB[0, 0] } };
            }
            
            // Als de matrix klein is, gebruik normale methode
            // De drempelwaarde 64 is een gangbare keuze
            if (size <= 64)
            {
                return NormalMultiply.Multiply(matrixA, matrixB);
            }

            // Stap 1: Splits beide matrices in 4 kleinere stukjes
            MatrixHelper.Split(matrixA, out int[,] a, out int[,] b, 
                                        out int[,] c, out int[,] d);
            MatrixHelper.Split(matrixB, out int[,] e, out int[,] f, 
                                        out int[,] g, out int[,] h);

            // Stap 2: Bereken de 7 tussenresultaten van Strassen
            // Elke M roept Multiply opnieuw aan met kleinere matrices
            // Dit is de recursie! De functie roept zichzelf aan
            int[,] m1 = Multiply(MatrixHelper.Add(a, d),      MatrixHelper.Add(e, h));
            int[,] m2 = Multiply(MatrixHelper.Add(c, d),      e);
            int[,] m3 = Multiply(a,                            MatrixHelper.Subtract(f, h));
            int[,] m4 = Multiply(d,                            MatrixHelper.Subtract(g, e));
            int[,] m5 = Multiply(MatrixHelper.Add(a, b),      h);
            int[,] m6 = Multiply(MatrixHelper.Subtract(c, a), MatrixHelper.Add(e, f));
            int[,] m7 = Multiply(MatrixHelper.Subtract(b, d), MatrixHelper.Add(g, h));

            // Stap 3: Combineer de 7 tussenresultaten naar het eindresultaat
            // Deze formules zijn Strassen's wiskundige ontdekking
            int[,] topLeft     = MatrixHelper.Add(MatrixHelper.Subtract(
                                    MatrixHelper.Add(m1, m4), m5), m7);
            int[,] topRight    = MatrixHelper.Add(m3, m5);
            int[,] bottomLeft  = MatrixHelper.Add(m2, m4);
            int[,] bottomRight = MatrixHelper.Add(MatrixHelper.Subtract(
                                    MatrixHelper.Add(m1, m3), m2), m6);

            // Stap 4: Voeg de 4 stukjes samen tot het eindresultaat
            return MatrixHelper.Combine(topLeft, topRight, bottomLeft, bottomRight);
        }
    }
}