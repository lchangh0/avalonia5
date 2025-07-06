using System;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Main;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        this.Opened += AbounwWindow_Opened;
    }

    void AbounwWindow_Opened(object? sender, EventArgs e)
    { 
        // 버전 정보 가져오기
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
        labelVersion.Text = $"Version {version}";
    }

    void ok_Click(object sender, RoutedEventArgs args)
    {
        Close();
    }

}