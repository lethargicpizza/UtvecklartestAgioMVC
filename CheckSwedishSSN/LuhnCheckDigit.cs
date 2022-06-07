using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UtvecklartestAgioMVC.CheckSwedishSSN
{
    public class LuhnCheckDigit : IComparable, IComparable<LuhnCheckDigit>,
                IEquatable<LuhnCheckDigit>
    {
        /// <summary>
        ///     Initializes a new instance of the LuhnCheckDigit class.
        /// </summary>
        /// <param name="significantNumberOfDigits">
        ///     A number used to determin the number of digits,
        ///     counted from right to left, to be a part of the validation.
        /// </param>
        /// <param name="number">The number to be validated.</param>
        public LuhnCheckDigit(string number = "", uint significantNumberOfDigits = 0)
        {
            Number = number;
            SignificantNumberOfDigits = significantNumberOfDigits;
        }

        /// <summary>
        ///     Gets or sets the identification number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the identification number is valid.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                var digits = ToIntArray();

                if (!digits.Any())
                {
                    return false;
                }

                var multiplier = 1;
                var sum = 0;
                foreach (var digit in digits.Reverse())
                {
                    var digitProduct = digit * multiplier;
                    sum += digitProduct / 10 + digitProduct % 10;
                    multiplier ^= 3; // Toggles between 1 and 2 (1 XOR 3 = 2, 2 XOR 3 = 1)
                }

                return sum % 10 == 0;
            }
        }

        /// <summary>
        ///     Gets the identification number containing only digits.
        /// </summary>
        protected string SanitizedNumber =>
            Regex.Replace(Number ?? string.Empty, @"\D", string.Empty);

        /// <summary>
        ///     Gets or sets the number of digits, counted from right to left,
        ///     to be validated.
        /// </summary>
        public uint SignificantNumberOfDigits { get; set; }

        /// <summary>
        ///     Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        ///     A signed number indicating the relative values of this
        ///     instance and the value parameter.Less than zero: this instance is
        ///     less than value. Zero: this instance is equal to value.
        ///     Greater than zero: this instance is greater than value.
        /// </returns>
        public virtual int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 1;
            }

            var other = obj as LuhnCheckDigit;

            if (other == null)
            {
                throw new ArgumentException("Object is not a LuhnCheckDigit.");
            }

            return CompareTo(other);
        }

        /// <summary>
        ///     Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        ///     A signed number indicating the relative values of this
        ///     instance and the value parameter.Less than zero: this instance is
        ///     less than value. Zero: this instance is equal to value.
        ///     Greater than zero: this instance is greater than value.
        /// </returns>
        public int CompareTo(LuhnCheckDigit other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            if (Equals(other))
            {
                return 0;
            }

            var result = string.CompareOrdinal(Number, other.Number);
            if (result == 0)
            {
                result = SignificantNumberOfDigits.CompareTo(other.SignificantNumberOfDigits);
            }

            return result;
        }

        /// <summary>
        ///     Determines whether this instance and another specified
        ///     LuhnCheckDigit object have the same value.
        /// </summary>
        /// <param name="other">A LuhnCheckDigit.</param>
        /// <returns>
        ///     true if the value of the value parameter is the
        ///     same as this instance; otherwise, false.
        /// </returns>
        public bool Equals(LuhnCheckDigit other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetHashCode() != other.GetHashCode())
            {
                return false;
            }

            return string.Equals(Number, other.Number) &&
                   SignificantNumberOfDigits.Equals(other.SignificantNumberOfDigits);
        }

        /// <summary>
        ///     Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a deep copy of this instance.</returns>
        public LuhnCheckDigit Copy() => (LuhnCheckDigit)MemberwiseClone();

        /// <summary>
        ///     Returns an array of integers containing the rightmost significant digits of this instance.
        /// </summary>
        /// <returns>An int array.</returns>
        protected virtual int[] ToIntArray()
        {
            var digits = SanitizedNumber.ToArray();

            // If specified take the rightmost significant digits.
            if (SignificantNumberOfDigits > 0)
            {
                digits = digits.Skip(Math.Max(0, digits.Count() - (int)SignificantNumberOfDigits)).ToArray();
            }

            return digits.Select(x => (int)char.GetNumericValue(x)).ToArray();
        }

        /// <summary>
        ///     Determines whether this instance of IdentificationNumber and a specified object,
        ///     which must also be a IdentificationNumber object, have the same value.
        /// </summary>
        /// <param name="obj">An Object.</param>
        /// <returns>
        ///     true if obj is a IdentificationNumber and its value is the same as this
        ///     instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj) => Equals(obj as LuhnCheckDigit);

        /// <summary>
        ///     Returns the hash code for this object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked // "overflow" is ok
            {
                var hash = (int)2166136261;

                hash = (hash * 16777619) ^ Number?.GetHashCode() ?? 0;
                hash = (hash * 16777619) ^ SignificantNumberOfDigits.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        ///     Converts the attributes of this IdentificationNumber to a human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{Number} [{SignificantNumberOfDigits} => {string.Join(string.Empty, ToIntArray())}]";

        /// <summary>
        ///     Determines whether two specified LuhnCheckDigit objects have the same value.
        /// </summary>
        /// <param name="a">A LuhnCheckDigit or null.</param>
        /// <param name="b">A LuhnCheckDigit or null.</param>
        /// <returns>true if the value of objA is the same as the value of objB; otherwise, false.</returns>
        public static bool operator ==(LuhnCheckDigit a, LuhnCheckDigit b)
            => a?.Equals(b) ?? ReferenceEquals(b, null);

        /// <summary>
        ///     Determines whether two specified LuhnCheckDigit objects have different values.
        /// </summary>
        /// <param name="a">A LuhnCheckDigit or null.</param>
        /// <param name="b">A LuhnCheckDigit or null.</param>
        /// <returns>true if the value of objA is different from the value of objB; otherwise, false.</returns>
        public static bool operator !=(LuhnCheckDigit a, LuhnCheckDigit b) => !(a == b);
    }
}