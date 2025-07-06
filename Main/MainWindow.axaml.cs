using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CLibray;
using Main;

namespace avalonia5;

public partial class MainWindow : Window
{
    CLibray.CLogger _logger;

    public MainWindow()
    {
        _logger = new CLibray.CLogger("MainWindow");
        InitializeComponent();
    }

    async void btnFile_Click(object sender, RoutedEventArgs args)
    {
        await OpenFile();
    }

    async void fileOpen_Click(object sender, RoutedEventArgs args)
    {
        await OpenFile();
    }


    async Task OpenFile()
    {
        string funcName = nameof(OpenFile);

        var filePath = await CFile.ShowOpenFileDialogAsync(this);
        if (filePath is not null)
        {
            txtFile.Text = filePath;
            _logger.Log($"{funcName}() // File={filePath}");

            txtContent.Text = CFile.ReadAllText(filePath);
        }
    }

    void fileExit_Click(object sender, RoutedEventArgs args)
    {
        Close();
    }

    void helpAbout_Click(object sender, RoutedEventArgs args)
    {
        var window = new AboutWindow();
        window.ShowDialog(this);
    }

}