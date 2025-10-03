using System;

namespace NNPTPZ1
{

    namespace Mathematics
    {
        public class ComplexNumber
        {
            public double RealPart { get; set; }
            public double ImaginaryPart { get; set; }

            public static readonly ComplexNumber Zero = new ComplexNumber();

            public override bool Equals(object obj) =>
                obj is ComplexNumber number &&
                RealPart == number.RealPart &&
                ImaginaryPart == number.ImaginaryPart;

            public ComplexNumber Multiply(ComplexNumber secondFactor) =>

                new ComplexNumber
                {
                    RealPart = RealPart * secondFactor.RealPart - ImaginaryPart * secondFactor.ImaginaryPart,
                    ImaginaryPart = RealPart * secondFactor.ImaginaryPart + ImaginaryPart * secondFactor.RealPart
                };

            public double GetAbsoluteValue() =>
                Math.Sqrt(RealPart * RealPart + ImaginaryPart * ImaginaryPart);

            public ComplexNumber Add(ComplexNumber secondFactor) =>
                new ComplexNumber
                {
                    RealPart = RealPart + secondFactor.RealPart,
                    ImaginaryPart = ImaginaryPart + secondFactor.ImaginaryPart
                };
            public double GetAngleInRadians() =>

                Math.Atan(ImaginaryPart / RealPart);

            public ComplexNumber Subtract(ComplexNumber secondFactor) =>
                new ComplexNumber
                {
                    RealPart = RealPart - secondFactor.RealPart,
                    ImaginaryPart = ImaginaryPart - secondFactor.ImaginaryPart
                };

            public override string ToString() =>

              $"({RealPart} + {ImaginaryPart}i)";


            public ComplexNumber Divide(ComplexNumber other)
            {
                ComplexNumber divident = Multiply(new ComplexNumber
                {
                    RealPart = other.RealPart,
                    ImaginaryPart = -other.ImaginaryPart
                });

                double divisor = other.RealPart * other.RealPart + other.ImaginaryPart * other.ImaginaryPart;

                return new ComplexNumber
                {
                    RealPart = divident.RealPart / divisor,
                    ImaginaryPart = divident.ImaginaryPart / divisor
                };
            }

        }
    }
}
