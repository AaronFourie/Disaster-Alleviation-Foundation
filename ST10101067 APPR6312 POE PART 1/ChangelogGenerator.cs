using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        // Run 'git log' to get the commit messages
        string gitLog = RunGitCommand("log --pretty=format:%s");

        var changelog = new Dictionary<string, List<string>>
        {
            { "Added", new List<string>() },
            { "Changed", new List<string>() },
            { "Fixed", new List<string>() },
            { "Updated", new List<string>() }
        };

        foreach (string line in gitLog.Split('\n'))
        {
            string trimmedLine = line.Trim();
            string[] parts = trimmedLine.Split();
            if (parts.Any())
            {
                string changeType = parts[^1];
                if (changeType.EndsWith(":"))
                {
                    changeType = changeType.Remove(changeType.Length - 1); // Remove the colon if present
                }
                string message = string.Join(" ", parts.Take(parts.Length - 1));

                // Use a case-insensitive match for the change type
                string matchType = changelog.Keys.FirstOrDefault(t => t.Equals(changeType, StringComparison.OrdinalIgnoreCase));
                if (matchType != null)
                {
                    changelog[matchType].Add(message);
                }
                else
                {
                    // If the change type is not recognized, consider it as "Added"
                    changelog["Added"].Add(trimmedLine);
                }
            }
        }

        // Define the changelog file name
        string changelogFile = "CHANGELOG.md";

        // Write the changelog to a file
        using (StreamWriter writer = new StreamWriter(changelogFile, false, Encoding.UTF8))
        {
            writer.WriteLine("## Changelog\n");
            foreach (var (category, changes) in changelog)
            {
                if (changes.Any())
                {
                    writer.WriteLine($"### {category}\n");
                    foreach (string change in changes)
                    {
                        writer.WriteLine($"- {change}");
                    }
                    writer.WriteLine();
                }
            }
        }

        Console.WriteLine($"Changelog has been written to {changelogFile}");
    }

    static string RunGitCommand(string arguments)
    {
        string gitExecutable = "git"; // Assuming "git" is in your system's PATH
        var processInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = gitExecutable,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Arguments = arguments
        };

        using (System.Diagnostics.Process process = new System.Diagnostics.Process { StartInfo = processInfo })
        {
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }
    }
}


