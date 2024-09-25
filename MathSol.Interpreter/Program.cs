using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer;
using MathSol.Interpreter.Parser;
using MathSol.Interpreter;
using MathSol.Interpreter.Executor;
using System.Diagnostics;

// Create a stopwatch instance


// Start measuring time


// Your code here
CodeFile codeFile = new CodeFile("test.msl");
ITokenizer tokenizer = new Tokenizer();
var result = tokenizer.Tokenize(codeFile);

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

for (int i = 0; i < 10000; i++)
{
    var parser = new Parser(result);
    var r = parser.Parse();
    var executor = new Executor(r);
    executor.Execute();
}



// Stop measuring time
stopwatch.Stop();

// Get the elapsed time as a TimeSpan object
TimeSpan ts = stopwatch.Elapsed;

// Output the execution time in high precision
Console.WriteLine($"Execution time: {ts.TotalSeconds:F10} seconds");