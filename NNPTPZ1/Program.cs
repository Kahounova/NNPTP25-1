using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1
{
    /// <summary>
    /// Produces a Newton fractal image derived from polynomial root iterations.
    /// </summary>
    /// <remarks>
    /// Reference: https://en.wikipedia.org/wiki/Newton_fractal
    /// </remarks>

    class Program
    {
        static int[] imageCoordinates;
        static string outputFilePath;
        static Bitmap fractalBitmap;
        static double xStart, yStart, xIncrement, yIncrement;
        static List<ComplexNumber> roots;
        static Polynome polynomial, derivativePolynomial;
        static Color[] rootColors;
        static int MaximalIndexOfRoot;
        private const string DefaultOutputFilePath = "../../../out.png";
        private const int MaximumNuberOfIterations = 30;
        private const double IterationTolerance = 0.5;
        private const double RootEqualityTolerance = 0.01;
        private const double MinCoordinateThreshold = 0.0001;



        static void Main(string[] args)
        {

            ParseCommandLineArguments(args);
            PrepareCalculationEnvironment();
            MaximalIndexOfRoot = ProcessNewtonIterationAlgorithm();
            SaveOutputImage();
        }

        private static void SaveOutputImage()
        {
            fractalBitmap.Save(outputFilePath ?? DefaultOutputFilePath);
        }

        private static int ProcessNewtonIterationAlgorithm()
        {
            for (int i = 0; i < imageCoordinates[0]; i++)
            {
                for (int j = 0; j < imageCoordinates[1]; j++)
                {
                    ProcessPixelUsingNewtonIteration(i, j);
                }
            }
            return MaximalIndexOfRoot;
        }

        private static void ProcessPixelUsingNewtonIteration(int i, int j)
        {
            ComplexNumber point = PrepareComplexNumberAtPoint(i, j);
            int convergenceIterations = ProcessNewtonIterationOnSinglePixel(ref point);
            int indexOfRoot = FindPolynomeRoot(point);

            ColorizePixel(i, j, convergenceIterations, indexOfRoot);
        }

        /// <summary>
        /// Sets the color of a single pixel based on which root it converged to and how many iterations were performed.
        /// </summary>
        /// <param name="i">The row index of the pixel in the image.</param>
        /// <param name="j">The column index of the pixel in the image.</param>
        /// <param name="convergenceIterations">The number of Newton iterations required for this pixel to converge.</param>
        /// <param name="indexOfRoot">The index of the root to which the pixel converged.</param>
        private static void ColorizePixel(int i, int j, int convergenceIteration, int indexOfRoot)
        {
            Color color = rootColors[indexOfRoot % rootColors.Length];
            color = Color.FromArgb(
                Math.Min(Math.Max(0, color.R - convergenceIteration * 2), 255), 
                Math.Min(Math.Max(0, color.G - convergenceIteration * 2), 255), 
                Math.Min(Math.Max(0, color.B - convergenceIteration * 2), 255)
                );
            fractalBitmap.SetPixel(j, i, color);
        }

        /// <summary>
        /// Finds the index of the nearest root to the given complex point.
        /// If the point does not match any existing root, it is added as a new one.
        /// </summary>
        /// <param name="point">The complex point used for comparison with known roots.</param>
        /// <returns>The index of the matching or newly added root.</returns>
        private static int FindPolynomeRoot(ComplexNumber point)
        {
            bool knownRoot = false;
            int indexOfRoot = 0;
            for (int i = 0; i < roots.Count; i++)
            {
                if (point.Subtract(roots[i]).GetAbsoluteValue() <= RootEqualityTolerance)
                {
                    knownRoot = true;
                    indexOfRoot = i;
                }
            }
            if (!knownRoot)
            {
                roots.Add(point);
                indexOfRoot = roots.Count;
                MaximalIndexOfRoot = indexOfRoot + 1;
            }

            return indexOfRoot;
        }

        /// <summary>
        /// Performs Newton’s iterative method on the specified complex point until it converges to a root.
        /// </summary>
        /// <param name="point">The complex number (passed by reference) representing the pixel being refined.</param>
        /// <returns>The total number of iterations required for convergence.</returns>

        private static int ProcessNewtonIterationOnSinglePixel(ref ComplexNumber point)
        {
            int iterations = 0;
            for (int i = 0; i < MaximumNuberOfIterations; i++)
            {
                ComplexNumber differential = polynomial.Eval(point).Divide(derivativePolynomial.Eval(point));
                point = point.Subtract(differential);
                if (differential.GetAbsoluteValue() >= IterationTolerance)
                {
                    i--;
                }
                iterations++;
            }

            return iterations;
        }

        /// <summary>
        /// Converts a pixel's row and column indices into a complex number
        /// representing its position in the fractal's "world" coordinates.
        /// </summary>
        /// <param name="i">Pixel row index.</param>
        /// <param name="j">Pixel column index.</param>
        /// <returns>The complex coordinates of the pixel.</returns>
        private static ComplexNumber PrepareComplexNumberAtPoint(int i, int j)
        {
            double y = yStart + i * yIncrement;
            double x = xStart + j * xIncrement;

            ComplexNumber point = new ComplexNumber()
            {
                RealPart = x,
                ImaginaryPart = y
            };

            if (point.RealPart == 0)
                point.RealPart = MinCoordinateThreshold;
            if (point.ImaginaryPart == 0)
                point.ImaginaryPart = MinCoordinateThreshold;
            return point;
        }

        /// <summary>
        /// Initializes polynomial, its derivative, color palette, and related data structures
        /// required for generating the Newton fractal.
        /// </summary>
        private static void PrepareCalculationEnvironment()
        {
            roots = new List<ComplexNumber>();
            polynomial = new Polynome();
            polynomial.Coefficients.Add(new ComplexNumber() { RealPart = 1 });
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(ComplexNumber.Zero);
            polynomial.Coefficients.Add(new ComplexNumber() { RealPart = 1 });
            derivativePolynomial = polynomial.Derive();
            Console.WriteLine(polynomial);
            Console.WriteLine(derivativePolynomial);

            rootColors = new Color[]
            {
                Color.Red, 
                Color.Blue, 
                Color.Green, 
                Color.Yellow, 
                Color.Orange, 
                Color.Fuchsia, 
                Color.Gold, 
                Color.Cyan, 
                Color.Magenta
            };
            MaximalIndexOfRoot = 0;
        }

        /// <summary>
        /// Parses command line arguments to initialize image dimensions, 
        /// coordinate bounds, and output path.
        /// </summary>
        private static void ParseCommandLineArguments(string[] args)
        {
            const int indexWidth = 0;
            const int indexHeight = 1;
            const int indexXMin = 2;
            const int indexXMax = 3;       
            const int indexYMin = 4;      
            const int indexOutputPath = 6;


            imageCoordinates = new int[2];
            for (int i = 0; i < imageCoordinates.Length; i++)
            {
                imageCoordinates[i] = int.Parse(args[i]);
            }
            double[] coordinateBounds = new double[indexYMin];
            for (int i = 0; i < coordinateBounds.Length; i++)
            {
                coordinateBounds[i] = double.Parse(args[i + 2]);
            }
            outputFilePath = args[indexOutputPath];
            fractalBitmap = new Bitmap(imageCoordinates[indexWidth], imageCoordinates[indexHeight]);
            xStart = coordinateBounds[indexWidth];
            double xMax = coordinateBounds[indexHeight];
            yStart = coordinateBounds[indexXMin];
            double yMax = coordinateBounds[indexXMax];

            xIncrement = (xMax - xStart) / imageCoordinates[indexWidth];
            yIncrement = (yMax - yStart) / imageCoordinates[indexHeight];
        }
    }
}
