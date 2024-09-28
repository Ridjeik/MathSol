using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSol.Interpreter.Tokenizer.Readers
{
    internal class MinusTokenReader : ITokenReader
    {
        public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
        {
            if (codeFile.PeekChar() == '-')
            {
                codeFile.ConsumeChar();
                return new MinusToken();
            }

            return null;
        }
    }
}