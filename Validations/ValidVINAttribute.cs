using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Validations
{
    // Module 2: Custom Validation Attribute -> validates VIN length + checksum
    public class ValidVINAttribute : ValidationAttribute
    {
        // Transliteration table used in the real-world VIN checksum algorithm (ISO 3779)
        private static readonly Dictionary<char, int> Transliteration = new()
        {
            {'A',1},{'B',2},{'C',3},{'D',4},{'E',5},{'F',6},{'G',7},{'H',8},
            {'J',1},{'K',2},{'L',3},{'M',4},{'N',5},{'P',7},{'R',9},
            {'S',2},{'T',3},{'U',4},{'V',5},{'W',6},{'X',7},{'Y',8},{'Z',9},
            {'0',0},{'1',1},{'2',2},{'3',3},{'4',4},{'5',5},{'6',6},{'7',7},{'8',8},{'9',9}
        };

        private static readonly int[] Weights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var vin = value as string;

            if (string.IsNullOrWhiteSpace(vin))
                return new ValidationResult("VIN is required");

            vin = vin.Trim().ToUpper();

            if (vin.Length != 17)
                return new ValidationResult("VIN must be exactly 17 characters");

            if (vin.Contains('I') || vin.Contains('O') || vin.Contains('Q'))
                return new ValidationResult("VIN cannot contain the letters I, O or Q");

            // Checksum validation (9th character is the check digit)
            long sum = 0;
            for (int i = 0; i < vin.Length; i++)
            {
                if (!Transliteration.TryGetValue(vin[i], out int digit))
                    return new ValidationResult("VIN contains invalid characters");

                sum += digit * Weights[i];
            }

            int remainder = (int)(sum % 11);
            char expectedCheckDigit = remainder == 10 ? 'X' : remainder.ToString()[0];

            if (vin[8] != expectedCheckDigit)
                return new ValidationResult("Invalid VIN (checksum verification failed)");

            return ValidationResult.Success;
        }
    }
}
