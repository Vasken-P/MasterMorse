# MasterMorse

MasterMorse aims to make learning Morse code accessible, with a translator and learning resources. The current project is a blank application shell; these features are not implemented yet.

## Project structure

- [`src/MasterMorse`](src/MasterMorse/) contains the .NET 10 WPF application.
- [`MainWindow`](src/MasterMorse/MainWindow.xaml) is the view and implements [`IMainView`](src/MasterMorse/Views/IMainView.cs).
- [`MainPresenter`](src/MasterMorse/Presenters/MainPresenter.cs) initializes the view through that interface. [`App`](src/MasterMorse/App.xaml.cs) connects them at startup.
- There is no model yet: add domain/application logic separately when real features require it, keeping it out of the WPF view and infrastructure.

## Build and run

Open [`MasterMorse.sln`](MasterMorse.sln) in Visual Studio 2026 or later with the .NET desktop development workload installed. The solution contains the WPF application in `src/MasterMorse`.

Alternatively, install the .NET 10 SDK on Windows and run:

```sh
dotnet build MasterMorse.sln
dotnet run --project src/MasterMorse/MasterMorse.csproj
```

The application opens an empty window titled "MasterMorse". On non-Windows systems, cross-build with `dotnet build src/MasterMorse/MasterMorse.csproj -p:EnableWindowsTargeting=true`; running WPF requires Windows.

## R-PIV for this scaffold

- **Research:** The repository initially contained only this README. The referenced `copilot-instructions.md` was not present, and there was no existing application or test infrastructure.
- **Plan:** Scaffold the WPF shell, connect a view interface and presenter, document the structure, and build and review the result.
- **Implement:** Changes are limited to `src/MasterMorse/`, `.gitignore`, and this README. No API, persistence, or Morse functionality is introduced.
- **Validate:** The cross-build succeeded and README links were checked against the file tree. There is no existing test suite. A Windows run is needed to verify the window visually.

Assumptions and risks: .NET 10 and Windows are required to run the application. Windows targeting packs may need to be restored for a cross-build; this does not verify runtime behavior on Windows.
