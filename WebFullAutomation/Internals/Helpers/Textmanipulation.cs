using System;
using System.Linq;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public static class TextManipulation
    {
        public static string RandomString()
        {
            var random = new Random();
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";

            var randLetters = Enumerable.Range(0, 4)
               .Select(x => letters[random.Next(0, letters.Length)]);

            var randNum = Enumerable.Range(randLetters.Count(), 3)
                .Select(x => numbers[random.Next(0, numbers.Length)]);

            var rand = randLetters.Concat(randNum);

            return new string(rand.ToArray()).ToLower();
        }

        public static string CreateGuid()
        {
            var guid = Guid.NewGuid().ToString("N").Substring(0, 32);

            return guid;
        }
    }
}
