using System;
using z80egui.libscas;
using System.Runtime.InteropServices;

namespace z80egui
{
    public unsafe class Program
    {
        public static unsafe void Main(string[] args)
        {
            list_t *symbols = scas.create_list();
            symbol_t test_symbol = new symbol_t
            {
                type = SymbolType.Equate,
                value = 1234,
                name = (char *)Marshal.StringToHGlobalAnsi("test_symbol")
            };
            scas.list_add(symbols, &test_symbol);
            var expression = scas.parse_expression("2 + 2 + test_symbol");
            for (int i = 0; i < expression->tokens->length; i++)
            {
                var token = (expression_token_t*)expression->tokens->items[i];
                switch (token->type)
                {
                    case TokenType.Number:
                        Console.WriteLine("[number] {0}", token->number);
                        break;
                    case TokenType.Operator:
                        Console.WriteLine("[operator] {0}", token->operator_);
                        break;
                    case TokenType.Symbol:
                        Console.WriteLine("[symbol] {0}", Marshal.PtrToStringAnsi((IntPtr)token->symbol));
                        break;
                    case TokenType.OpenParenthesis:
                        Console.WriteLine("[open parenthesis]");
                        break;
                }
            }
            int err;
            ulong result = scas.evaluate_expression(expression, symbols, &err);
            Console.WriteLine("Result: {0}", result);
        }
    }
}