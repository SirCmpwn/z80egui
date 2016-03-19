using System;
using System.Runtime.InteropServices;

namespace z80egui.libscas
{
    public static unsafe partial class scas
    {
        [DllImport("scas")]
        public static extern tokenized_expression_t* parse_expression(string str);
        [DllImport("scas")]
        public static extern ulong evaluate_expression(tokenized_expression_t* expression,
            list_t *symbols, int *error);
    }

    public enum TokenType
    {
        Symbol = 0x00,
        Number = 0x01,
        Operator = 0x02,
        OpenParenthesis = 0x03
    }

    public enum ExpressionResult
    {
        Success = 0,
        BadSymbol = 1,
        BadSyntax = 2
    }

    public enum OperatorType
    {
        Unary = 1,
        Binary = 2
    }

    public enum Operator
    {
        UnaryPlus = 0,
        UnaryMinus = 1,
        Negate = 2,
        LogicalNot = 3,
        Multiply = 4,
        Divide = 5,
        Modulo = 6,
        Plus = 7,
        Minus = 8,
        LeftShift = 9,
        RightShift = 10,
        LessThanOrEqualTo = 11,
        GreaterThanOrEqualTo = 12,
        LessThan = 13,
        GreaterThan = 14,
        EqualTo = 15,
        NotEqualTo = 16,
        BitwiseAND = 17,
        BitwiseXOR = 18,
        BitwiseOR = 19,
        LogicalAND = 20,
        LogicalOR = 21
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct expression_token_t
    {
        public TokenType type
        {
            get
            {
                return (TokenType)_type;
            }
            set
            {
                _type = (int)value;
            }
        }

        public Operator operator_
        {
            get
            {
                return (Operator)_operator;
            }
            set
            {
                _operator = (int)value;
            }
        }

        public int _type;
        public char* symbol;
        public ulong number;
        public int _operator;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct tokenized_expression_t
    {
        public list_t* tokens;
        public list_t* symbols;
    }
}