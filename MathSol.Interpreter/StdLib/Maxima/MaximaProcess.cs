using System.Diagnostics;

namespace MathSol.Interpreter.StdLib.Maxima;

class MaximaProcess
{
    public MaximaProcess()
    {

    }

    public string Execute(string input)
    {
        var process = new Process();
        process.StartInfo.FileName = "C:\\maxima-5.47.0\\bin\\maxima.bat";
        process.StartInfo.Arguments = @$"-eval ""(cl-user::run)"" -f -- -very-quiet";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();

        process.StandardInput.WriteLine($"grind({input});");

        var result = process.StandardOutput.ReadLine();
        while (!result.EndsWith('$'))
            result += process.StandardOutput.ReadLine();
        process.StandardOutput.ReadLine();
        process.Close();
        return result.EndsWith('$') ? result.TrimEnd('$').Replace(" ", string.Empty) : throw new Exception(string.Format("Unexpected result: {0}", result));
    }
}
