namespace EqSolve;

public static class Vocab
{
    // invalid
    public const string
        INV = "#";

    // parenthesis
    public const string
        OPAR = "(",
        CPAR = ")";

    // double parameter operators
    public const string
        ADD = "+",
        SUB = "-",
        MUL = "*",
        DIV = "/",
        EXP = "^";

    // single parameter operators
    public const string
        NEG = "~",
        LOG = "log",
        LN = "ln",
        SIN = "sin",
        COS = "cos",
        TAN = "tan",
        ARCSIN = "arcsin",
        ARCCOS = "arccos",
        ARCTAN = "arctan",
        FACT = "!";

    public const string
        EUL = "e",
        PI = "pi";

    // variables
    public const string
        X = "x",
        Y = "y",
        Z = "z";
    
    public static bool IsDecChar(char c)
    {
        return char.IsDigit(c) || c == '.';
    }

    public static bool IsDec(in string s)
    {
        int length = s.Length;

        if (length == 0)
            return false;

        int minLength = 1;
        bool hasDecimal = false;

        if (s[0] == '-')
            ++minLength;
        else if (s[0] == '.')
            hasDecimal = true;
        else if (!char.IsDigit(s[0]))
            return false;

        for (int i = 1; i < length; ++i)
        {
            if (s[i] == '.')
            {
                if (hasDecimal)
                    return false;
                else
                    hasDecimal = true;
            }
            else if (s[i] == '-')
            {
                return false;
            }
            else if (!char.IsDigit(s[i]))
            {
                return false;
            }
        }

        if (hasDecimal)
            ++minLength;

        if (length < minLength)
            return false;

        return true;
    }

    public static bool IsTerm(in string s)
    {
        return
            s == OPAR ||
            s == CPAR ||
            s == ADD ||
            s == SUB ||
            s == MUL ||
            s == DIV ||
            s == EXP ||
            s == LOG ||
            s == LN ||
            s == SIN ||
            s == COS ||
            s == TAN ||
            s == ARCSIN ||
            s == ARCCOS ||
            s == ARCTAN ||
            s == FACT ||
            s == EUL ||
            s == PI ||
            s == X ||
            s == Y ||
            s == Z;
    }

    public static bool IsOp(in string s)
    {
        return
            s == ADD ||
            s == SUB ||
            s == MUL ||
            s == DIV ||
            s == EXP ||
            s == NEG ||
            s == LOG ||
            s == LN ||
            s == SIN ||
            s == COS ||
            s == TAN ||
            s == ARCSIN ||
            s == ARCCOS ||
            s == ARCTAN ||
            s == FACT;
    }

    public static bool IsTwoOp(in string s)
    {
        return
            s == ADD ||
            s == SUB ||
            s == MUL ||
            s == DIV ||
            s == EXP;
    }

    public static bool IsOneOp(in string s)
    {
        return
            s == NEG ||
            s == LOG ||
            s == LN ||
            s == SIN ||
            s == COS ||
            s == TAN ||
            s == ARCSIN ||
            s == ARCCOS ||
            s == ARCTAN ||
            s == FACT;
    }

    public static int Prec(in string op)
    {
        if (op == ADD || op == SUB || op == NEG)
            return 1;
        else if (op == MUL || op == DIV)
            return 2;
        else if (op == EXP)
            return 3;
        else if (op == LOG || op == LN || op == SIN || op == COS || op == TAN ||
            op == ARCSIN || op == ARCCOS || op == ARCTAN || op == FACT)
            return 4;
        else
            return 0;
    }

    public static bool IsConst(in string s)
    {
        return
            s == EUL ||
            s == PI;
    }

    public static bool IsVar(in string s)
    {
        return
            s == X ||
            s == Y ||
            s == Z;
    }

    public static bool IsConstOrVar(in string s)
    {
        return
            s == EUL ||
            s == PI ||
            s == X ||
            s == Y ||
            s == Z;
    }
}
