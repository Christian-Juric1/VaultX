using System.Windows;
using VaultX.Converters;
using VaultX.ViewModels;

namespace VaultX;

public partial class MainWindow : Window
{
    private MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        _viewModel = new MainViewModel();
        this.DataContext = _viewModel;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var passwordDialog = new Views.MasterPasswordDialog();
        if (passwordDialog.ShowDialog() == true)
        {
            await _viewModel.InitializeAsync(passwordDialog.MasterPassword);
        }
        else
        {
            Close();
        }
    }
}