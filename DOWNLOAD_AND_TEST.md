# How to Download and Test Builds from GitHub Actions

This guide explains how to download and test the Windows Recovery App directly from GitHub Actions builds.

## Method 1: Download from Actions Tab (Recommended)

1. **Navigate to Actions**:
   - Go to the repository: https://github.com/Rskeehan/WindowsRecoveryApp
   - Click on the "Actions" tab at the top

2. **Find the Workflow Run**:
   - Click on "Build and Test" in the left sidebar
   - You'll see a list of workflow runs
   - Click on the most recent successful run (green checkmark ✓)

3. **Download the Artifact**:
   - Scroll down to the "Artifacts" section at the bottom of the page
   - Click on `WindowsRecoveryApp-[commit-hash]` to download the ZIP file
   - The artifact will be retained for 30 days

4. **Extract and Run**:
   - Extract the downloaded ZIP file to a folder
   - Right-click on `WindowsRecoveryApp.exe`
   - Select "Run as administrator"
   - The app will launch with full functionality

## Method 2: Manual Workflow Trigger

If you want to build a specific branch:

1. Go to the "Actions" tab
2. Click "Build and Test" in the left sidebar
3. Click the "Run workflow" button on the right
4. Select your branch
5. Click "Run workflow"
6. Wait for the build to complete
7. Download the artifact as described above

## Method 3: Automatic Builds

Builds are automatically triggered when:
- You push commits to any branch
- You open or update a pull request
- You manually trigger the workflow (see Method 2)

## What's Included in the Build

The artifact contains:
- `WindowsRecoveryApp.exe` - Main application (self-contained)
- All required .NET runtime files
- Application configuration and manifest files

**Size**: Approximately 140-170 MB (self-contained .NET 8.0 runtime included)

## System Requirements

- Windows 10 (1809 or later) or Windows 11
- Administrator privileges
- No additional runtime installation needed (self-contained)

## Testing Checklist

When testing the downloaded build:
- [ ] App launches without errors
- [ ] UAC prompt appears (requesting admin rights)
- [ ] Main window displays correctly
- [ ] Quick Scan button works
- [ ] Full Scan button works
- [ ] Fix Explorer button works (test with your right-click issue)
- [ ] MAS script button launches PowerShell
- [ ] CTT script button launches PowerShell
- [ ] Log window shows output in real-time

## Troubleshooting

### "Windows protected your PC" message
This is SmartScreen protection for unsigned executables:
1. Click "More info"
2. Click "Run anyway"
3. The app will run normally

### Download issues
- Make sure you're logged into GitHub
- Check that the workflow completed successfully (green checkmark)
- Artifacts are only available for 30 days
- You must be logged in to download artifacts from private repositories

### Build failed
- Check the workflow logs in the Actions tab
- Look for red X marks indicating failures
- Click on the failed step to see detailed error messages

## Feedback

After testing, please provide feedback on:
- Does the app launch successfully?
- Does the Explorer fix resolve your right-click crash issue?
- Are there any errors in the log?
- Does the UI look correct?
- Do all features work as expected?

## Next Steps

Once testing is successful, the app can be:
- Tagged as version 1.0
- Published as a GitHub Release
- Signed for easier distribution
- Published to Microsoft Store (optional)
