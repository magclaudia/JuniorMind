using GitClient;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public class LibGit2RepositoryHunks
{
    private LibGit2RepositoryChanges libGit2RepositoryChanges;
    private LibGit2RepositoryDiff libGit2RepositoryDiff;
    private PanelCommunicationService panelCommunicationService;
    private GetProjectPath path;
    private int hunkIndex;

    public LibGit2RepositoryHunks(LibGit2RepositoryChanges libGit2RepositoryChanges, LibGit2RepositoryDiff libGit2RepositoryDiff, PanelCommunicationService panelCommunicationService)
    {
        this.libGit2RepositoryChanges = libGit2RepositoryChanges;
        this.libGit2RepositoryDiff = libGit2RepositoryDiff;
        this.panelCommunicationService = panelCommunicationService;
        path = new GetProjectPath();
        hunkIndex = 0;
    }

    class Hunk
    {
        public List<string> FileHeaders { get; } = new List<string>();
        public string HunkHeader { get; set; }
        public List<HunkLine> Lines { get; } = new List<HunkLine>();
    }

    class HunkLine
    {
        public string Content { get; set; }
        public LineType Type { get; set; }
        public int? OldLineNumber { get; set; }
        public int? NewLineNumber { get; set; }
    }

    enum LineType { Context, Addition, Removal, Header }

    public void StageHunk(int hunkIndex, string line)
    {
        List<Hunk> hunks = ParseGitDiff();

        if (hunkIndex < 0 || hunkIndex >= hunks.Count)
        {
            return;
        }

        HandleLineSelection(hunks[hunkIndex], line);
    }

    private void HandleLineSelection(Hunk hunk, string line)
    {
        List<string> diff = RunGitCommand($"diff -- {panelCommunicationService.GetFilePath()}").Split("\n").ToList().Skip(4).ToList();
        bool success = false;

        if (line.StartsWith("@@"))
        {
            success = StageGitHunk(hunk);
        }
        else if (line.StartsWith('+') || line.StartsWith('-'))
        {
            int indexForHunk = hunk.Lines.FindIndex(x => x.Content == line);

            if (indexForHunk < 0)
            {
                return;
            }

            success = StageSingleLine(hunk, hunk.Lines[indexForHunk]);
        }
        else
        {
            return;
        }
    }

    private string BuildFullPatch(List<string> headers, string hunkHeader, List<HunkLine> lines)
    {
        return string.Join("\n", headers) + "\n" +
               hunkHeader + "\n" +
               string.Join("\n", lines.Select(l => l.Content)) + "\n";
    }

    private bool StageSingleLine(Hunk hunk, HunkLine line)
    {
        var patch = string.Join("\n", hunk.FileHeaders) + "\n" +
                   CreateSingleLinePatch(hunk, line);

        return ApplyPatch(patch);
    }

    private string CreateSingleLinePatch(Hunk hunk, HunkLine line)
    {
        var contextLines = hunk.Lines
            .Where(l => l.Type == LineType.Context)
            .Take(3)
            .ToList();

        return $"{hunk.HunkHeader}\n" +
               $"{line.Content}";
    }

    private (string header, List<HunkLine> lines) CreateHunkHeaderWithContext(List<HunkLine> contextLines, Hunk header)
    {
        var oldLines = contextLines.Where(l => l.OldLineNumber.HasValue).ToList();
        var newLines = contextLines.Where(l => l.NewLineNumber.HasValue).ToList();
        int oldStart = oldLines.FirstOrDefault()?.OldLineNumber ?? 0;
        int newStart = newLines.FirstOrDefault()?.NewLineNumber ?? 0;
        int oldCount = oldLines.Count(l => l.Type != LineType.Addition);
        int newCount = newLines.Count(l => l.Type != LineType.Removal);
        oldCount = oldCount == 0 ? 1 : oldCount;
        newCount = newCount == 0 ? 1 : newCount;

        int startFrom = $"@@ -{oldStart},{oldCount} +{newStart},{newCount} @@ ".Length;
        var headerLimits = "";

        if (header.HunkHeader.Length > startFrom)
        {
            string text = header.HunkHeader.Substring(startFrom);
            headerLimits = $"@@ -{oldStart},{oldCount} +{newStart},{newCount} @@ {text}";
        }
        else
        {
            headerLimits = $"@@ -{oldStart},{oldCount} +{newStart},{newCount} @@";
        }

        return (headerLimits, contextLines);
    }

    private bool StageGitHunk(Hunk hunkAll)
    {
        var patch = BuildFullPatch(
            hunkAll.FileHeaders,
            hunkAll.HunkHeader,
            hunkAll.Lines.Where(l => l.Type != LineType.Header).ToList());
        return ApplyPatch(patch);
    }

    //private bool StageSingleLine(Hunk hunk, HunkLine line)
    //{
    //    var contextLines = GetContextAroundLine(
    //        hunk.Lines, 
    //        line,      
    //        context: 2);

    //    contextLines = contextLines.Where(l => l.Type != LineType.Header).ToList();
    //    var (header, adjustedLines) = CreateHunkHeaderWithContext(contextLines, hunk);
    //    var patch = BuildFullPatch(hunk.FileHeaders, header, adjustedLines);
    //    return ApplyPatch(patch);
    //}

    private List<HunkLine> GetContextAroundLine(List<HunkLine> lines, HunkLine target, int context = 1)
    {
        int index = lines.FindIndex(l => l.Content == target.Content);
        if (index == -1) return new List<HunkLine>();
        int start = Math.Max(0, index - context);
        int end = Math.Min(lines.Count - 1, index + context);
        return lines.GetRange(start, end - start + 1);
    }

    private bool ApplyPatch(string patch)
    {
        var tempFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(tempFile, patch);
            var result = RunGitCommand($"apply --cached --verbose \"{tempFile}\"");
            return string.IsNullOrEmpty(result);
        }
        catch
        {
            return false;
        }
        finally
        {
            File.Delete(tempFile); 
        }
    }

    private List<Hunk> ParseGitDiff()
    {
        var diff = RunGitCommand($"diff -- {panelCommunicationService.GetFilePath()}");
        return ParseDiffHunks(diff);
    }

    private List<Hunk> ParseDiffHunks(string diff)
    {
        var hunks = new List<Hunk>();
        var lines = diff.Split('\n');
        List<string> currentFileHeaders = new List<string>();
        Hunk currentHunk = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("diff --git"))
            {
                currentFileHeaders = new List<string> { line };
                currentHunk = null;
            }
            else if (line.StartsWith("--- "))
            {
                currentFileHeaders.Add(line);
            }
            else if (line.StartsWith("+++ "))
            {
                currentFileHeaders.Add(line);
            }
            else if (line.StartsWith("@@"))
            {
                currentHunk = new Hunk();
                currentHunk.FileHeaders.AddRange(currentFileHeaders);
                currentHunk.HunkHeader = line;
                currentHunk.Lines.Add(new HunkLine
                {
                    Content = line,
                    Type = LineType.Header
                });

                hunks.Add(currentHunk);

                var match = Regex.Match(line, @"@@ \-(\d+),?(\d*) \+(\d+),?(\d*) @@");
                var oldStart = int.Parse(match.Groups[1].Value);
                var oldLines = match.Groups[2].Success && !string.IsNullOrEmpty(match.Groups[2].Value)
                    ? int.Parse(match.Groups[2].Value) : 0;
                var newStart = int.Parse(match.Groups[3].Value);
                var newLines = match.Groups[4].Success && !string.IsNullOrEmpty(match.Groups[4].Value)
                    ? int.Parse(match.Groups[4].Value) : 0;

                int oldLine = oldStart;
                int newLine = newStart;

                foreach (var contentLine in lines.SkipWhile(l => l != line).Skip(1))
                {
                    if (contentLine.StartsWith("@@")) break;

                    var hunkLine = new HunkLine { Content = contentLine };

                    if (contentLine.StartsWith("-"))
                    {
                        hunkLine.Type = LineType.Removal;
                        hunkLine.OldLineNumber = oldLine++;
                    }
                    else if (contentLine.StartsWith("+"))
                    {
                        hunkLine.Type = LineType.Addition;
                        hunkLine.NewLineNumber = newLine++;
                    }
                    else
                    {
                        hunkLine.Type = LineType.Context;
                        hunkLine.OldLineNumber = oldLine++;
                        hunkLine.NewLineNumber = newLine++;
                    }

                    currentHunk.Lines.Add(hunkLine);
                }
            }
        }

        return hunks;
    }

    private string RunGitCommand(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = path.ProjectPath(Environment.CurrentDirectory),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Git command failed: {error}");
        }

        return output;
    }
}