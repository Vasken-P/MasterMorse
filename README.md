# MasterMorse

MasterMorse aims to make learning Morse code accessible, with a translator and learning resources. The application is a Morse Code Translator: it translates text to Morse code and Morse code to text in real time, swaps directions, copies results, and plays Morse audio with play, pause and stop controls.

## Project structure

- [`MasterMorse`](MasterMorse/) contains the .NET 10 WPF application. [`MainWindow`](MasterMorse/MainWindow.xaml) is the view.
- [`PresenterLibrary`](PresenterLibrary/) contains [`IMainView`](PresenterLibrary/Views/IMainView.cs) and [`MainPresenter`](PresenterLibrary/MainPresenter.cs). The view implements the interface, and [`App`](MasterMorse/App.xaml.cs) connects it to the presenter at startup.
- [`ModelLibrary`](ModelLibrary/) is a class library for future domain models and logic. The presenter references it as a project, so it builds into a DLL with the solution; no models are implemented yet.

## Build and run

On Windows, open [`MasterMorse.sln`](MasterMorse.sln) in Visual Studio 2026 or later with the .NET desktop development workload installed. Set `MasterMorse` as the startup project if necessary, then press F5.

Alternatively, install the .NET 10 SDK on Windows and run:

```sh
dotnet build MasterMorse.sln
dotnet run --project MasterMorse/MasterMorse.csproj
```

The application opens an empty window titled "MasterMorse". On non-Windows systems, cross-build with `dotnet build MasterMorse.sln -p:EnableWindowsTargeting=true`; running WPF requires Windows.
