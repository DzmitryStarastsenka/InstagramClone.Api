using System;
using System.Linq;

namespace InstagramClone.UnitTests.Helpers;

public static class RandomHelper
{
    private const string LetterPattern = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static readonly Random Random = new();

    public static string GetRandomString(int length) =>
        new(Enumerable.Repeat(LetterPattern, length).Select(s => s[Random.Next(s.Length)]).ToArray());

    public static int GetRandomNumber(int firstNumber, int lastNumber) => Random.Next(firstNumber, lastNumber);
}