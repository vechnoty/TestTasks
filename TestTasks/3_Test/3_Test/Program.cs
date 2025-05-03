using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string projectDirectory = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

        string inputPath = Path.Combine(projectDirectory, "input.txt");
        string outputPath = Path.Combine(projectDirectory, "output.txt");
        string problemsPath = Path.Combine(projectDirectory, "problems.txt");

        string[] lines = File.ReadAllLines(inputPath);
        using var output = new StreamWriter(outputPath);
        using var problems = new StreamWriter(problemsPath);

        foreach (var line in lines)
        {
            try
            {
                var result = NormalizeLogLine(line);
                if (result != null)
                    output.WriteLine(result);
                else
                    problems.WriteLine(line);
            }
            catch
            {
                problems.WriteLine(line);
            }
        }

        Console.WriteLine("Обработка завершена.");
    }

    static string NormalizeLogLine(string line)
    {
        // Format 1: DD.MM.YYYY HH:MM:SS.mmm LEVEL Message
        var regex1 = new Regex(@"^(?<date>\d{2}\.\d{2}\.\d{4}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3}) (?<level>\w+) (?<message>.+)$");
        // Format 2: YYYY-MM-DD HH:MM:SS.mmm+| LEVEL|thread|method| message
        var regex2 = new Regex(@"^(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}\.\d+)\| (?<level>\w+)\|\d+\|(?<method>[^|]+)\| (?<message>.+)$");

        Match match;

        if ((match = regex1.Match(line)).Success)
        {
            var date = DateTime.ParseExact(match.Groups["date"].Value, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                               .ToString("yyyy-MM-dd");// проверить
            var time = match.Groups["time"].Value;
            var level = NormalizeLevel(match.Groups["level"].Value);
            var method = "DEFAULT";
            var message = match.Groups["message"].Value;

            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }
        else if ((match = regex2.Match(line)).Success)
        {
            var date = match.Groups["date"].Value;
            var time = match.Groups["time"].Value;
            var level = NormalizeLevel(match.Groups["level"].Value);
            var method = match.Groups["method"].Value;
            var message = match.Groups["message"].Value;

            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }

        return null; // invalid
    }

    static string NormalizeLevel(string input)
    {
        return input.ToUpper() switch
        {
            "INFORMATION" => "INFO",
            "INFO" => "INFO",
            "WARNING" => "WARN",
            "WARN" => "WARN",
            "ERROR" => "ERROR",
            "DEBUG" => "DEBUG",
            _ => "UNKNOWN"
        };
    }
}
