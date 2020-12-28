using System;
using System.Text;

#pragma warning disable S1854

namespace LanguageGame
{
    public static class Translator
    {
        /// <summary>
        /// Translates from English to Pig Latin. Pig Latin obeys a few simple following rules:
        /// - if word starts with vowel sounds, the vowel is left alone, and most commonly 'yay' is added to the end;
        /// - if word starts with consonant sounds or consonant clusters, all letters before the initial vowel are
        ///   placed at the end of the word sequence. Then, "ay" is added.
        /// Note: If a word begins with a capital letter, then its translation also begins with a capital letter,
        /// if it starts with a lowercase letter, then its translation will also begin with a lowercase letter.
        /// </summary>
        /// <param name="phrase">Source phrase.</param>
        /// <returns>Phrase in Pig Latin.</returns>
        /// <exception cref="ArgumentException">Thrown if phrase is null or empty.</exception>
        /// <example>
        /// "apple" -> "appleyay"
        /// "Eat" -> "Eatyay"
        /// "explain" -> "explainyay"
        /// "Smile" -> "Ilesmay"
        /// "Glove" -> "Oveglay".
        /// </example>
        public static string TranslateToPigLatin(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                throw new ArgumentException("Source string cannot be null or empty or whitespace.");
            }

            var vowels = new char[] { 'a', 'o', 'i', 'u', 'e' };
            var consonants = GetConsonants(vowels);
            var punctuation = new char[] { ' ', '-', ',', '.', '!', '?' };
            var yay = new char[] { 'y', 'a', 'y' };
            var ay = new char[] { 'a', 'y' };
            var result = new StringBuilder(phrase.Length * 2);
            var generalIndex = 0;
            while (generalIndex < phrase.Length)
            {
                if (IsVowel(phrase[generalIndex], vowels))
                {
                    var word = GetWord(generalIndex, phrase, punctuation);
                    result.Append(word);
                    result.Append(yay);
                    generalIndex += word.Length;
                }
                else if (IsConsonant(phrase[generalIndex], consonants))
                {
                    var word = GetWord(generalIndex, phrase, punctuation);
                    Transform(word, consonants);
                    result.Append(word);
                    result.Append(ay);
                    generalIndex += word.Length;
                }
                else
                {
                    result.Append(phrase[generalIndex]);
                    generalIndex++;
                }
            }

            return result.ToString();
        }

        private static char[] GetWord(int idx, string source, char[] punctuation)
        {
            int wordLength;
            if (source.IndexOfAny(punctuation, idx) > -1)
            {
                wordLength = source.IndexOfAny(punctuation, idx);
            }
            else
            {
                wordLength = source.Length;
            }

            return source[idx..wordLength].ToCharArray();
        }

        private static void Transform(char[] word, char[] consonants)
        {
            bool upperFlag = char.IsUpper(word[0]);
            word[0] = char.ToLower(word[0], System.Globalization.CultureInfo.InvariantCulture);
            for (int j = 0; j < word.Length; j++)
            {
                if (IsConsonant(word[0], consonants))
                {
                    var tmp = word[0];
                    for (int i = 0; i < word.Length - 1; i++)
                    {
                        word[i] = word[i + 1];
                    }

                    word[^1] = tmp;
                }
            }

            if (upperFlag)
            {
                word[0] = char.ToUpper(word[0], System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private static char[] GetConsonants(char[] vowels)
        {
            var letters = new char['z' - 'a' + 1];
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i] = (char)((int)'a' + i);
            }

            var consonants = new char[letters.Length - 5];
            var j = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                if (!IsVowel(letters[i], vowels))
                {
                    consonants[j] = letters[i];
                    j++;
                }
            }

            return consonants;
        }

        private static bool IsVowel(char ch, char[] vowels)
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            for (int i = 0; i < vowels.Length; i++)
            {
                if (char.ToLower(ch, culture) == vowels[i])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConsonant(char ch, char[] consonants)
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            for (int i = 0; i < consonants.Length; i++)
            {
                if (char.ToLower(ch, culture) == consonants[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
