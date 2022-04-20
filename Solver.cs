using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EqSolve;
using static Vocab;

public static class Solver
{
    private static long Fact(long a)
    {
        for (long i = a - 1; i >= 1; --i)
        {
            a *= i;
        }
        return a;
    }

    private static bool IsInt(double a)
    {
        return (a % 1) < double.Epsilon;
    }

    public static double Solve(List<string> postfixterms)
    {
        Stack<double> stack = new();

        for (int i = 0; i < postfixterms.Count; ++i)
        {
            string s = postfixterms[i];
            if (IsTwoOp(s))
            {
                double b = stack.Pop();
                double a = stack.Pop();
                double res = 0;
                if (s == ADD)
                    res = a + b;
                else if (s == SUB)
                    res = a - b;
                else if (s == MUL)
                    res = a * b;
                else if (s == DIV)
                    res = a / b;
                else if (s == EXP)
                    res = Math.Pow(a, b);
                stack.Push(res);
            }
            else if (IsOneOp(s))
            {
                double a = stack.Pop();
                double res = 0;
                if (s == NEG)
                    res = -a;
                else if (s == LOG)
                    res = Math.Log10(a);
                else if (s == LN)
                    res = Math.Log(a);
                else if (s == SIN)
                    res = Math.Sin(a);
                else if (s == COS)
                    res = Math.Cos(a);
                else if (s == TAN)
                    res = Math.Tan(a);
                else if (s == ARCSIN)
                    res = Math.Asin(a);
                else if (s == ARCCOS)
                    res = Math.Acos(a);
                else if (s == ARCTAN)
                    res = Math.Atan(a);
                else if (s == FACT)
                {
                    if (IsInt(a))
                        res = Fact((long)a);
                    else
                        throw new ArgumentException("cannot factorial decimal");
                }
                stack.Push(res);
            }
            else
            {
                stack.Push(double.Parse(s));
            }
        }

        return stack.Pop();
    }
}
