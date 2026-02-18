# Technical Documentation

## Architecture Overview

Windows Recovery App is a WPF (Windows Presentation Foundation) application built with C# and .NET 8.0. It provides a graphical interface for running Windows built-in repair tools and accessing popular third-party utilities.

## Project Structure

```
WindowsRecoveryApp/
├── WindowsRecoveryApp.sln          # Visual Studio solution file
├── README.md                        # User documentation
├── CONTRIBUTING.md                  # Contribution guidelines
├── LICENSE                          # MIT License
├── .gitignore                       # Git ignore rules
└── WindowsRecoveryApp/              # Main application project
    ├── WindowsRecoveryApp.csproj    # Project file
    ├── app.manifest                 # Application manifest (requests admin)
    ├── App.xaml                     # Application XAML
    ├── App.xaml.cs                  # Application code-behind
    ├── MainWindow.xaml              # Main window UI definition
    └── MainWindow.xaml.cs           # Main window logic
```

## Key Components

### MainWindow.xaml
Defines the user interface with:
- Title bar
- Automated Repair section with scan buttons
- Explorer fix button
- Operations log (TextBox with scroll)
- Quick Access Tools section

### MainWindow.xaml.cs
Contains the application logic:

#### Key Methods

**CheckAdminPrivileges()**
- Checks if the app is running with administrator privileges
- Logs a warning if not running as admin

**QuickScan_Click()**
- Runs System File Checker (SFC)
- Async operation to prevent UI freezing
- Disables buttons during scan

**FullScan_Click()**
- Runs DISM RestoreHealth
- Runs SFC scan
- Provides CHKDSK guidance
- Can take 30-60 minutes

**FixExplorer_Click()**
- Kills all explorer.exe processes
- Restarts Explorer
- Clears icon cache
- Fixes context menu issues

**MASScript_Click()**
- Launches PowerShell with Microsoft Activation Scripts
- Uses irm (Invoke-RestMethod) to download and execute

**ChrisTitus_Click()**
- Launches PowerShell with Chris Titus Tech utility
- Uses irm to download and execute

**RunCommand()**
- Helper method to execute system commands
- Redirects output to log
- Handles errors gracefully

**LogMessage()**
- Thread-safe logging to UI
- Uses Dispatcher.Invoke for cross-thread UI updates
- Timestamps all messages

## System Tools Used

### System File Checker (SFC)
```
sfc /scannow
```
- Scans all protected system files
- Replaces corrupted files with cached copies
- Located in Windows\System32\dllcache
- Requires administrator privileges

### DISM (Deployment Image Servicing and Management)
```
DISM /Online /Cleanup-Image /RestoreHealth
```
- Repairs Windows component store
- Downloads replacement files from Windows Update
- Should be run before SFC for best results
- Requires administrator privileges and internet connection

### Explorer Restart
```
taskkill /f /im explorer.exe
start explorer.exe
```
- Terminates Windows Explorer process
- Restarts it cleanly
- Clears in-memory cache
- Fixes many UI-related issues

### Icon Cache Clear
```
del /f /s /q /a "%LocalAppData%\IconCache.db"
del /f /s /q /a "%LocalAppData%\Microsoft\Windows\Explorer\iconcache*"
```
- Removes corrupted icon cache
- Forces Windows to rebuild icon cache
- Fixes icon display issues

## Security Considerations

### Administrator Privileges
The application requests administrator privileges through the manifest:
```xml
<requestedExecutionLevel level="requireAdministrator" uiAccess="false" />
```

This is required because:
- SFC and DISM require elevated privileges
- Killing/starting explorer.exe requires admin rights
- Clearing system caches requires admin rights

### Third-Party Scripts
The app provides quick access to:
- **MAS (Microsoft Activation Scripts)**: https://massgrave.dev/
- **Chris Titus Tech**: https://christitus.com/

These are downloaded and executed via PowerShell with:
```powershell
-ExecutionPolicy Bypass
```

Users are prompted before launching these tools.

## Error Handling

### Process Execution
- All process executions are wrapped in try-catch blocks
- Errors are logged to the UI
- User is notified via MessageBox
- Process exit codes are checked

### Threading
- Long-running operations use async/await
- UI updates use Dispatcher.Invoke
- Buttons are disabled during operations
- isScanning flag prevents concurrent operations

## UI/UX Design

### Color Scheme
- Background: #FF1E1E1E (dark gray)
- Accent: #FF0078D4 (Windows blue)
- Success: #FF107C10 (green)
- Warning: #FFE74856 (red)
- Text: White and LightGray

### User Flow
1. User launches app (elevated)
2. App checks admin privileges
3. User selects operation
4. Operation runs with live logging
5. User sees results in log
6. Operation completes with MessageBox

## Performance Considerations

### Async Operations
All system commands run asynchronously to prevent UI freezing:
```csharp
await Task.Run(() => RunCommand("sfc", "/scannow"));
```

### Process Output
Output is streamed line-by-line to prevent memory issues:
```csharp
process.OutputDataReceived += (sender, e) => { ... };
process.BeginOutputReadLine();
```

### Button States
Buttons are disabled during operations to prevent:
- Concurrent scans
- Resource conflicts
- User confusion

## Testing

### Manual Testing Checklist
- [ ] App launches without errors
- [ ] Admin privilege check works
- [ ] Quick scan completes successfully
- [ ] Full scan completes successfully
- [ ] Explorer fix restarts Explorer
- [ ] MAS script launches PowerShell
- [ ] CTT script launches PowerShell
- [ ] Log updates in real-time
- [ ] Buttons disable during operations
- [ ] Error messages display correctly

### Test Scenarios
1. **Normal Operation**: Run all scans on healthy system
2. **Corrupted Files**: Test on system with corrupted files
3. **No Admin**: Run without admin privileges
4. **Network Issues**: Test DISM without internet
5. **Concurrent**: Try to run multiple scans
6. **Cancel**: User closes app during scan

## Build and Deployment

### Build Configuration
```xml
<PropertyGroup>
  <OutputType>WinExe</OutputType>
  <TargetFramework>net8.0-windows</TargetFramework>
  <UseWPF>true</UseWPF>
  <EnableWindowsTargeting>true</EnableWindowsTargeting>
</PropertyGroup>
```

### Release Build
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

Creates a standalone executable with all dependencies.

### Distribution
Options:
1. **ZIP Release**: Package bin folder
2. **Installer**: Use WiX or Inno Setup
3. **Microsoft Store**: Package as MSIX
4. **Portable**: Single-file publish

## Future Enhancements

### Planned Features
- Progress bars for long operations
- Cancel button for active scans
- System restore point creation
- Registry repair utilities
- Scheduled scans
- Report generation (HTML/PDF)
- Notification system
- System tray integration

### Technical Improvements
- Unit tests
- Integration tests
- Logging framework (e.g., Serilog)
- Configuration file support
- Localization support
- Auto-update mechanism
- Telemetry (opt-in)

## Dependencies

### Runtime Dependencies
- .NET 8.0 Windows Desktop Runtime
- Windows 10 version 1809 or later
- PowerShell 5.1 or later (for quick access tools)

### Development Dependencies
- .NET 8.0 SDK
- Visual Studio 2022 or later
- Windows 10 SDK

## Troubleshooting Development Issues

### Build Errors
**"To build a project targeting Windows on this operating system"**
- Solution: Add `<EnableWindowsTargeting>true</EnableWindowsTargeting>`
- Note: This only affects building, not running

**"The target framework 'net8.0-windows' is out of support"**
- Update to latest .NET version
- Or suppress warning if intentional

### Runtime Errors
**"Access Denied"**
- Ensure app is running as administrator
- Check UAC settings

**"Process cannot be started"**
- Verify command exists (sfc, DISM)
- Check PATH environment variable

## Resources

- [WPF Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [SFC Reference](https://support.microsoft.com/en-us/windows/using-system-file-checker)
- [DISM Reference](https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism)
- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
