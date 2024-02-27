using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputField;
    List<Term> terms = new List<Term>();
    List<BracketTerms> bracketTerms = new List<BracketTerms>();
    Stack<char> stack = new Stack<char>();
    private string InputFieldCpy;
    public void ParseInputField()
    {
        InputFieldCpy = inputField.text;
        int i = 0;
        string number = "", nue = "", Deno = "",Symbol = "";
        int NumOfElementsInBracket = 0;
        if (inputField.text.IndexOf('\n') != -1)
        {
            i = inputField.text.IndexOf('\n');
        }

        if (inputField.text[i].Equals('\n')) {
            i++;
        }
        while (i<inputField.text.Length && !inputField.text[i].Equals('\n')&& !inputField.text[i].Equals('<'))
        {

            if (isSymbol(inputField.text[i]))
            {
                Symbol += inputField.text[i];

            }
            else if (inputField.text[i].Equals('―'))
            {
                int NueIndex = inputField.text.IndexOf("%>")+2;
                while (!inputField.text[NueIndex].Equals(' ') && !inputField.text[NueIndex].Equals('<') && !isSymbol(inputField.text[NueIndex]))
                {
                    nue += inputField.text[NueIndex];
                    inputField.text = inputField.text.Remove(NueIndex, 1);
                    i--;
                }

                while (inputField.text[NueIndex].Equals(' ') && !inputField.text[NueIndex].Equals('<')&& !isSymbol(inputField.text[NueIndex]))
                {
                    inputField.text = inputField.text.Remove(NueIndex, 1);
                    i--;
                }

                int DenoIndex = inputField.text.IndexOf("</line-height>")+24;

                while (!inputField.text[DenoIndex].Equals(' ') && !inputField.text[DenoIndex].Equals('<')&&!isSymbol(inputField.text[NueIndex]))
                {
                    Deno += inputField.text[DenoIndex];
                    inputField.text = inputField.text.Remove(DenoIndex, 1);
                }

                while (inputField.text[DenoIndex].Equals(' ') && !inputField.text[DenoIndex].Equals('<')&&!isSymbol(inputField.text[NueIndex]))
                {
                    inputField.text = inputField.text.Remove(DenoIndex, 1);
                }

            }
            else if (inputField.text[i].Equals('('))
            {
                stack.Push(inputField.text[i]);
            }
            else if (inputField.text[i].Equals(')'))
            {
                List<Term> Bracketterms = new List<Term>();
                int counter = terms.Count - 1;
                while (NumOfElementsInBracket != 0)
                {
                    Bracketterms.Add(terms[counter]);
                    NumOfElementsInBracket--;
                    counter--;
                }
                BracketTerms bracket ;
                if (Symbol.Equals(""))
                    bracket = new BracketTerms(Bracketterms, null, int.Parse(number));
                else
                    bracket = new BracketTerms(Bracketterms, terms[counter], int.Parse(number));

                bracketTerms.Add(bracket);
                stack.Clear();
                ResetValues(ref number, ref nue, ref Deno, ref Symbol);

            }
            else if(inputField.text[i].Equals('+')|| inputField.text[i].Equals('-'))
            {
                if (stack.Count != 0)
                {
                    NumOfElementsInBracket++;
                }
                Term term ;
                int DenoTemp = 0;
                int NeuTemp = 0;
                if (!nue.Equals(""))
                {
                    NeuTemp = int.Parse(nue);
                    DenoTemp = int.Parse(Deno);
                }

                if (number.Equals(""))
                    term = new Term(Symbol, '+', NeuTemp, DenoTemp);
                else
                    term = new Term(Symbol, number[0], NeuTemp, DenoTemp, int.Parse(number));
                terms.Add(term);

                ResetValues(ref number, ref nue, ref Deno, ref Symbol);
                if (inputField.text[i].Equals('-'))
                    number += '-';
            }
            else if (stack.Count == 0 && !inputField.text[i].Equals('(') && !inputField.text[i].Equals('―'))
            {
                number += inputField.text[i];
            }
            i++;

        }
        Term term1;
        int DenoTemp1 = 0;
        int NeuTemp1 = 0;
        if (!nue.Equals(""))
        {
            NeuTemp1 = int.Parse(nue);
            DenoTemp1 = int.Parse(Deno);
        }

        if (number.Equals(""))
            term1 = new Term(Symbol, '+', NeuTemp1, DenoTemp1);

        else if(number.Equals("-"))
            term1 = new Term(Symbol, '-', NeuTemp1, DenoTemp1);

        else
            term1 = new Term(Symbol, number[0], NeuTemp1, DenoTemp1, int.Parse(number));

        terms.Add(term1);
        ResetValues(ref number, ref nue, ref Deno, ref Symbol);
        inputField.text = InputFieldCpy;
        PrintAllValues();
    }
    public void ResetValues(ref string number , ref string nue, ref string deno,ref string symbol)
    {
        number = "";
        nue = "";
        deno = "";
        symbol = "";
    }
    public void PrintAllValues()
    {
        Debug.Log(terms.Count);
        foreach (Term term in terms)
        {
            Debug.Log(" nue " + term.GetNue() + " deno " + term.GetDeno() + " number " + term.GetNumber() + " SYMBOL " + term.GetSymbol());
        }
    }
    public bool isSymbol(char latter)
    {
        if(latter.Equals('a') || latter.Equals('b') || latter.Equals('c') || latter.Equals('x') || latter.Equals('y') || latter.Equals('z'))
            return true;
        else
            return false;
    }
}
