using System.Windows;

namespace VaultX.Views;

public partial class AddEditPasswordDialog : Window
{
    public string Website { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public AddEditPasswordDialog()
    {
        InitializeComponent();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        Website = WebsiteTextBox.Text;
        Email = EmailTextBox.Text;
        Password = PasswordBox.Password;

        if (string.IsNullOrWhiteSpace(Website) || string.IsNullOrWhiteSpace(Password))
        {
            MessageBox.Show("Website und Passwort sind Pflichtfelder!", "Fehler",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        WebsiteTextBox.Text = Website ?? "";
        EmailTextBox.Text = Email ?? "";
        PasswordBox.Password = Password ?? "";
    }
}