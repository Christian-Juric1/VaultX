using System.IO;
using System.Text.Json;
using VaultX.Models;

namespace VaultX.Services;

public class DataService
{
    private readonly string _dataFilePath;
    private readonly string _masterPassword;

    public DataService(string masterPassword, string dataFilePath = "passwords.vaultx")
    {
        _masterPassword = masterPassword;
        _dataFilePath = dataFilePath;
    }

    public async Task<VaultData> LoadDataAsync()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                return new VaultData();
            }

            string encryptedContent = await File.ReadAllTextAsync(_dataFilePath);
            string decryptedContent = EncryptionService.Decrypt(encryptedContent, _masterPassword);

            if (string.IsNullOrEmpty(decryptedContent) || decryptedContent.Contains("[Entschlüsselung fehlgeschlagen]"))
            {
                throw new UnauthorizedAccessException("Falsches Master-Passwort");
            }

            return JsonSerializer.Deserialize<VaultData>(decryptedContent) ?? new VaultData();
        }
        catch (JsonException)
        {
            throw new InvalidDataException("Datei ist beschädigt");
        }
    }

    public async Task SaveDataAsync(VaultData vaultData)
    {
        try
        {
            string jsonContent = JsonSerializer.Serialize(vaultData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            string encryptedContent = EncryptionService.Encrypt(jsonContent, _masterPassword);
            await File.WriteAllTextAsync(_dataFilePath, encryptedContent);
        }
        catch (Exception ex)
        {
            throw new IOException($"Fehler beim Speichern: {ex.Message}");
        }
    }
}