# Contributing to Windows Recovery App

Thank you for considering contributing to Windows Recovery App! This document provides guidelines for contributing to the project.

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:
- A clear, descriptive title
- Steps to reproduce the issue
- Expected behavior
- Actual behavior
- Your Windows version
- Screenshots if applicable

### Suggesting Enhancements

Enhancement suggestions are welcome! Please create an issue with:
- A clear, descriptive title
- Detailed description of the suggested enhancement
- Why this enhancement would be useful
- Any examples or mockups if applicable

### Pull Requests

1. Fork the repository
2. Create a new branch for your feature (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Test your changes thoroughly on Windows
5. Commit your changes (`git commit -m 'Add some amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## Development Setup

### Prerequisites

- Windows 10 or Windows 11
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Visual Studio 2022 (recommended) or Visual Studio Code
- Git

### Building the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/Rskeehan/WindowsRecoveryApp.git
   cd WindowsRecoveryApp
   ```

2. Open the solution:
   ```bash
   WindowsRecoveryApp.sln
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run --project WindowsRecoveryApp
   ```

### Code Style

- Follow standard C# coding conventions
- Use meaningful variable and method names
- Add comments for complex logic
- Keep methods focused and concise
- Handle exceptions appropriately

### Testing

Before submitting a PR:
- Test on a clean Windows installation if possible
- Test with and without administrator privileges
- Verify all buttons and features work correctly
- Check the log output is clear and helpful

## Areas for Contribution

Some areas where contributions would be especially welcome:

### Features
- Additional repair tools and diagnostics
- Support for more Windows versions
- Scheduled scans
- Report generation
- System restore point creation
- Registry repair utilities

### UI/UX
- Dark/light theme toggle
- Localization/internationalization
- Better progress indicators
- System tray integration
- Notification system

### Documentation
- Video tutorials
- More detailed troubleshooting guides
- FAQ section
- Translations

### Testing
- Unit tests
- Integration tests
- Test automation

## Code of Conduct

### Our Standards

- Be respectful and inclusive
- Welcome newcomers
- Accept constructive criticism gracefully
- Focus on what is best for the community
- Show empathy towards other community members

### Our Responsibilities

Project maintainers are responsible for clarifying the standards of acceptable behavior and are expected to take appropriate and fair corrective action in response to any instances of unacceptable behavior.

## Questions?

If you have questions, feel free to:
- Open an issue with the "question" label
- Contact the maintainers
- Check existing issues and discussions

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
