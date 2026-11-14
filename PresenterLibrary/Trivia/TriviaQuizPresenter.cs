using System.Text.Json;
using MasterMorse.Models.Trivia;

namespace MasterMorse.Presenters.Trivia;

public sealed class TriviaQuizPresenter
{
    private readonly ITriviaQuizView _view;
    private readonly ITriviaQuestionBank _questionBank;
    private readonly ITriviaGameFactory _gameFactory;
    private readonly Action _returnToTitle;
    private ITriviaGameSession? _game;

    public TriviaQuizPresenter(
        ITriviaQuizView view,
        ITriviaQuestionBank questionBank,
        ITriviaGameFactory gameFactory,
        Action returnToTitle)
    {
        _view = view;
        _questionBank = questionBank;
        _gameFactory = gameFactory;
        _returnToTitle = returnToTitle;
    }

    public bool Initialize()
    {
        _view.Title = "MorseMaster - Jeopardy Quiz";

        try
        {
            _game = _gameFactory.Create(_questionBank.LoadQuestions());
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException or JsonException or InvalidDataException)
        {
            _view.ShowQuestionBankError(GetQuestionBankError(exception));
            return false;
        }

        _view.ClueSelected += SelectClue;
        _view.AnswerSubmitted += SubmitAnswer;
        _view.BoardRequested += ShowBoard;
        _view.ReturnToTitleRequested += _returnToTitle;
        ShowBoard();
        return true;
    }

    private void SelectClue(string questionId)
    {
        var game = GetGame();
        var question = game.SelectQuestion(questionId);
        _view.DisplayClue(question, game.Score, game.AnsweredQuestionCount, game.TotalQuestionCount);
    }

    private void SubmitAnswer(string answer)
    {
        var game = GetGame();
        var result = game.SubmitAnswer(answer);

        if (result.IsComplete)
        {
            _view.DisplayResults(result.Score, result.Score / (double)game.TotalQuestionCount);
            return;
        }

        _view.DisplayAnswerResult(result);
    }

    private void ShowBoard()
    {
        var game = GetGame();
        _view.DisplayBoard(
            game.GetBoard(), game.Score, game.AnsweredQuestionCount, game.TotalQuestionCount);
    }

    private ITriviaGameSession GetGame() =>
        _game ?? throw new InvalidOperationException("The trivia game has not been initialized.");

    private static string GetQuestionBankError(Exception exception) =>
        exception switch
        {
            FileNotFoundException or DirectoryNotFoundException =>
                "The quiz question file could not be found. Ensure lib/questions.json is included with the application.",
            JsonException =>
                "The quiz question file is malformed. Correct lib/questions.json and try again.",
            InvalidDataException => exception.Message,
            UnauthorizedAccessException =>
                "The quiz question file cannot be accessed. Check its file permissions.",
            _ => "The quiz question file could not be loaded."
        };
}
