using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string inputFilePath = "D:/1/1in.csv";
        string outputFilePathTemplate = "D:/1/1out.csv";
        string dateFormat = "dd/MM/yyyy";

        Func<string, DateTime> getDate = (line) =>
        {
            string[] parts = line.Split(',');
            return DateTime.ParseExact(parts[0], dateFormat, null);
        };

        Func<string, double> getAmount = (line) =>
        {
            string[] parts = line.Split(',');
            return double.Parse(parts[1]);
        };

        Action<DateTime, double> writeDailyTotal = (date, total) =>
        {
            string outputFilePath = string.Format(outputFilePathTemplate, date.ToString("yyyyMMdd"));
            using (var writer = new StreamWriter(outputFilePath, true))
            {
                writer.WriteLine("{0},{1}", date.ToString(dateFormat), total.ToString("0.00"));
            }
        };

        int batchSize = 10;
        int batchCount = 0;
        DateTime batchDate = DateTime.MinValue;
        double batchTotal = 0;

        using (var reader = new StreamReader(inputFilePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                DateTime date = getDate(line);
                double amount = getAmount(line);

                if (date != batchDate)
                {
                    if (batchCount > 0)
                    {
                        writeDailyTotal(batchDate, batchTotal);
                        batchCount = 0;
                        batchTotal = 0;
                    }
                    batchDate = date;
                }

                batchTotal += amount;
                batchCount++;

                if (batchCount == batchSize)
                {
                    writeDailyTotal(batchDate, batchTotal);
                    batchCount = 0;
                    batchTotal = 0;
                }
            }

            if (batchCount > 0)
            {
                writeDailyTotal(batchDate, batchTotal);
            }
        }
    }
}
