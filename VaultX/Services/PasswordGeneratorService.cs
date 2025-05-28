using System.Security.Cryptography;
using System.Text;

namespace VaultX.Services;

public static class PasswordGeneratorService
{
    private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    public static string GeneratePassword(int length = 16, bool includeUpperCase = true,
        bool includeNumbers = true, bool includeSpecialChars = true)
    {
        if (length < 4) length = 4;

        var characterSet = LowerCase;
        if (includeUpperCase) characterSet += UpperCase;
        if (includeNumbers) characterSet += Numbers;
        if (includeSpecialChars) characterSet += SpecialChars;

        var password = new StringBuilder();
        using (var rng = RandomNumberGenerator.Create())
        {
            // Stelle sicher, dass mindestens ein Zeichen aus jeder gewählten Kategorie vorhanden ist
            if (includeUpperCase) password.Append(GetRandomChar(UpperCase, rng));
            if (includeNumbers) password.Append(GetRandomChar(Numbers, rng));
            if (includeSpecialChars) password.Append(GetRandomChar(SpecialChars, rng));
            password.Append(GetRandomChar(LowerCase, rng));

            // Fülle den Rest zufällig auf
            for (int i = password.Length; i < length; i++)
            {
                password.Append(GetRandomChar(characterSet, rng));
            }
        }

        // Mische das Passwort
        return new string(password.ToString().OrderBy(x => Guid.NewGuid()).ToArray());
    }

    private static char GetRandomChar(string chars, RandomNumberGenerator rng)
    {
        byte[] randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        int randomValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % chars.Length;
        return chars[randomValue];
    }
}