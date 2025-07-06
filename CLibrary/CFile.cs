using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using System.IO;

namespace CLibray;

public static class CFile
{
    public static async Task<string?> ShowOpenFileDialogAsync(Window parent)
    {
        var files = await parent.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false
        });
        return files != null && files.Count > 0 ? files[0].Path.LocalPath : null;
    }


    public static async Task<string?> ShowSaveFileDialogAsync(Window parent)
    {
        var file = await parent.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        { });
        return file?.Path.LocalPath;
    }

    public static async Task<string?> ShowOpenFolderDialogAsync(Window parent)
    {
        var folders = await parent.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            AllowMultiple = false
        });
        return folders != null && folders.Count > 0 ? folders[0].Path.LocalPath : null;
    }

    public static string? ReadAllText(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        return null;
    }

    public static void WriteAllText(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }



    public class CFileNode
    {
        public enum ENodeType { Folder, File };

        public ENodeType NodeType { get; set; }
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public List<CFileNode> Children { get; set; } = new List<CFileNode>();

    }

    public static bool GetFileNodes(string rootPath,
        out CFileNode nodeRoot)
    {
        nodeRoot = new CFileNode();

        if (!Directory.Exists(rootPath))
            return false;

        nodeRoot.NodeType = CFileNode.ENodeType.Folder;
        nodeRoot.Path = rootPath;
        nodeRoot.Name = Path.GetFileName(rootPath) ?? "";

        GetFileNodesSub(nodeRoot);

        return true;
    }

    static bool GetFileNodesSub(CFileNode nodeParent)
    {
        try
        {
            string[] dirs = Directory.GetDirectories(nodeParent.Path);
            foreach (var dir in dirs)
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                if ((di.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;

                CFileNode nodeDir = new CFileNode();
                nodeDir.NodeType = CFileNode.ENodeType.Folder;
                nodeDir.Path = dir;
                nodeDir.Name = Path.GetFileName(dir);
                nodeParent.Children.Add(nodeDir);
                //Log(nodeDir.Path);

                GetFileNodesSub(nodeDir);
            }

            string[] files = Directory.GetFiles(nodeParent.Path);
            foreach (var file in files)
            {
                CFileNode nodeFile = new CFileNode();
                nodeFile.NodeType = CFileNode.ENodeType.File;
                nodeFile.Path = file;
                nodeFile.Name = Path.GetFileName(file);
                nodeParent.Children.Add(nodeFile);
                //Log(nodeFile.Path);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    static CLogger? _logger = null;
    static void Log(string msg, ELogLevel logLevel = ELogLevel.Info)
    {
        if (_logger == null)
            _logger = new CLogger("CFile");
        _logger.Log(msg, logLevel);
    }


}
