using System.Windows;

namespace VaultX;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Globale Exception-Behandlung
        DispatcherUnhandledException += (sender, args) =>
        {
            MessageBox.Show($"Ein unerwarteter Fehler ist aufgetreten:\n{args.Exception.Message}",
                           "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };
    }
}