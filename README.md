# Windows Recovery App

An easy-to-use Windows application that automates the process of scanning for OS issues and making applicable repairs. Perfect for fixing common Windows 11 problems including Explorer crashes, system file corruption, and more.

## Features

### 🔧 Automated System Repair
- **Quick Scan & Fix**: Runs System File Checker (SFC) to quickly identify and repair corrupted system files
- **Full System Scan**: Comprehensive scan that runs:
  - DISM (Deployment Image Servicing and Management) to repair Windows image
  - SFC (System File Checker) to fix corrupted system files
  - Provides guidance for Check Disk (CHKDSK) if needed

### 🖱️ Explorer Fix
- **Fix Explorer Issues**: Specifically addresses the "right-click crashes Explorer" issue by:
  - Restarting Windows Explorer
  - Clearing icon cache
  - Refreshing shell extensions

### 🚀 Quick Access Tools
- **Microsoft Activation Scripts (MAS)**: One-click access to the popular open-source Windows/Office activation tool
- **Chris Titus Tech Windows Utility**: Quick access to comprehensive Windows tweaking and optimization tools

## Requirements

- Windows 10 or Windows 11
- .NET 8.0 Runtime (usually pre-installed on Windows 11)
- Administrator privileges (the app will request elevation)

## Installation

### Option 1: Download from GitHub Actions (Testing/Latest Build)
1. Go to the [Actions tab](https://github.com/Rskeehan/WindowsRecoveryApp/actions)
2. Click on the latest successful "Build and Test" workflow run
3. Download the `WindowsRecoveryApp-[commit-hash]` artifact
4. Extract the ZIP file
5. Right-click `WindowsRecoveryApp.exe` and select "Run as administrator"

**See [DOWNLOAD_AND_TEST.md](DOWNLOAD_AND_TEST.md) for detailed instructions on downloading and testing builds.**

### Option 2: Download Release (Recommended)
1. Download the latest release from the [Releases](https://github.com/Rskeehan/WindowsRecoveryApp/releases) page
2. Extract the ZIP file
3. Right-click `WindowsRecoveryApp.exe` and select "Run as administrator"

### Option 3: Build from Source
1. Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
2. Clone this repository:
   ```bash
   git clone https://github.com/Rskeehan/WindowsRecoveryApp.git
   cd WindowsRecoveryApp
   ```
3. Build the project:
   ```bash
   dotnet build WindowsRecoveryApp.sln -c Release
   ```
4. Run the application:
   ```bash
   cd WindowsRecoveryApp\bin\Release\net8.0-windows
   WindowsRecoveryApp.exe
   ```

## Usage

### Running a Quick Scan
1. Launch the application (right-click → Run as administrator)
2. Click "Quick Scan & Fix"
3. Wait for the scan to complete (usually 5-15 minutes)
4. Review the log for any issues found and repairs made

### Running a Full System Scan
1. Launch the application as administrator
2. Click "Full System Scan"
3. Confirm you want to proceed
4. Wait for the complete scan (can take 30-60 minutes)
5. Review the results and restart if prompted

### Fixing Explorer Issues
If you're experiencing issues with right-clicking or Explorer crashes:
1. Click "Fix Explorer Issues"
2. The app will restart Explorer and clear caches
3. Test right-clicking in File Explorer
4. If issues persist, run a Full System Scan

### Using Quick Access Tools

#### Microsoft Activation Scripts
1. Click "Microsoft Activation Scripts"
2. Confirm you want to launch the tool
3. A PowerShell window will open with the MAS tool
4. Follow the on-screen instructions

#### Chris Titus Tech Tools
1. Click "Chris Titus Tech Tools"
2. Confirm you want to launch the tool
3. A PowerShell window will open with the CTT utility
4. Use the GUI to apply tweaks and optimizations

## Common Issues Addressed

- ✅ Right-click crashes File Explorer
- ✅ System files corrupted after failed Windows update
- ✅ Windows upgrade interrupted mid-installation
- ✅ Explorer.exe freezing or crashing
- ✅ Context menu issues
- ✅ System instability after upgrade from Home to Pro
- ✅ Missing or corrupted system files

## Troubleshooting

### "A scan is already in progress"
Wait for the current operation to complete before starting a new one.

### "Application is not running with administrator privileges"
Right-click the application and select "Run as administrator" for full functionality.

### Scan takes a very long time
Full system scans can take 30-60 minutes depending on your system. This is normal for DISM and SFC operations.

### Issues persist after scanning
1. Restart your computer
2. Run the Full System Scan again
3. Check for Windows updates
4. Consider running `chkdsk C: /f /r` from an elevated command prompt (requires restart)

## How It Works

### System File Checker (SFC)
Scans all protected system files and replaces corrupted files with a cached copy located in a compressed folder at `%WinDir%\System32\dllcache`.

### DISM
Checks the Windows component store for corruption and repairs the Windows system image. Should be run before SFC for best results.

### Explorer Fix
Terminates and restarts the Windows Explorer process, clears icon cache, and resets shell extensions to resolve context menu and UI issues.

## Safety & Security

- All repair operations use Windows built-in tools (SFC, DISM)
- No third-party repair tools are downloaded or installed
- The app itself doesn't modify system files directly
- Quick access tools (MAS, CTT) are downloaded from their official sources
- Source code is open and available for review

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Microsoft Activation Scripts: https://massgrave.dev/
- Chris Titus Tech Windows Utility: https://christitus.com/

## Disclaimer

This tool is provided as-is. While it uses Windows built-in repair tools, always ensure you have backups of important data before performing system repairs. The author is not responsible for any data loss or system issues that may occur from using this tool.