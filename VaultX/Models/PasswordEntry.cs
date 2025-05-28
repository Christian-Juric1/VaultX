using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VaultX.Models;

public class PasswordEntry : INotifyPropertyChanged
{
    private string _website;
    private string _email;
    private string _encryptedPassword;
    private DateTime _createdDate;
    private DateTime _lastModifiedDate;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Website
    {
        get => _website;
        set
        {
            _website = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string EncryptedPassword
    {
        get => _encryptedPassword;
        set
        {
            _encryptedPassword = value;
            OnPropertyChanged();
        }
    }

    public DateTime CreatedDate
    {
        get => _createdDate;
        set
        {
            _createdDate = value;
            OnPropertyChanged();
        }
    }

    public DateTime LastModifiedDate
    {
        get => _lastModifiedDate;
        set
        {
            _lastModifiedDate = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
