using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EqSolve;
using static Vocab;

public static class Parser
{
    private static string RemoveWhitespace(in string s)
    {
        return string.Join("", s.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }

    // for debugging lol
    public static void PrintTerms(List<string> terms)
    {
        int length = terms.Count;
        for (int i = 0; i < length; ++i)
        {
            Console.Write(terms[i] + ' ');
        }
        Console.WriteLine();
    }

    // for debugging lol
    /*
    public static void PrintStack(Stack<string> stack)
    {
        Stack<string> temp = new();
        while (stack.Count != 0)
        {
            temp.Push(stack.Peek());
            stack.Pop();
        }

        while (temp.Count != 0)
        {
            string s = temp.Peek();
            Console.Write(s + " ");
            temp.Pop();

            // To restore contents of
            // the original stack.
            stack.Push(s);
        }
        Console.WriteLine();
    }
    */

    private static List<string> StringToTerms(in string s)
    {
        int length = s.Length;

        if (length == 0)
            throw new ArgumentException("Not a valid string.");

        List<string> terms = new();
        StringBuilder currword = new();

        bool currwordisdec = IsDecChar(s[0]);

        for (int i = 0; i < length; ++i)
        {
            if (currwordisdec)
            {
                if (!IsDecChar(s[i]))
                {
                    terms.Add(currword.ToString());
                    currword.Clear();
                    currwordisdec = false;
                }
            }
            else
            {
                if (IsDecChar(s[i]))
                {
                    terms.Add(currword.ToString());
                    currword.Clear();
                    currwordisdec = true;
                }
                else if (IsTerm(currword.ToString()))
                {
                    terms.Add(currword.ToString());
                    currword.Clear();
                }
            }

            currword.Append(s[i]);
        }

        if (currword.Length != 0)
        {
            terms.Add(currword.ToString());
            currword.Clear();
        }

        return terms;
    }

    private static List<string> AddMultToTerms(List<string> terms)
    {
        List<string> newterms = new() { terms[0] };
        string lastterm = newterms[0];

        int length = terms.Count;
        for (int i = 1; i < length; ++i)
        {
            bool mulreq =
                (IsConstOrVar(terms[i]) && IsDec(lastterm)) ||
                (IsDec(terms[i]) && IsConstOrVar(lastterm)) ||
                (lastterm == CPAR && terms[i] == OPAR) ||
                (terms[i] == OPAR && IsDec(lastterm)) ||
                (IsDec(terms[i]) && lastterm == CPAR);
            if (mulreq)
                newterms.Add(MUL);

            newterms.Add(terms[i]);
            lastterm = terms[i];
        }

        return newterms;
    }

    private static void ReplaceVarsAndConsts(List<string> terms, string x, string y, string z)
    {
        int length = terms.Count;
        for (int i = 0; i < length; ++i)
        {
            if (terms[i] == X) terms[i] = x;
            else if (terms[i] == Y) terms[i] = y;
            else if (terms[i] == Z) terms[i] = z;
            else if (terms[i] == EUL) terms[i] = "2.718281828459045";
            else if (terms[i] == PI) terms[i] = "3.141592653589793";
        }
    }

    private static List<string> FixNegatives(List<string> terms)
    {
        List<string> newterms = new();
        if (terms[0] == SUB)
            newterms.Add(NEG);
        else
            newterms.Add(terms[0]);

        string prev;
        string curr;
        string next;

        int length = terms.Count;
        int i = 1;
        while (i < length - 1)
        {
            prev = terms[i - 1];
            curr = terms[i];
            next = terms[i + 1];

            if (curr == SUB)
            {
                if ((IsDec(prev) || prev == CPAR)
                    && (IsDec(next) || next == OPAR))
                {
                    newterms.Add(SUB);
                }
                else if (curr == SUB && next == SUB)
                {
                    newterms.Add(ADD);
                    ++i;
                }
                else if (IsDec(next))
                {
                    newterms.Add(SUB + next);
                    ++i;
                }
                else
                {
                    newterms.Add(NEG);
                }
            }
            else
            {
                newterms.Add(curr);
            }

            ++i;
        }

        if (i != length)
            newterms.Add(terms[^1]);

        return newterms;
    }

    public static List<string> InfixToPostfix(List<string> infixterms)
    {
        Stack<string> stack = new();
        stack.Push(INV);
        List<string> postfixterms = new();

        int length = infixterms.Count;
        for (int i = 0; i < length; ++i)
        {
            string s = infixterms[i];
            if (IsDec(s))
            {
                postfixterms.Add(s);
            }
            else if (s == OPAR)
            {
                stack.Push(s);
            }
            else if (s == EXP)
            {
                stack.Push(s);
            }
            else if (s == CPAR)
            {
                while (stack.Peek() != INV && stack.Peek() != OPAR)
                {
                    postfixterms.Add(stack.Pop());
                }
                stack.Pop();
            }
            else
            {
                if (Prec(s) > Prec(stack.Peek()))
                {
                    stack.Push(s);
                }
                else
                {
                    while (stack.Peek() != INV && Prec(s) <= Prec(stack.Peek()))
                    {
                        postfixterms.Add(stack.Pop());
                    }
                    stack.Push(s);
                }
            }
        }

        while (stack.Peek() != INV)
        {
            postfixterms.Add(stack.Pop());
        }

        return postfixterms;
    }

    public static List<string> ParseToTerms(string s, string x, string y, string z)
    {
        s = RemoveWhitespace(s);
        Console.Write("Remove whitespace: ");
        Console.WriteLine(s);

        List<string> terms;
        terms = StringToTerms(s);
        Console.Write("String to terms: ");
        PrintTerms(terms);

        terms = AddMultToTerms(terms);
        Console.Write("Add mult to terms: ");
        PrintTerms(terms);

        ReplaceVarsAndConsts(terms, x, y, z);
        Console.Write("Replace vars/consts: ");
        PrintTerms(terms);

        terms = FixNegatives(terms);
        Console.Write("Fix negatives: ");
        PrintTerms(terms);

        terms = InfixToPostfix(terms);
        Console.Write("Postfix notation: ");
        PrintTerms(terms);

        return terms;
    }
}
