using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using VaultX.Models;
using VaultX.Services;

namespace VaultX.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private DataService _dataService;
    private VaultData _vaultData;
    private string _searchText = "";
    private PasswordEntry _selectedEntry;
    private string _masterPassword;

    public ObservableCollection<PasswordEntry> Entries { get; set; } = new ObservableCollection<PasswordEntry>();
    public ObservableCollection<PasswordEntry> FilteredEntries { get; set; } = new ObservableCollection<PasswordEntry>();

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            FilterEntries();
        }
    }

    public PasswordEntry SelectedEntry
    {
        get => _selectedEntry;
        set
        {
            _selectedEntry = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddEntryCommand { get; }
    public ICommand EditEntryCommand { get; }
    public ICommand DeleteEntryCommand { get; }
    public ICommand CopyPasswordCommand { get; }

    public MainViewModel()
    {
        AddEntryCommand = new Utils.RelayCommand(AddEntry);
        EditEntryCommand = new Utils.RelayCommand(EditEntry, () => SelectedEntry != null);
        DeleteEntryCommand = new Utils.RelayCommand(DeleteEntry, () => SelectedEntry != null);
        CopyPasswordCommand = new Utils.RelayCommand(CopyPassword, () => SelectedEntry != null);
    }

    public async Task InitializeAsync(string masterPassword)
    {
        _masterPassword = masterPassword;
        _dataService = new DataService(masterPassword);

        try
        {
            _vaultData = await _dataService.LoadDataAsync();
            Entries.Clear();
            foreach (var entry in _vaultData.Entries)
            {
                Entries.Add(entry);
            }
            FilterEntries();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}", "Fehler",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void FilterEntries()
    {
        FilteredEntries.Clear();
        var filtered = string.IsNullOrEmpty(SearchText)
            ? Entries
            : Entries.Where(e =>
                e.Website.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                e.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var entry in filtered)
        {
            FilteredEntries.Add(entry);
        }
    }

    private void AddEntry()
    {
        var dialog = new Views.AddEditPasswordDialog();
        if (dialog.ShowDialog() == true)
        {
            var newEntry = new PasswordEntry
            {
                Website = dialog.Website,
                Email = dialog.Email,
                EncryptedPassword = EncryptionService.Encrypt(dialog.Password, _masterPassword),
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            Entries.Add(newEntry);
            _vaultData.Entries.Add(newEntry);
            FilterEntries();
            SaveData();
        }
    }

    private void EditEntry()
    {
        if (SelectedEntry == null) return;

        var dialog = new Views.AddEditPasswordDialog
        {
            Website = SelectedEntry.Website,
            Email = SelectedEntry.Email,
            Password = EncryptionService.Decrypt(SelectedEntry.EncryptedPassword, _masterPassword)
        };

        if (dialog.ShowDialog() == true)
        {
            SelectedEntry.Website = dialog.Website;
            SelectedEntry.Email = dialog.Email;
            SelectedEntry.EncryptedPassword = EncryptionService.Encrypt(dialog.Password, _masterPassword);
            SelectedEntry.LastModifiedDate = DateTime.Now;
            FilterEntries();
            SaveData();
        }
    }

    private void DeleteEntry()
    {
        if (SelectedEntry == null) return;

        var result = MessageBox.Show($"Möchten Sie den Eintrag für '{SelectedEntry.Website}' wirklich löschen?",
            "Eintrag löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Entries.Remove(SelectedEntry);
            _vaultData.Entries.Remove(SelectedEntry);
            FilterEntries();
            SaveData();
        }
    }

    private void CopyPassword()
    {
        if (SelectedEntry == null) return;

        string password = EncryptionService.Decrypt(SelectedEntry.EncryptedPassword, _masterPassword);
        Clipboard.SetText(password);
        MessageBox.Show("Passwort in die Zwischenablage kopiert!", "Info",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void SaveData()
    {
        try
        {
            await _dataService.SaveDataAsync(_vaultData);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Fehler",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}