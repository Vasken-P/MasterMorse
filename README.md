# MorseMaster

MorseMaster aims to make learning Morse code accessible. It currently includes a beginner guidebook with lessons on dots and dashes, letter and number patterns, Morse timing, and a short practice question.

## Project structure

- [`MasterMorse`](MasterMorse/) contains the .NET 10 WPF application and [`GuidebookView`](MasterMorse/Views/GuidebookView.xaml), the guidebook's dedicated view.
- [`TitleView`](MasterMorse/Views/TitleView.xaml) and [`TitlePresenter`](PresenterLibrary/Title/TitlePresenter.cs) display the title screen and request navigation to the guidebook. [`App`](MasterMorse/App.xaml.cs) connects the views and presenters.
- [`PresenterLibrary/Guidebook`](PresenterLibrary/Guidebook/) contains [`IGuidebookView`](PresenterLibrary/Guidebook/IGuidebookView.cs) and [`GuidebookPresenter`](PresenterLibrary/Guidebook/GuidebookPresenter.cs). The presenter loads guidebook content and checks answers through the model; the view reports user actions through its interface.
- [`AlphabetReferenceView`](MasterMorse/Views/AlphabetReferenceView.xaml) and [`AlphabetReferencePresenter`](PresenterLibrary/AlphabetReference/AlphabetReferencePresenter.cs) display the shared-model alphabet and abbreviations reference. The page is not yet linked from navigation.
- [`ModelLibrary`](ModelLibrary/) is shared by features. It contains the guidebook lesson, example, and practice-question types, plus the content provider, and builds into `ModelLibrary.dll`.

Each feature has its own view and presenter over the shared model. Select **Get Started** on the title screen to open the guidebook, or **Back to title** to return. The guidebook displays its topics as cards on one scrollable page, followed by a text-answer practice question. Incorrect answers can be retried; a correct answer gives feedback and disables submission.

## Build and run

On Windows, open [`MasterMorse.sln`](MasterMorse.sln) in Visual Studio 2026 or later with the .NET desktop development workload installed. Set `MasterMorse` as the startup project if necessary, then press F5.

Alternatively, install the .NET 10 SDK on Windows and run:

```sh
dotnet build MasterMorse.sln
dotnet run --project MasterMorse/MasterMorse.csproj
```

On non-Windows systems, cross-build with `dotnet build MasterMorse.sln -p:EnableWindowsTargeting=true`; running WPF requires Windows.
