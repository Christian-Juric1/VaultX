namespace VaultX.Models;

public class VaultData
{
    public List<PasswordEntry> Entries { get; set; } = new List<PasswordEntry>();
    public string Version { get; set; } = "1.0";
    public DateTime LastBackup { get; set; }
}