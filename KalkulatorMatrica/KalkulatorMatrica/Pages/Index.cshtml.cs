using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MatrixOperationsWebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int[][] MatrixA { get; set; }

        [BindProperty]
        public int[][] MatrixB { get; set; }

        [BindProperty]
        public string Operation { get; set; }

        [BindProperty]
        public int Scalar { get; set; } = 1;

        [BindProperty]
        public int Power { get; set; } = 1;

        public int[][] Result { get; set; }

        public void OnGet()
        {
            // Inicijalizacija stranice ako je potrebno
        }

        public IActionResult OnPost()
        {
            try
            {
                // Pokretanje operacije na osnovu korisničkog unosa
                switch (Operation)
                {
                    case "Add":
                        if (!AreDimensionsEqual(MatrixA, MatrixB))
                            throw new InvalidOperationException("Matrice moraju biti istih dimenzija za zbrajanje!");
                        Result = AddMatrices(MatrixA, MatrixB);
                        break;
                    case "Subtract":
                        if (!AreDimensionsEqual(MatrixA, MatrixB))
                            throw new InvalidOperationException("Matrice moraju biti istih dimenzija za oduzimanje!");
                        Result = SubtractMatrices(MatrixA, MatrixB);
                        break;
                    case "Multiply":
                        if (MatrixA[0].Length != MatrixB.Length)
                            throw new InvalidOperationException("Broj stupaca matrice A mora biti jednak broju redaka matrice B za množenje!");
                        Result = MultiplyMatrices(MatrixA, MatrixB);
                        break;
                    case "ScalarMultiplyA":
                        Result = ScalarMultiply(MatrixA, Scalar);
                        break;
                    case "ScalarMultiplyB":
                        Result = ScalarMultiply(MatrixB, Scalar);
                        break;
                    case "TransposeA":
                        Result = Transpose(MatrixA);
                        break;
                    case "TransposeB":
                        Result = Transpose(MatrixB);
                        break;
                    case "PowerA":
                        if (!IsSquare(MatrixA))
                            throw new InvalidOperationException("Matrica A mora biti kvadratna za potenciranje!");
                        Result = PowerMatrix(MatrixA, Power);
                        break;
                    case "PowerB":
                        if (!IsSquare(MatrixB))
                            throw new InvalidOperationException("Matrica B mora biti kvadratna za potenciranje!");
                        Result = PowerMatrix(MatrixB, Power);
                        break;
                    case "CheckSymmetryA":
                        if (!IsSquare(MatrixA))
                            throw new InvalidOperationException("Matrica A mora biti kvadratna za provjeru simetrije!");
                        TempData["Message"] = IsSymmetric(MatrixA) ? "Matrica A je simetrična." : "Matrica A nije simetrična.";
                        Result = null;
                        break;
                    case "CheckSymmetryB":
                        if (!IsSquare(MatrixB))
                            throw new InvalidOperationException("Matrica B mora biti kvadratna za provjeru simetrije!");
                        TempData["Message"] = IsSymmetric(MatrixB) ? "Matrica B je simetrična." : "Matrica B nije simetrična.";
                        Result = null;
                        break;
                    case "DeterminantA":
                        ViewData["DeterminantResult"] = $"Determinanta matrice A: {Determinant(MatrixA)}";
                        break;
                    case "DeterminantB":
                        ViewData["DeterminantResult"] = $"Determinanta matrice B: {Determinant(MatrixB)}";
                        break;
                    default:
                        throw new InvalidOperationException("Nepoznata operacija.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Greška: {ex.Message}");
                return Page();
            }
        }

        private bool AreDimensionsEqual(int[][] A, int[][] B)
        {
            return A.Length == B.Length && A[0].Length == B[0].Length;
        }

        private bool IsSquare(int[][] matrix)
        {
            return matrix.Length == matrix[0].Length;
        }

        private bool IsSymmetric(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] != matrix[j][i])
                        return false;
                }
            }
            return true;
        }
        private int[][] AddMatrices(int[][] A, int[][] B)
        {
            if (A.Length != B.Length || A[0].Length != B[0].Length)
                throw new InvalidOperationException("Matrice moraju biti istih dimenzija za zbrajanje!");

            int[][] result = new int[A.Length][];
            for (int i = 0; i < A.Length; i++)
            {
                result[i] = new int[A[i].Length];
                for (int j = 0; j < A[i].Length; j++)
                {
                    result[i][j] = A[i][j] + B[i][j];
                }
            }
            return result;
        }

        private int[][] SubtractMatrices(int[][] A, int[][] B)
        {
            if (A.Length != B.Length || A[0].Length != B[0].Length)
                throw new InvalidOperationException("Matrice moraju biti istih dimenzija za oduzimanje!");

            int[][] result = new int[A.Length][];
            for (int i = 0; i < A.Length; i++)
            {
                result[i] = new int[A[i].Length];
                for (int j = 0; j < A[i].Length; j++)
                {
                    result[i][j] = A[i][j] - B[i][j];
                }
            }
            return result;
        }

        private int[][] MultiplyMatrices(int[][] A, int[][] B)
        {
            if (A[0].Length != B.Length)
                throw new InvalidOperationException("Broj stupaca matrice A mora biti jednak broju redaka matrice B!");

            int[][] result = new int[A.Length][];
            for (int i = 0; i < A.Length; i++)
            {
                result[i] = new int[B[0].Length];
                for (int j = 0; j < B[0].Length; j++)
                {
                    result[i][j] = 0;
                    for (int k = 0; k < B.Length; k++)
                    {
                        result[i][j] += A[i][k] * B[k][j];
                    }
                }
            }
            return result;
        }

        private int[][] ScalarMultiply(int[][] matrix, int scalar)
        {
            int[][] result = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                result[i] = new int[matrix[i].Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    result[i][j] = matrix[i][j] * scalar;
                }
            }
            return result;
        }

        private int[][] Transpose(int[][] matrix)
        {
            int[][] result = new int[matrix[0].Length][];
            for (int i = 0; i < matrix[0].Length; i++)
            {
                result[i] = new int[matrix.Length];
                for (int j = 0; j < matrix.Length; j++)
                {
                    result[i][j] = matrix[j][i];
                }
            }
            return result;
        }

        private int[][] PowerMatrix(int[][] matrix, int power)
        {
            if (matrix.Length != matrix[0].Length)
                throw new InvalidOperationException("Matrica mora biti kvadratna za potenciranje!");

            int[][] result = matrix;
            for (int p = 1; p < power; p++)
            {
                result = MultiplyMatrices(result, matrix);
            }
            return result;
        }

        private int Determinant(int[][] matrix)
        {
            // Check if the matrix is square
            if (matrix.Length != matrix[0].Length)
                throw new InvalidOperationException("Matrica mora biti kvadratna za izračun determinante!");

            int size = matrix.Length;

            // Base case for 2x2 matrix
            if (size == 2)
            {
                return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            }

            // Recursive case
            int determinant = 0;
            for (int col = 0; col < size; col++)
            {
                int[][] minor = GetMinor(matrix, 0, col);
                determinant += matrix[0][col] * Determinant(minor) * (col % 2 == 0 ? 1 : -1);
            }

            return determinant;
        }

        // Helper method to get the minor matrix by excluding a row and column
        private int[][] GetMinor(int[][] matrix, int excludedRow, int excludedCol)
        {
            int size = matrix.Length;
            int[][] minor = new int[size - 1][];

            for (int i = 0, row = 0; i < size; i++)
            {
                if (i == excludedRow) continue;

                minor[row] = new int[size - 1];
                for (int j = 0, col = 0; j < size; j++)
                {
                    if (j == excludedCol) continue;

                    minor[row][col] = matrix[i][j];
                    col++;
                }
                row++;
            }

            return minor;
        }



    }
}
