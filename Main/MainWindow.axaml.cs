using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
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

        treeFile.SelectionChanged += treeFile_SelectionChanged;
    }

    void btnFile_Click(object sender, RoutedEventArgs args)
    {
        CFile.GetFileNodes("/home/user", out CFile.CFileNode nodeRoot);
        PopulateFileTree(nodeRoot);
    }

    void PopulateFileTree(CFile.CFileNode nodeRoot)
    {
        treeFile.Items.Clear();
        PopulateFileTreeSub(treeFile.Items, nodeRoot);
    }

    void PopulateFileTreeSub(ItemCollection parentItems, CFile.CFileNode nodeParent)
    {
        foreach (var node in nodeParent.Children)
        {
            TreeViewItem tvi = new TreeViewItem();
            tvi.Header = node.Name;
            tvi.Tag = node;
            parentItems.Add(tvi);

            if (node.NodeType == CFile.CFileNode.ENodeType.Folder)
            {
                tvi.Background = new SolidColorBrush(Colors.LightGreen);
                PopulateFileTreeSub(tvi.Items, node);
            }
        }

    }
    void treeFile_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems[0] is TreeViewItem selectedItem)
        {
            txtFile.Text = selectedItem.Header?.ToString() ?? string.Empty;
            _logger.Log($"treeFile_SelectedItemChanged() // Selected={txtFile.Text}");

            CFile.CFileNode? node = selectedItem.Tag as CFile.CFileNode;
            if (node != null)
            {
                if (node.NodeType == CFile.CFileNode.ENodeType.File)
                {
                    txtContent.Text = CFile.ReadAllText(node.Path);
                }
            }
        }
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