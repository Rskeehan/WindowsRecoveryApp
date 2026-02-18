using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Principal;

namespace WindowsRecoveryApp
{
    public partial class MainWindow : Window
    {
        private bool isScanning = false;

        public MainWindow()
        {
            InitializeComponent();
            CheckAdminPrivileges();
            LogMessage("Windows Recovery App started. Ready to scan and repair.");
        }

        private void CheckAdminPrivileges()
        {
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                LogMessage("⚠️ WARNING: Application is not running with administrator privileges.");
                LogMessage("Some repair functions may require administrator rights.");
                LogMessage("Right-click the app and select 'Run as Administrator' for full functionality.\n");
            }
            else
            {
                LogMessage("✓ Running with administrator privileges.\n");
            }
        }

        private void LogMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                txtLog.ScrollToEnd();
            });
        }

        private async void QuickScan_Click(object sender, RoutedEventArgs e)
        {
            if (isScanning)
            {
                MessageBox.Show("A scan is already in progress. Please wait.", "Busy", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            isScanning = true;
            btnQuickScan.IsEnabled = false;
            btnFullScan.IsEnabled = false;

            try
            {
                LogMessage("\n═══ Starting Quick Scan ═══");
                LogMessage("Running System File Checker (SFC)...");
                
                await Task.Run(() => RunCommand("sfc", "/scannow"));
                
                LogMessage("✓ Quick scan completed.");
                MessageBox.Show("Quick scan completed. Check the log for details.", "Complete", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error during quick scan: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isScanning = false;
                btnQuickScan.IsEnabled = true;
                btnFullScan.IsEnabled = true;
            }
        }

        private async void FullScan_Click(object sender, RoutedEventArgs e)
        {
            if (isScanning)
            {
                MessageBox.Show("A scan is already in progress. Please wait.", "Busy", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                "Full system scan will run multiple repair tools and may take 30-60 minutes.\n\n" +
                "The scan will run:\n" +
                "1. DISM (Deployment Image Servicing)\n" +
                "2. System File Checker (SFC)\n" +
                "3. Check Disk (CHKDSK) - if needed\n\n" +
                "Continue?",
                "Confirm Full Scan",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            isScanning = true;
            btnQuickScan.IsEnabled = false;
            btnFullScan.IsEnabled = false;

            try
            {
                LogMessage("\n═══ Starting Full System Scan ═══");
                
                // Step 1: DISM RestoreHealth
                LogMessage("\n[1/3] Running DISM to check and repair Windows image...");
                LogMessage("This may take 10-20 minutes...");
                await Task.Run(() => RunCommand("DISM", "/Online /Cleanup-Image /RestoreHealth"));
                
                // Step 2: SFC scan
                LogMessage("\n[2/3] Running System File Checker (SFC)...");
                LogMessage("This may take 10-20 minutes...");
                await Task.Run(() => RunCommand("sfc", "/scannow"));
                
                // Step 3: Check disk info
                LogMessage("\n[3/3] Checking disk status...");
                LogMessage("To schedule a full disk check, run CHKDSK manually at next reboot.");
                LogMessage("Command: chkdsk C: /f /r");
                
                LogMessage("\n✓ Full system scan completed.");
                MessageBox.Show(
                    "Full system scan completed!\n\n" +
                    "Check the log for details. If issues persist:\n" +
                    "1. Restart your computer\n" +
                    "2. Run the scan again\n" +
                    "3. Consider running CHKDSK at next boot",
                    "Scan Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error during full scan: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isScanning = false;
                btnQuickScan.IsEnabled = true;
                btnFullScan.IsEnabled = true;
            }
        }

        private async void FixExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogMessage("\n═══ Fixing Explorer Issues ═══");
                
                // Restart Explorer
                LogMessage("Restarting Windows Explorer...");
                await Task.Run(() =>
                {
                    // Kill explorer process
                    foreach (var process in Process.GetProcessesByName("explorer"))
                    {
                        try
                        {
                            process.Kill();
                            process.WaitForExit();
                        }
                        catch { }
                    }
                    
                    // Wait a moment
                    System.Threading.Thread.Sleep(1000);
                    
                    // Start explorer again
                    Process.Start("explorer.exe");
                });
                
                LogMessage("✓ Explorer restarted successfully.");
                
                // Clear icon cache
                LogMessage("Clearing icon cache...");
                await Task.Run(() =>
                {
                    try
                    {
                        string iconcachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        RunCommand("cmd", $"/c del /f /s /q /a \"{iconcachePath}\\IconCache.db\"");
                        RunCommand("cmd", $"/c del /f /s /q /a \"{iconcachePath}\\Microsoft\\Windows\\Explorer\\iconcache*\"");
                    }
                    catch { }
                });
                
                LogMessage("✓ Icon cache cleared.");
                LogMessage("✓ Explorer fix completed. Right-click should work now.");
                
                MessageBox.Show(
                    "Explorer has been restarted and cache cleared.\n\n" +
                    "If right-click still doesn't work:\n" +
                    "1. Try restarting your computer\n" +
                    "2. Run a full system scan\n" +
                    "3. Check for Windows updates",
                    "Fix Complete",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error fixing explorer: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MASScript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogMessage("\nLaunching Microsoft Activation Scripts (MAS)...");
                
                var result = MessageBox.Show(
                    "This will open PowerShell and download Microsoft Activation Scripts.\n\n" +
                    "MAS is an open-source tool for Windows/Office activation.\n\n" +
                    "Continue?",
                    "Launch MAS",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string command = "irm https://massgrave.dev/get | iex";
                    
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    };
                    
                    Process.Start(startInfo);
                    LogMessage("✓ MAS PowerShell window opened.");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error launching MAS: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}\n\nYou may need to run the app as administrator.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChrisTitus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogMessage("\nLaunching Chris Titus Tech Windows Utility...");
                
                var result = MessageBox.Show(
                    "This will open PowerShell and download Chris Titus Tech Windows Utility.\n\n" +
                    "This tool provides various Windows tweaks and optimizations.\n\n" +
                    "Continue?",
                    "Launch CTT",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string command = "irm christitus.com/win | iex";
                    
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                        UseShellExecute = true,
                        Verb = "runas"
                    };
                    
                    Process.Start(startInfo);
                    LogMessage("✓ CTT PowerShell window opened.");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error launching CTT: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}\n\nYou may need to run the app as administrator.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RunCommand(string command, string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                LogMessage(e.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                LogMessage($"ERROR: {e.Data}");
                            }
                        };

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            LogMessage($"⚠️ Process exited with code: {process.ExitCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error running command: {ex.Message}");
                throw;
            }
        }
    }
}
