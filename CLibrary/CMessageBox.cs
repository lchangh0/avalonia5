
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;

namespace CLibray;

public enum EMessageBoxResult
{
    Ok,
    Yes,
    No,
    Abort,
    Cancel,
    None
}

public static class CMessageBox
{
    public static async Task<EMessageBoxResult> ShowAsync(Window owner,
        string strMsg, string strTitle)
    {
        var ret = await MessageBoxManager.GetMessageBoxStandard(
            strTitle,
            strMsg)
            .ShowWindowDialogAsync(owner);

        return (EMessageBoxResult)ret;
    }
}
