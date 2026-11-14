# MasterMorse

MasterMorse aims to make learning Morse code accessible. It currently includes a beginner guidebook with lessons on dots and dashes, letter and number patterns, Morse timing, and short practice questions.

## Project structure

- [`MasterMorse`](MasterMorse/) contains the .NET 10 WPF application and [`GuidebookView`](MasterMorse/Views/GuidebookView.xaml), the guidebook's dedicated view.
- [`PresenterLibrary/Guidebook`](PresenterLibrary/Guidebook/) contains [`IGuidebookView`](PresenterLibrary/Guidebook/IGuidebookView.cs) and [`GuidebookPresenter`](PresenterLibrary/Guidebook/GuidebookPresenter.cs). The presenter selects lessons, handles navigation, and checks answers through the model; the view reports user actions through its interface.
- [`ModelLibrary`](ModelLibrary/) is shared by features. It contains the guidebook lesson, example, and practice-question types, plus the content provider, and builds into `ModelLibrary.dll`.

Each feature has its own view and presenter over the shared model. The guidebook view advances one lesson at a time. Each lesson presents an explanation and examples, then offers one answer-by-text practice question. Incorrect answers can be retried; a correct answer gives feedback and disables submission until the user moves to another lesson.

## Build and run

On Windows, open [`MasterMorse.sln`](MasterMorse.sln) in Visual Studio 2026 or later with the .NET desktop development workload installed. Set `MasterMorse` as the startup project if necessary, then press F5.

Alternatively, install the .NET 10 SDK on Windows and run:

```sh
dotnet build MasterMorse.sln
dotnet run --project MasterMorse/MasterMorse.csproj
```

On non-Windows systems, cross-build with `dotnet build MasterMorse.sln -p:EnableWindowsTargeting=true`; running WPF requires Windows.
