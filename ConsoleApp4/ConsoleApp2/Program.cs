using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = $@"C:/2"; //шлях до папки з текстовими файлами

            Func<string, IEnumerable<string>> tokenizer = Tokenize;
            Func<IEnumerable<string>, IDictionary<string, int>> calculator = CalculateFrequency;
            Action<IDictionary<string, int>> printer = PrintReport;

            IDictionary<string, int> report = GenerateReport(path, tokenizer, calculator);
            printer(report);

            Console.ReadLine();
        }

        static IDictionary<string, int> GenerateReport(string path, Func<string, IEnumerable<string>> tokenizer, Func<IEnumerable<string>, IDictionary<string, int>> CalculateFrequency)
        {
            IEnumerable<string> fileNames = Directory.EnumerateFiles(path);
            IDictionary<string, int> wordFrequencies = new Dictionary<string, int>();

            foreach (string filePath in fileNames)
            {
                string fileContent = File.ReadAllText(filePath);

                IEnumerable<string> tokens = tokenizer(fileContent);
                IDictionary<string, int> frequencies = CalculateFrequency(tokens);

                foreach (var pair in frequencies)
                {
                    string word = pair.Key;
                    int frequency = pair.Value;

                    if (!wordFrequencies.ContainsKey(word))
                    {
                        wordFrequencies[word] = 0;
                    }

                    wordFrequencies[word] += frequency;
                }
            }

            return wordFrequencies;
        }

        static IEnumerable<string> Tokenize(string text)
        {
            string[] words = text.Split(new char[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Select(word => word.ToLower());
        }

        static IDictionary<string, int> CalculateFrequency(IEnumerable<string> tokens)
        {
            IDictionary<string, int> frequencies = new Dictionary<string, int>();

            foreach (string token in tokens)
            {
                if (!frequencies.ContainsKey(token))
                {
                    frequencies.Add(token, 1);
                }
                else
                {
                    frequencies[token]++;
                }
            }
            return frequencies;
        }
        static void PrintReport(IDictionary<string, int> frequencies)
        {
            Console.WriteLine("Created report:");

            foreach (KeyValuePair<string, int> entry in frequencies.OrderByDescending(e => e.Value))
            {
                Console.WriteLine("{0}: {1}", entry.Key, entry.Value);
            }
            Console.WriteLine();
        }
    }
}