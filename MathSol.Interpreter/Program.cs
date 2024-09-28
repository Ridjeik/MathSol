using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer;
using MathSol.Interpreter;
using MathSol.Interpreter.Parser.Parsers;

CodeFile codeFile = new CodeFile("test.msl");
ITokenizer tokenizer = new Tokenizer();
var result = tokenizer.Tokenize(codeFile);
var tokens = result.GetEnumerator();
tokens.MoveNext();

var result2 = new ProgramParser(tokens).Parse();

Console.WriteLine(result2);