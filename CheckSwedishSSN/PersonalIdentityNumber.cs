using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace UtvecklartestAgioMVC.CheckSwedishSSN
{
    /// <summary>
    ///     Range of characteristics pertaining to, and differentiating between, masculinity and femininity.
    /// </summary>
    public enum Gender
    {
        Unknown,
        Female,
        Male,
        Other
    }

    /// <summary>
    ///     Represents a Swedish personal identity number.
    /// </summary>
    public class PersonalIdentityNumber : LuhnCheckDigit
    {
        /// <summary>
        ///     The regular expression to validate the format of a personal identity number.
        /// </summary>
        private static readonly Regex PersonalIdentityNumberRegex =
            new Regex(@"^(\d{6}[-+]?|\d{8}-?)\d{4}$");

        /// <summary>
        ///     Initializes a new instance of the PersonalIdentityNumber class.
        /// </summary>
        /// <param name="number">The personal identity number to be validated.</param>
        public PersonalIdentityNumber(string number = "")
            : base(number, 10)
        {
            // Empty!
        }

        /// <summary>
        ///     Gets the gender of the person the personal identity number belongs to.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this instance is invalid.</exception>
        public Gender Gender
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException();
                }

                // The third number in the birth number, the second last in the 
                // personal identity number, specifies the person’s sex; 
                // even indicating a female, odd indicating a male.
                return Number[Number.Length - 2] % 2 == 0 ? Gender.Female : Gender.Male;
            }
        }

        /// <summary>
        ///     Gets the DateTime value of the birthdate part of the personal identification number.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this instance is invalid.</exception>
        public DateTime Birthdate
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException();
                }

                TryParseBirthdate(out DateTime birthDate);
                return birthDate;
            }
        }

        /// <summary>
        ///     Gets the birth number of the personal identification number.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this instance is invalid.</exception>
        public string BirthNumber
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException();
                }

                var sanitizedNumber = SanitizedNumber;
                return sanitizedNumber.Substring(sanitizedNumber.Length - 4, 3);
            }
        }

        /// <summary>
        ///     Gets the check digit of the personal identification number.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this instance is invalid.</exception>
        public int CheckDigit
        {
            get
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException();
                }

                return (int)char.GetNumericValue(SanitizedNumber.Last());
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this PersonalIdentityNumber is valid.
        /// </summary>
        public override bool IsValid =>
            TryParseBirthdate(out DateTime birthdate) && base.IsValid;

        /// <summary>
        ///     Converts this PersonalIdentityNumber to a human-readable string using the long year pattern.
        /// </summary>
        /// <returns>A string representation of value of the this PersonalIdentityNumber.</returns>
        public override string ToString() => ToString("Y");

        /// <summary>
        ///     Converts this PersonalIdentityNumber to a human-readable string using specified format.
        /// </summary>
        /// <param name="format">A standard format string (see Remarks).</param>
        /// <returns>A string representation of value of the this PersonalIdentityNumber.</returns>
        /// <remarks>
        ///     The ToString(String) method returns the string representation of a personal identity number value in a specific
        ///     format.
        ///     The format parameter should contain a single format specifier character that defines the format of the returned
        ///     string. If format is null or an empty string, the format specifier, 'Y', is used.
        ///     <list type="bullet">
        ///         <item>
        ///             Getting a string where the birthdate is specified by eight digits. To do this, use the "Y" format specifier.
        ///         </item>
        ///         <item>
        ///             Getting a string where the birthdate is specified by six digits. To do this, use the "y" format specifier.
        ///         </item>
        ///         <item>
        ///             Getting a string where the personal identity number is unformatted. To do this, use the "g" format
        ///             specifier.
        ///         </item>
        ///     </list>
        /// </remarks>
        public string ToString(string format)
        {
            switch (format)
            {
                case null:
                case "":
                case "Y": // long year pattern
                    if (IsValid)
                    {
                        return $"{Birthdate:yyyyMMdd}-{BirthNumber}{CheckDigit}";
                    }
                    goto case "g";
                case "y": // short year pattern
                    if (IsValid)
                    {
                        var separator = Birthdate <= DateTime.Today.AddYears(-100) ? '+' : '-';
                        return $"{Birthdate:yyMMdd}{separator}{BirthNumber}{CheckDigit}";
                    }
                    goto case "g";
                case "g": // number
                    return Number;
                default:
                    throw new FormatException();
            }
        }

        /// <summary>
        ///     Converts the date part of the personal identification number to a DateTime value.
        ///     A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="result">
        ///     When this method returns, contains the DateTime value contained in birthdate,
        ///     if the conversion succeeded, or default(DateTime) if the conversion failed.
        ///     The conversion fails if the personal identity number is invalid or date part
        ///     is not a valid date. This parameter is passed uninitialized; any value
        ///     originally supplied in result will be overwritten.
        /// </param>
        /// <returns>
        ///     true if the date part of the personal identity number was converted successfully; otherwise, false.
        /// </returns>
        private bool TryParseBirthdate(out DateTime result)
        {
            if (PersonalIdentityNumberRegex.IsMatch(Number))
            {
                try
                {
                    var sanitizedNumber = SanitizedNumber;
                    var isTenDigitNumber = sanitizedNumber.Length == 10;
                    if (isTenDigitNumber)
                    {
                        // Add current century.
                        // "990612-6074" => "9906126074" => "209906126074"
                        //     Number      SanitizedNumber   SanitizedNumber including century
                        sanitizedNumber = sanitizedNumber.Insert(0, (DateTime.Today.Year / 100).ToString());
                    }

                    result = new DateTime(
                        int.Parse(sanitizedNumber.Substring(0, 4)), // year
                        int.Parse(sanitizedNumber.Substring(4, 2)), // month
                        int.Parse(sanitizedNumber.Substring(6, 2)) // day
                    );

                    if (isTenDigitNumber)
                    {
                        // If Number contains a plus sign (+) the person is +100.
                        // "010927-1049" => 2001-09-27 (2001-09-27 => no adjustment needed!)
                        // "990612-6074" => 1999-06-12 (2099-06-12 => -100 years)
                        // "9906126074"  => 1999-06-12 (2099-06-12 => -100 years)
                        // "010927+1049" => 1901-09-27 (2001-09-27 => -100 years)
                        // "990612+6074" => 1899-06-12 (2099-06-12 => -100 years, -100 years)

                        if (result > DateTime.Today)
                        {
                            result = result.AddYears(-100);
                        }

                        if (Number.Contains("+"))
                        {
                            result = result.AddYears(-100);
                        }
                    }

                    return true;
                }
                catch
                {
                    // Fall through at any exception!
                }
            }

            result = default(DateTime);

            return false;
        }
    }
}
