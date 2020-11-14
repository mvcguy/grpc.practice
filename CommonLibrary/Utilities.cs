using System;

namespace CommonLibrary
{
    public static class Utilities
    {

        public static string RandomSentence()
        {
            var length = new Random(Guid.NewGuid().GetHashCode()).Next(7, 15);
            var str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                var wordLength = new Random(Guid.NewGuid().GetHashCode()).Next(2, 12);
                str += $"{RandomString(wordLength)} ";
            }
            return str;
        }

        public static string RandomString(int length = 5)
        {
            var str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str += (char)new Random(Guid.NewGuid().GetHashCode()).Next(97, 122);
            }
            return str;
        }

        public static string RandomDigits(int length = 8)
        {
            var str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str += (char)new Random(Guid.NewGuid().GetHashCode()).Next(48, 57);
            }
            return str;
        }
    }
}
