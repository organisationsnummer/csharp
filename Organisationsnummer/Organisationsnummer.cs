using System.Text.RegularExpressions;
using PerNr = Personnummer.Personnummer;
using System;

namespace Organisationsnummer
{
    public class Organisationsnummer
    {
        #region Static

        // ReSharper disable StringLiteralTypo
        private static readonly string[] FirmaTypes =
        {
            // String to map different company types.
            // Will only pick 0-9, but we use 10 to be EF as we want it constant.
            "Okänt", // 0
            "Dödsbon", // 1
            "Stat, landsting, kommun eller församling", // 2
            "Utländska företag som bedriver näringsverksamhet eller äger fastigheter i Sverige", // 3
            "Okänt", // 4
            "Aktiebolag", // 5
            "Enkelt bolag", // 6
            "Ekonomisk förening eller bostadsrättsförening", // 7
            "Ideella förening och stiftelse", // 8
            "Handelsbolag, kommanditbolag och enkelt bolag", // 9
            "Enskild firma", // 10
        };

        private static readonly Regex regex =
            new Regex(@"^(\d{2}){0,1}(\d{2})(\d{2})(\d{2})([-+]?)?((?!000)\d{3})(\d)$");

        private static bool LuhnCheck(string value)
        {
            var t = value.ToCharArray().Select(d => d - 48).ToArray();
            var sum = 0;
            for (var i = 0; i < t.Length; i++)
            {
                var temp = t[i];
                temp *= 2 - (i % 2);
                if (temp > 9)
                {
                    temp -= 9;
                }

                sum += temp;
            }

            return sum % 10 == 0;
        }

        /// <summary>
        /// Parse a string to an organisationsnummer.
        /// </summary>
        /// <param name="number">Organisationsnummer as string to base the object on.</param>
        /// <returns>Organisationsnummer as an object</returns>
        /// <exception cref="OrganisationsnummerException">Thrown on parse error.</exception>
        public static Organisationsnummer Parse(string number) => new(number);

        /// <summary>
        /// Validates a string as a organisationsnummer.
        /// </summary>
        /// <param name="number">Organisationsnummer to validate.</param>
        /// <returns>True on valid organisationsnummer.</returns>
        public static bool Valid(string number)
        {
            try
            {
                Parse(number);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        private string? _number = null;
        private PerNr? _personnummer = null;

        /// <summary>
        /// Get organisation VAT.
        /// </summary>
        public string VatNumber => @$"SE{ShortString}01";

        /// <summary>
        /// Determine is the organisationsnummer is a personnummer or not.
        /// </summary>
        public bool IsPersonnummer => _personnummer != null;

        /// <summary>
        /// Get Personnummer instance (if IsPersonnummer).
        /// </summary>
        public PerNr? Personnummer => _personnummer;

        /// <summary>
        /// Get type of company/firm.
        /// </summary>
        public string Type =>
            IsPersonnummer
                ? FirmaTypes[10]
                : FirmaTypes[int.Parse(_number!.Substring(0, 1))];

        /// <summary>
        /// Organisationsnummer constructor.
        /// </summary>
        /// <param name="number">Organisationsnummer as string to base the object on.</param>
        /// <returns>Organisationsnummer as an object</returns>
        /// <exception cref="OrganisationsnummerException">Thrown on parse error.</exception>
        public Organisationsnummer(string number)
        {
            InnerParse(number);
        }

        /// <summary>
        /// Format the Organisationsnummer and return it as a string.
        /// </summary>
        /// <param name="separator">If to include separator (-) or not.</param>
        /// <returns></returns>
        public string Format(bool separator = true)
        {
            var num = ShortString;
            return separator ? $"{num!.Substring(0, 6)}-{num!.Substring(6)}" : num!;
        }

        private string ShortString => ((IsPersonnummer ? _personnummer!.Format(false) : _number)!)
                .Replace("-", "")
                .Replace("+", "");

        private void InnerParse(string input)
        {
            try
            {
                var matches = regex.Matches(input);
                if (matches.Count < 1 || matches[0].Groups.Count < 7)
                {
                    throw new OrganisationsnummerException();
                }

                input = input.Replace("-", "")
                             .Replace("+", "");
                // Get regexp match
                //var matches = regex.Matches(input);
                var groups = matches[0].Groups;

                // if [1] is set, it may only be prefixed with 16.
                if (!string.IsNullOrEmpty(groups[1].Value) && int.Parse(groups[1].Value) != 16)
                {
                    throw new OrganisationsnummerException();
                }

                // Third digit must be more than 20.
                // Second digit must be more than 10.
                if (int.Parse(groups[3].Value) < 20 ||
                    int.Parse(groups[2].Value) < 10 ||
                    !LuhnCheck(input))
                {
                    throw new OrganisationsnummerException();
                }

                _number = input;
            }
            catch (Exception ex)
            {
                try
                {
                    _personnummer = PerNr.Parse(input);
                }
                catch
                {
                    throw ex;
                }
            }
        }
    }
}
