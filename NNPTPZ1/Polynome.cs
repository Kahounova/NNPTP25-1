using System.Collections.Generic;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class Polynome
        {
            public List<ComplexNumber> Coefficients { get; set; }

            public Polynome() => Coefficients = new List<ComplexNumber>();

            public void Add(ComplexNumber coefficient) =>
                Coefficients.Add(coefficient);

            /// <summary>
            /// Creates and returns a new polynomial that is the derivative of the current one.
            /// </summary>
            /// <returns>Derivated polynomial</returns>
            public Polynome Derive()
            {
                Polynome derivedPolynome = new Polynome();
                for (int i = 1; i < Coefficients.Count; i++)
                {
                    derivedPolynome.Coefficients.Add(Coefficients[i]
                                                .Multiply(new ComplexNumber() { RealPart = i }));
                }

                return derivedPolynome;
            }

            /// <summary>
            /// Calculates the value of the polynomial for a given real number.
            /// </summary>
            /// <param name="point">The real number at which the polynomial is evaluated.</param>
            /// <returns>The resulting complex value of the polynomial.</returns>
            public ComplexNumber Eval(double point) =>
                Eval(new ComplexNumber { RealPart = point});

            /// <summary>
            /// Calculates the polynomial’s value for a given complex input.
            /// </summary>
            /// <param name="point">The complex number where the polynomial is evaluated.</param>
            /// <returns>The computed complex result.</returns>

            public ComplexNumber Eval(ComplexNumber point)
            {
                ComplexNumber polynomialValue = ComplexNumber.Zero;

                for (int i = 0; i < Coefficients.Count; i++)
                {
                    ComplexNumber currentCoefficient = Coefficients[i];
                    ComplexNumber termValue = currentCoefficient;

                    if (i > 0)
                    {
                        ComplexNumber pointPower = point;
                        for (int j = 0; j < i - 1; j++)
                            pointPower = pointPower.Multiply(point);

                        termValue = termValue.Multiply(pointPower);
                    }

                    polynomialValue = polynomialValue.Add(termValue);
                }

                return polynomialValue;
            }

            public override string ToString()
            {
                string polynomialString = "";

                for (int termIndex = 0; termIndex < Coefficients.Count; termIndex++)
                {
                    polynomialString += Coefficients[termIndex];

                    if (termIndex > 0)
                    {
                        for (int powerIndex = 0; powerIndex < termIndex; powerIndex++)
                        {
                            polynomialString += "x";
                        }
                    }

                    if (termIndex + 1 < Coefficients.Count)
                        polynomialString += " + ";
                }

                return polynomialString;
            }
        }
    }
}
