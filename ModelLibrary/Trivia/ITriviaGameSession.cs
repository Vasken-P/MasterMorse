namespace MasterMorse.Models.Trivia;

public interface ITriviaGameSession
{
    int Score { get; }
    int AnsweredQuestionCount { get; }
    int TotalQuestionCount { get; }
    bool IsComplete { get; }
    TriviaQuestion CurrentQuestion { get; }

    TriviaAnswerResult SubmitAnswer(int answerIndex);
    TriviaQuestion AdvanceToNextQuestion();
}
