namespace MasterMorse.Models;

public sealed class GuidebookContentProvider : IGuidebookContentProvider
{
    private static readonly IReadOnlyList<GuidebookLesson> Lessons = Array.AsReadOnly(
    [
        new GuidebookLesson(
            "What is Morse code?",
            "Morse code represents letters and numbers with patterns of short and long signals. You can listen to, see, or feel those signals.",
            Array.Empty<MorseExample>()),
        new GuidebookLesson(
            "Dots and dashes",
            "A dot (·) is a short signal. A dash (–) is a long signal. Together, they form a character's pattern.",
            Array.Empty<MorseExample>()),
        new GuidebookLesson(
            "Letters and numbers",
            "Each letter and number has its own pattern. Read the signals in order, from left to right.",
            Array.AsReadOnly(
            [
                new MorseExample('E', "·", "one dot"),
                new MorseExample('T', "–", "one dash"),
                new MorseExample('A', "·–", "dot, then dash"),
                new MorseExample('5', "·····", "five dots")
            ])),
        new GuidebookLesson(
            "Signal timing",
            "Use the dot as one time unit. A dash lasts three time units: three times as long as a dot.",
            Array.Empty<MorseExample>()),
        new GuidebookLesson(
            "Pauses and spacing",
            "Silence separates the signals: pause for one time unit within a letter, three units between letters, and seven units between words.",
            Array.Empty<MorseExample>()),
        new GuidebookLesson(
            "Reading a message",
            "Read ···   –––   ··· as three groups: ··· is S, ––– is O, and ··· is S. Together they spell SOS.",
            Array.Empty<MorseExample>()),
        new GuidebookLesson(
            "Sending a message",
            "Send Morse code with sound, light, or taps. Keep the short and long signals and the pauses between them clear.",
            Array.Empty<MorseExample>())
    ]);

    private static readonly MorsePracticeQuestion PracticeQuestion = new(
        "Which letter is represented by ·– (dot, then dash)?",
        "A",
        "Correct. ·– represents A.",
        "Not quite. Look at the letters and numbers box, then try again.");

    public IReadOnlyList<GuidebookLesson> GetLessons() => Lessons;

    public MorsePracticeQuestion GetPracticeQuestion() => PracticeQuestion;
}
