# MorseMaster

MorseMaster aims to make learning Morse code accessible. It currently includes a beginner guidebook with lessons on dots and dashes, letter and number patterns, Morse timing, and a short practice question.

## Project structure

- [`MasterMorse`](MasterMorse/) contains the .NET 10 WPF application and [`GuidebookView`](MasterMorse/Views/GuidebookView.xaml), the guidebook's dedicated view.
- [`TitleView`](MasterMorse/Views/TitleView.xaml) and [`TitlePresenter`](PresenterLibrary/Title/TitlePresenter.cs) display the home screen and its guidebook action. [`AppNavigationCoordinator`](MasterMorse/Navigation/AppNavigationCoordinator.cs) handles navigation between the app's windows.
- The navigation bar opens the separate multiple-choice quiz. Its WPF view is [`TriviaQuizView`](MasterMorse/Views/TriviaQuizView.xaml), its presenter is in [`PresenterLibrary/Trivia`](PresenterLibrary/Trivia/), and its question bank is [`MasterMorse/lib/questions.json`](MasterMorse/lib/questions.json).
- [`PresenterLibrary/Guidebook`](PresenterLibrary/Guidebook/) contains [`IGuidebookView`](PresenterLibrary/Guidebook/IGuidebookView.cs) and [`GuidebookPresenter`](PresenterLibrary/Guidebook/GuidebookPresenter.cs). The presenter loads guidebook content and checks answers through the model; the view reports user actions through its interface.
- [`AlphabetReferenceView`](MasterMorse/Views/AlphabetReferenceView.xaml) and [`AlphabetReferencePresenter`](PresenterLibrary/AlphabetReference/AlphabetReferencePresenter.cs) display the shared-model alphabet and General/Medical abbreviations reference.
- [`ModelLibrary`](ModelLibrary/) is shared by features. It contains the guidebook lesson, example, and practice-question types, plus the content provider, and builds into `ModelLibrary.dll`.
- The translator and 1v1 duel implementation is retained in `MainWindow`, `DuelWindow`, `PresenterLibrary`, and `ModelLibrary/Duel`. These features are not yet connected to navigation.

Each feature has its own view and presenter over the shared model. A shared top navigation bar links Home, Guidebook, Quiz, and Morse Reference. The current feature is highlighted and disabled; switching between features hides rather than closes their windows so their state is retained. The home screen's **Get Started** action opens the guidebook. The guidebook displays its topics as cards on one scrollable page, followed by a text-answer practice question. Incorrect answers can be retried; a correct answer gives feedback and disables submission.

Select **Quiz** in the top navigation bar for a separate questionnaire with ten randomly selected multiple-choice questions per session. Correct answers add 10 points, incorrect answers subtract 2, and the final screen shows the score and average.

## Build and run

On Windows, open [`MasterMorse.sln`](MasterMorse.sln) in Visual Studio 2026 or later with the .NET desktop development workload installed. Set `MasterMorse` as the startup project if necessary, then press F5.

Alternatively, install the .NET 10 SDK on Windows and run:

```sh
dotnet build MasterMorse.sln
dotnet run --project MasterMorse/MasterMorse.csproj
```

On non-Windows systems, cross-build with `dotnet build MasterMorse.sln -p:EnableWindowsTargeting=true`; running WPF requires Windows.
