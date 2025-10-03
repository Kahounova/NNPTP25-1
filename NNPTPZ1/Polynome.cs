using System.Collections.Generic;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class Polynome
        {
            /// <summary>
            /// Coe
            /// </summary>
            public List<ComplexNumber> Coefficients { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public Polynome() => Coefficients = new List<ComplexNumber>();

            public void Add(ComplexNumber coefficient) =>
                Coefficients.Add(coefficient);

            /// <summary>
            /// Derives this polynomial and creates new one
            /// </summary>
            /// <returns>Derivated polynomial</returns>
            public Polynome Derive()
            {
                Polynome derivativePolynome = new Polynome();
                for (int i = 1; i < Coefficients.Count; i++)
                {
                    derivativePolynome.Coefficients.Add(Coefficients[i].Multiply(new ComplexNumber() { RealPart = i }));
                }

                return derivativePolynome;
            }

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
            public ComplexNumber Eval(double point) =>
                Eval(new ComplexNumber { RealPart = point, ImaginaryPart = 0 });

            /// <summary>
            /// Evaluates polynomial at given point
            /// </summary>
            /// <param name="x">point of evaluation</param>
            /// <returns>y</returns>
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


            /// <summary>
            /// ToString
            /// </summary>
            /// <returns>String repr of polynomial</returns>
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
