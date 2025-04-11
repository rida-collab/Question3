using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Symbol
{
    public string VarName { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public int LineNumber { get; set; }
}

class Program
{
    static List<Symbol> symbolTable = new List<Symbol>();

    static void Main()
    {
        Console.WriteLine("Enter your variable declarations one by one (type 'exit' to stop):\n");

        int lineNum = 1;
        string input;

        while (true)
        {
            Console.Write($"Line {lineNum}: ");
            input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            ParseAndInsert(input, lineNum);
            lineNum++;
        }

        Console.WriteLine("\n=== SYMBOL TABLE ===");
        Console.WriteLine("{0,-15} | {1,-10} | {2,-10} | {3,-5}", "Variable Name", "Type", "Value", "Line");
        Console.WriteLine(new string('-', 50));

        foreach (var symbol in symbolTable)
        {
            Console.WriteLine("{0,-15} | {1,-10} | {2,-10} | {3,-5}", symbol.VarName, symbol.Type, symbol.Value, symbol.LineNumber);
        }

        if (symbolTable.Count == 0)
        {
            Console.WriteLine("No entries in the symbol table.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void ParseAndInsert(string line, int lineNumber)
    {
        // Basic variable declaration pattern: type name = value;
        string pattern = @"^(int|float|char|string|bool)\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*(.+);$";
        var match = Regex.Match(line, pattern);

        if (!match.Success)
        {
            Console.WriteLine("✘ Invalid syntax.");
            return;
        }

        string type = match.Groups[1].Value;
        string varName = match.Groups[2].Value;
        string value = match.Groups[3].Value;

        if (ContainsPalindrome(varName))
        {
            symbolTable.Add(new Symbol
            {
                VarName = varName,
                Type = type,
                Value = value,
                LineNumber = lineNumber
            });
            Console.WriteLine("✔ Inserted into symbol table.");
        }
        else
        {
            Console.WriteLine("✘ Variable name does not contain a palindrome substring of length ≥ 3.");
        }
    }

    static bool ContainsPalindrome(string word)
    {
        int len = word.Length;

        for (int i = 0; i < len; i++)
        {
            for (int j = i + 2; j < len; j++) // check substrings of length >= 3
            {
                string substr = word.Substring(i, j - i + 1); // FIX: include j
                if (IsPalindrome(substr))
                    return true;
            }
        }

        return false;
    }

    static bool IsPalindrome(string str)
    {
        int left = 0;
        int right = str.Length - 1;

        while (left < right)
        {
            if (str[left] != str[right])
                return false;
            left++;
            right--;
        }

        return true;
    }
}
