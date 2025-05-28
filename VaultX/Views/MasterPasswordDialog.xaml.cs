using System.Windows;
using System.Windows.Input;

namespace VaultX.Views;

public partial class MasterPasswordDialog : Window
{
    public string MasterPassword { get; private set; }

    public MasterPasswordDialog()
    {
        InitializeComponent();

        MasterPasswordBox.Focus();
    }

    private void Unlock_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(MasterPasswordBox.Password))
        {
            ShowError("Bitte geben Sie ein Master-Passwort ein.");
            return;
        }

        MasterPassword = MasterPasswordBox.Password;
        DialogResult = true;
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void MasterPasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            Unlock_Click(sender, e);
        }
    }

    private void ShowError(string message)
    {
        ErrorTextBlock.Text = message;
        ErrorTextBlock.Visibility = Visibility.Visible;
    }
}