using MathSol.Interpreter.FileSystem;
using System.Text.RegularExpressions;

namespace MathSol.Interpreter.Preprocessor;

public class Preprocessor
{
    public CodeFile Preprocess(CodeFile codeFile)
    {
        var code = string.Empty;
        var lines = codeFile.RestOfCode.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (line.StartsWith("%include"))
            {
                var fileName = line.Substring(8).Trim();
                var includedFile = CodeFile.FromFile(fileName);
                var preprocessedIncludedFile = Preprocess(includedFile);
                code += preprocessedIncludedFile.RestOfCode.Trim() + Environment.NewLine;
            }
            else
            {
                code += line + Environment.NewLine;
            }
        }

        code = code.Trim();

        return CodeFile.FromProcessedFile(codeFile.FileName, code);
    }
}
