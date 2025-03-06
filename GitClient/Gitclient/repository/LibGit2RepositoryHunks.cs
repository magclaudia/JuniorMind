using GitClient;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using GitClient.ui;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace GitClient.repository
{
    public class LibGit2RepositoryHunks
    {
        private PanelCommunicationService panelCommunicationService;
        private GetProjectPath path;
        private string diff;
        public LibGit2RepositoryHunks(PanelCommunicationService panelCommunicationService)
        {
            this.panelCommunicationService = panelCommunicationService;
            path = new GetProjectPath();
            diff = "";
        }


        public void StageHunk(string line)
        {
            List<Hunk> hunks = ParseGitDiff();

            if (hunks.Count == 0)
            {
                RunGitCommand($"add -- {panelCommunicationService.GetFilePath()}");
            }

            int hunkIndex = -1;

            if (line.StartsWith("@@"))
            {
                hunkIndex = hunks.FindIndex(x => x.HunkHeader == line);
            }
            else
            {
                hunkIndex = hunks.FindIndex(x => x.Lines.Any(y => y.Content == line));
            }
            
            if (hunkIndex < 0 || hunkIndex >= hunks.Count)
            {
                return;
            }

            HandleLineSelection(hunks[hunkIndex], line);
        }

        public void UnstageHunk(int hunkIndex, string line)
        {
            List<Hunk> hunks = ParseGitDiff();

            if (hunkIndex < 0 || hunkIndex >= hunks.Count && hunks.Count > 0)
            {
                return;
            }
            else if (hunks.Count == 0 || hunks[hunkIndex].HunkHeader!.Contains("-0,0"))
            {
                RunGitCommand($"reset -- {panelCommunicationService.GetFilePath()}");
            }
            else
            {
                HandleLineSelection(hunks[hunkIndex], line);
            }
        }
        private void HandleLineSelection(Hunk hunk, string line)
        {
            List<string> diffList = new List<string>();
            bool success = false;

            if (line.StartsWith("@@"))
            {
                if (ReadButtons.WorkingInUnstagePanel == true)
                {
                    success = StageGitHunk(hunk);
                }
                else
                {
                    success = UnstageGitHunk(hunk);
                }
            }
            else if (line.StartsWith('+') || line.StartsWith('-'))
            {
                int indexForHunk = hunk.Lines.FindIndex(x => x.Content == line);

                if (indexForHunk < 0)
                {
                    return;
                }

                if (ReadButtons.WorkingInUnstagePanel == true)
                {
                    // success = StageSingleLine(hunk, hunk.Lines[indexForHunk]);
                }
                else
                {
                    //success = UnstageSingleLine(hunk, hunk.Lines[indexForHunk]);
                }
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

            if (header.HunkHeader!.Length > startFrom)
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

        private bool StageGitHunk(Hunk hunk)
        {
            var patch = BuildFullPatch(
                hunk.FileHeaders,
                hunk.HunkHeader!,
                hunk.Lines.Where(line => line.Type != LineType.Header).ToList());
            
            if (ApplyPatch(patch))
            {
                diff = null!;
                return true;
            }
            return false;
        }

        private bool UnstageGitHunk(Hunk hunk)
        {
            var patch = BuildFullPatch(
                hunk.FileHeaders,
                hunk.HunkHeader!,
                hunk.Lines.Where(line => line.Type != LineType.Header).ToList());
            
            if (ApplyPatch(patch))
            {
                diff = null!;
                return true;
            }
            return false;
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
                string result = "";

                if (ReadButtons.WorkingInUnstagePanel == true)
                {
                    result = RunGitCommand($"apply --cached --verbose \"{tempFile}\"");
                }
                else
                {
                    result = RunGitCommand($"apply -R --cached \"{tempFile}\"");
                }

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
            if (string.IsNullOrEmpty(diff))
            {
                if (ReadButtons.WorkingInUnstagePanel == true)
                {
                    diff = RunGitCommand($"diff -- {panelCommunicationService.GetFilePath()}");
                }
                else
                {
                    diff = RunGitCommand($"diff --staged {panelCommunicationService.GetFilePath()}");
                }
            }
            return ParseDiffHunks();
        }

        private List<Hunk> ParseDiffHunks()
        {
            List<Hunk> hunks = new List<Hunk>();
            string[] lines = diff.Split('\n');
            List<string> currentFileHeaders = new List<string>();
            Hunk? currentHunk = new Hunk();
            bool inHunk = false;
            int currentOldLine = 0;
            int currentNewLine = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("diff --git"))
                {
                    currentFileHeaders = new List<string> { line };

                    if (inHunk)
                    {
                        hunks.Add(currentHunk!);
                        currentHunk = null;
                        inHunk = false;
                    }
                }
                else if (line.StartsWith("--- ") || line.StartsWith("+++ "))
                {
                    currentFileHeaders.Add(line);
                }
                else if (line.StartsWith("@@"))
                {
                    if (inHunk)
                    {
                        hunks.Add(currentHunk!);
                    }

                    currentHunk = new Hunk();
                    inHunk = true;
                    currentHunk.FileHeaders.AddRange(currentFileHeaders);
                    currentHunk.HunkHeader = line;
                    currentHunk.Lines.Add(new HunkLine
                    {
                        Content = line,
                        Type = LineType.Header
                    });

                    var match = Regex.Match(line, @"@@ \-(\d+),?(\d*) \+(\d+),?(\d*) @@");
                    
                    if (!match.Success)
                    {
                        continue;
                    }

                    int oldStart = int.Parse(match.Groups[1].Value);
                    int oldCount = string.IsNullOrEmpty(match.Groups[2].Value)
                        ? 1
                        : int.Parse(match.Groups[2].Value);

                    int newStart = int.Parse(match.Groups[3].Value);
                    int newCount = string.IsNullOrEmpty(match.Groups[4].Value)
                        ? 1
                        : int.Parse(match.Groups[4].Value);

                    currentOldLine = oldStart;
                    currentNewLine = newStart;
                }
                else if (inHunk)
                {
                    var hunkLine = new HunkLine() { Content = line };

                    if (line.StartsWith("-"))
                    {
                        hunkLine.Type = LineType.Removal;
                        hunkLine.OldLineNumber = currentOldLine++;
                    }
                    else if (line.StartsWith("+"))
                    {
                        hunkLine.Type = LineType.Addition;
                        hunkLine.NewLineNumber = currentNewLine++;
                    }
                    else
                    {
                        hunkLine.Type = LineType.Context;
                        hunkLine.OldLineNumber = currentOldLine++;
                        hunkLine.NewLineNumber = currentNewLine++;
                    }

                    currentHunk!.Lines.Add(hunkLine);
                }
            }

            if (inHunk)
            {
                hunks.Add(currentHunk!);
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
            var output = process!.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Git command failed: {error}");
            }

            return output;
        }
    }
}