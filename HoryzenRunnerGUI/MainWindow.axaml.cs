using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace HoryzenRunnerGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        osPathBrowseBTN.Click += OsPathBrowseBTN_Click;
        runBTN.Click += RunBTN_Click;
    }

    private async void OsPathBrowseBTN_Click(object? sender, RoutedEventArgs e)
    {
        IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions());
        osPathRunner.Text = files.Count > 0 ? files[0].Path.LocalPath : string.Empty;
    }

    private async void RunBTN_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(osPathRunner.Text)) return;

        Process os = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = osPathRunner.Text,
                Arguments = osPathTB.Text,
                WorkingDirectory = Directory.GetParent(osPathRunner.Text)?.FullName ?? string.Empty,
                RedirectStandardOutput = true, // Redirect output
                RedirectStandardError = true, // Capture errors
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            }
        };

        os.OutputDataReceived += (sender, args) => AppendOutput(args.Data);
        os.ErrorDataReceived += (sender, args) => AppendOutput("[ERROR] " + args.Data);

        os.Start();
        os.BeginOutputReadLine();
        os.BeginErrorReadLine();

        await os.WaitForExitAsync();
    }

    private void AppendOutput(string? text)
    {
        if (text == null) return;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            outputTB.Text += text + "\n"; // Update TextBox in UI thread
        });
    }
}
