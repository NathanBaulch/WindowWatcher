using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowWatcher
{
    public class BayesianClassifier
    {
        private readonly IDictionary<string, Category> _categories = new Dictionary<string, Category>();
        private readonly IDictionary<string, BayesianClassification> _classifications = new Dictionary<string, BayesianClassification>();

        public void Teach(string text, string categoryName)
        {
            if (!_categories.TryGetValue(categoryName, out var category))
            {
                category = new Category();
                _categories.Add(categoryName, category);
            }

            category.Teach(ParseText(text));
            _classifications.Clear();
        }

        public void Unteach(string text, string categoryName)
        {
            if (_categories.TryGetValue(categoryName, out var category))
            {
                category.Unteach(ParseText(text));
                _classifications.Clear();
            }
        }

        public BayesianClassification Classify(string text)
        {
            if (!_classifications.TryGetValue(text, out var classification))
            {
                var words = ParseText(text);
                var totalWords = _categories.Values.Sum(category => category.TotalWords);
                var scores = _categories.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.Score(words) + Math.Log(pair.Value.TotalWords / (double) totalWords));
                var max = scores.Values.Max();
                scores = scores.ToDictionary(
                    pair => pair.Key,
                    pair => Math.Exp(pair.Value - max));
                var sum = scores.Values.Sum();
                scores = scores.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value / sum);

                string bestCategory = null;
                var bestScore = 0d;

                foreach (var score in scores)
                {
                    if (score.Value > bestScore)
                    {
                        bestCategory = score.Key;
                        bestScore = score.Value;
                    }
                    else if (score.Value == bestScore)
                    {
                        bestCategory = null;
                    }
                }

                classification = new BayesianClassification {Category = bestCategory, Certainty = bestScore};
                _classifications[text] = classification;
            }

            return classification;
        }

        private static IEnumerable<string> ParseText(IEnumerable<char> text)
        {
            IList<string> words = new List<string>();
            string word = null;

            foreach (var c in text)
            {
                if (char.IsLetterOrDigit(c))
                {
                    word += c;
                }
                else if (word != null)
                {
                    words.Add(word);
                    word = null;
                }
            }

            if (word != null)
            {
                words.Add(word);
            }

            return words.ToArray();
        }

        #region Nested type: Category

        private class Category
        {
            private readonly IDictionary<string, int> _wordCounts = new Dictionary<string, int>();

            public int TotalWords { get; private set; }

            public void Teach(IEnumerable<string> words)
            {
                foreach (var word in words)
                {
                    TeachWord(word);
                }
            }

            private void TeachWord(string word)
            {
                _wordCounts.TryGetValue(word, out var count);
                _wordCounts[word] = ++count;
                TotalWords++;
            }

            public void Unteach(IEnumerable<string> words)
            {
                foreach (var word in words)
                {
                    UnteachWord(word);
                }
            }

            private void UnteachWord(string word)
            {
                if (_wordCounts.TryGetValue(word, out var count) && count > 0)
                {
                    _wordCounts[word] = --count;
                    TotalWords--;
                }
            }

            public double Score(IEnumerable<string> words)
            {
                return words.Aggregate(0.0, ScoreWord);
            }

            private double ScoreWord(double score, string word)
            {
                var numerator = (_wordCounts.TryGetValue(word, out var wordCount) ? wordCount : 0.01);
                return score + Math.Log(numerator / TotalWords);
            }
        }

        #endregion
    }

    public class BayesianClassification
    {
        public string Category { get; set; }
        public double Certainty { get; set; }
    }
}