using MathSol.Interpreter.FileSystem;
using System;
using System.IO;
using System.Text;

namespace MathSol.Interpreter.Preprocessor
{
    public class Preprocessor
    {
        public CodeFile Preprocess(CodeFile codeFile)
        {
            ArgumentNullException.ThrowIfNull(codeFile);
            if (string.IsNullOrEmpty(codeFile.FileName))
            {
                throw new ArgumentException("CodeFile.FileName cannot be null or empty for preprocessing with includes.", nameof(codeFile.FileName));
            }

            var processedCodeBuilder = new StringBuilder();
            var lines = codeFile.RestOfCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string currentFileDirectory = Path.GetDirectoryName(codeFile.FileName);

            if (string.IsNullOrEmpty(currentFileDirectory) && codeFile.FileName != "Code snippet")
            {
                currentFileDirectory = Environment.CurrentDirectory;
            }

            foreach (var line in lines)
            {
                string trimmedLine = line.TrimStart();
                if (trimmedLine.StartsWith("%include"))
                {
                    string includedFileNameRaw = trimmedLine.Substring("%include".Length).Trim();

                    if (includedFileNameRaw.StartsWith("\"") && includedFileNameRaw.EndsWith("\"") && includedFileNameRaw.Length > 1)
                    {
                        includedFileNameRaw = includedFileNameRaw.Substring(1, includedFileNameRaw.Length - 2);
                    }
                    else if (includedFileNameRaw.StartsWith("'") && includedFileNameRaw.EndsWith("'") && includedFileNameRaw.Length > 1)
                    {
                        includedFileNameRaw = includedFileNameRaw.Substring(1, includedFileNameRaw.Length - 2);
                    }

                    string resolvedIncludePath;

                    if (Path.IsPathRooted(includedFileNameRaw))
                    {
                        resolvedIncludePath = includedFileNameRaw;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(currentFileDirectory))
                        {
                            throw new InvalidOperationException($"Cannot resolve relative include path '{includedFileNameRaw}' because the base directory of the current file ('{codeFile.FileName}') is unknown. Ensure the main file has a valid path.");
                        }
                        resolvedIncludePath = Path.GetFullPath(Path.Combine(currentFileDirectory, includedFileNameRaw));
                    }

                    try
                    {
                        if (!File.Exists(resolvedIncludePath))
                        {
                            throw new FileNotFoundException($"Included file not found: '{resolvedIncludePath}'. Referenced in '{codeFile.FileName}' as '{includedFileNameRaw}'.");
                        }

                        if (string.Equals(resolvedIncludePath, codeFile.FileName, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException($"Recursive include detected: File '{resolvedIncludePath}' attempts to include itself. Referenced in '{codeFile.FileName}'.");
                        }

                        var includedFile = CodeFile.FromFile(resolvedIncludePath);
                        var preprocessedIncludedFile = Preprocess(includedFile);
                        processedCodeBuilder.Append(preprocessedIncludedFile.RestOfCode.TrimEnd());
                        processedCodeBuilder.AppendLine();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    processedCodeBuilder.AppendLine(line);
                }
            }

            string finalCode = processedCodeBuilder.ToString().TrimEnd();

            return CodeFile.FromProcessedFile(codeFile.FileName, finalCode);
        }
    }
}
