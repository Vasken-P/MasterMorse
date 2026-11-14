namespace MasterMorse.Models;

public sealed class GuidebookContentProvider : IGuidebookContentProvider
{
    private static readonly IReadOnlyList<GuidebookLesson> Lessons = Array.AsReadOnly(
    [
        new GuidebookLesson(
            "Dots and dashes",
            "Morse code represents each character with a pattern of short dots and long dashes. A dot lasts one time unit; a dash lasts three. Each letter has its own pattern.",
            Array.AsReadOnly(
            [
                new MorseExample('E', ".", "one dot"),
                new MorseExample('T', "-", "one dash"),
                new MorseExample('A', ".-", "dot, then dash")
            ]),
            new MorsePracticeQuestion(
                "Which character is represented by a single dot (.)?",
                "E",
                "Correct. E is the simplest Morse pattern: one dot.",
                "Not quite. A single dot represents E.")),
        new GuidebookLesson(
            "Build patterns into letters",
            "Letters combine dots and dashes in a particular order. Read each signal from left to right; changing the order changes the character.",
            Array.AsReadOnly(
            [
                new MorseExample('A', ".-", "dot, then dash"),
                new MorseExample('N', "-.", "dash, then dot"),
                new MorseExample('S', "...", "three dots"),
                new MorseExample('K', "-.-", "dash, dot, dash")
            ]),
            new MorsePracticeQuestion(
                "Which character is represented by dash, dot, dash (-.-)?",
                "K",
                "Correct. Dash, dot, dash is K.",
                "Not quite. Dash, dot, dash is K.")),
        new GuidebookLesson(
            "Numbers use the same signals",
            "Numbers are also made from dots and dashes. Standard Morse digits each have five signals, so learn their complete pattern just like a letter.",
            Array.AsReadOnly(
            [
                new MorseExample('1', ".----", "dot, then four dashes"),
                new MorseExample('5', ".....", "five dots"),
                new MorseExample('0', "-----", "five dashes")
            ]),
            new MorsePracticeQuestion(
                "Which number is represented by five dots (.....)?",
                "5",
                "Correct. Five dots represent the number 5.",
                "Not quite. Five dots represent the number 5.")),
        new GuidebookLesson(
            "Pauses keep a message readable",
            "Timing separates the signals: leave one time unit between dots and dashes in the same character, three units between characters, and seven units between words. The pauses are silent; they are not extra dots or dashes.",
            Array.AsReadOnly(
            [
                new MorseExample('E', ".", "one dot"),
                new MorseExample('T', "-", "one dash")
            ]),
            new MorsePracticeQuestion(
                "How many time units of silence separate two characters?",
                "3",
                "Correct. A three-unit pause marks the space between characters.",
                "Not quite. A three-unit pause separates characters; seven units separate words."))
    ]);

    public IReadOnlyList<GuidebookLesson> GetLessons() => Lessons;
}
