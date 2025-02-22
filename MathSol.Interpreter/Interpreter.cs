using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Utils;
using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter;

public class Interpreter
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    public Interpreter()
    {
        this.ConfigureServices();
    }

    private void ConfigureServices()
    {
        _serviceCollection.AddTokenizer();
        _serviceCollection.AddParser();
        _serviceCollection.AddStdLib();
    }

    public void Interpret(CodeFile codeFile)
    {
        ArgumentNullException.ThrowIfNull(codeFile);

        var serviceProvider = this._serviceCollection.BuildServiceProvider();

        var tokens = serviceProvider.GetRequiredService<ITokenizer>().Tokenize(codeFile);
        //tokens.ToList().ForEach(Console.WriteLine);


        var tokensEnumerator = tokens.GetEnumerator();
        tokensEnumerator.MoveNext();


        var syntaxTree = serviceProvider.GetRequiredService<IParser>().Parse(tokensEnumerator);
        //Console.WriteLine(syntaxTree.ToString());
        
        serviceProvider.GetRequiredService<INodeExecutor>().Execute(syntaxTree);
    }
}
