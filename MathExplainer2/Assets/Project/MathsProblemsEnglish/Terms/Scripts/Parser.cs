using System.Collections.Generic;
using Unity.VisualScripting;
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
        string number = "", nue = "", Deno = "",Symbol = "" , NumPow="",SymbPow="";
        int NumOfElementsInBracket = 0, BracketMultipliedNum = 0;
        bool BracketEnd = false;
        Debug.Log(inputField.text);
        if (inputField.text.IndexOf('\n') != -1)
        {
            i = inputField.text.IndexOf('\n');
        }

        if (inputField.text[i].Equals('\n')) {
            i++;
        }
        while (i<inputField.text.Length && !inputField.text[i].Equals('\n')&& (!inputField.text[i].Equals('<') || inputField.text[i+2].Equals('u')))
        {
            if (inputField.text[i].Equals('<'))
            {
                i = inputField.text.IndexOf('>');
                i++;
                if (int.TryParse((inputField.text[i - 6]).ToString(), out _))
                {
                    while (!inputField.text[i].Equals('<'))
                    {
                        NumPow += inputField.text[i];
                        i++;
                    }
                }
                else
                {
                    while (!inputField.text[i].Equals('<'))
                    {
                        SymbPow += inputField.text[i];
                        i++;
                    }
                }
                i = inputField.text.IndexOf('>',i)+2;
                Debug.Log(inputField.text[i - 1]);
                Debug.Log(inputField.text.Substring(i-1));

                i-=2;
            }
            else if (isSymbol(inputField.text[i]))
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
                if (number.Equals("-"))
                    number = "-1";
                Debug.Log(number);
                BracketMultipliedNum =int.Parse(number);
                number = "";
                stack.Push(inputField.text[i]);
            }
            else if (inputField.text[i].Equals(')'))
            {
                BracketEnd = true;
                AddNewTerm(number,nue,Deno,Symbol, SymbPow, NumPow);
                ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref NumPow, ref SymbPow);
                List<Term> Bracketterms = new List<Term>();
                int counter = terms.Count - 1;
                BracketTerms bracket;

                while (NumOfElementsInBracket >= 0)
                {
                    Bracketterms.Add(terms[counter]);
                    terms.RemoveAt(counter);
                    NumOfElementsInBracket--;
                    counter--;
                }
                bracket = new BracketTerms(Bracketterms, counter,BracketMultipliedNum);
                BracketMultipliedNum = 0;
                bracketTerms.Add(bracket);
                stack.Clear();
                ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref NumPow, ref SymbPow);


            }
            else if(inputField.text[i].Equals('+')|| inputField.text[i].Equals('-'))
            {
                if (i != 0 && !inputField.text[i+1].Equals('(')&& !inputField.text[i - 1].Equals('('))
                {
                    if (stack.Count != 0)
                    {
                        NumOfElementsInBracket++;
                    }
                    AddNewTerm(number,nue,Deno,Symbol,SymbPow,NumPow);
                    ResetValues(ref number, ref nue, ref Deno, ref Symbol, ref SymbPow, ref NumPow);
                }

                if (inputField.text[i].Equals('-'))
                    number += '-';
            }
            else if (!inputField.text[i].Equals('(') && !inputField.text[i].Equals('―'))
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

        if (SymbPow.Equals(""))
            SymbPow = "0";

        if (NumPow.Equals(""))
            NumPow = "0";


        if (number.Equals(""))
            term1 = new Term(Symbol, '+', NeuTemp1, DenoTemp1, int.Parse(SymbPow), int.Parse(NumPow));

        else if(number.Equals("-"))
            term1 = new Term(Symbol, '-', NeuTemp1, DenoTemp1, int.Parse(SymbPow), int.Parse(NumPow));

        else
            term1 = new Term(Symbol, number[0], NeuTemp1, DenoTemp1, int.Parse(number),int.Parse(SymbPow),int.Parse(NumPow));
        if (!BracketEnd)
        {
            terms.Add(term1);
        }
        else
            BracketEnd = false;
        ResetValues(ref number, ref nue, ref Deno, ref Symbol,ref NumPow,ref SymbPow);
        inputField.text = InputFieldCpy;
        PrintAllValues();
    }
    public void ResetValues(ref string number , ref string nue, ref string deno,ref string symbol, ref string NumPow, ref string SymbPow)
    {
        number = "";
        nue = "";
        deno = "";
        symbol = "";
        NumPow = "";
        SymbPow = "";
    }
    public void PrintAllValues()
    {
        foreach (Term term in terms)
        {
            Debug.Log(" nue " + term.GetNue() + " deno " + term.GetDeno() + " number " + term.GetNumber() + " SYMBOL " + term.GetSymbol() + "sympPow "  + term.GetSymbPow() + " numpow " + term.GetNumPow());
        }
        foreach (BracketTerms bracketTerm in bracketTerms)
        {
            bracketTerm.GetTerms().Reverse();
            foreach (Term term2 in bracketTerm.GetTerms())
            {
                Debug.Log(" nue from inside" + term2.GetNue() + " deno " + term2.GetDeno() + " number " + bracketTerm.GetNumber() + " SYMBOL " + term2.GetSymbol());

            }
        }
    }
    public bool isSymbol(char latter)
    {
        if(latter.Equals('a') || latter.Equals('b') || latter.Equals('c') || latter.Equals('x') || latter.Equals('y') || latter.Equals('z'))
            return true;
        else
            return false;
    }
    public void AddNewTerm(string number = "",string nue = "",string Deno = "",string Symbol = "", string SymbPow = "", string NumPow = "")
    {
        Term term;
        int DenoTemp = 0;
        int NeuTemp = 0;
        if (!nue.Equals(""))
        {
            NeuTemp = int.Parse(nue);
            DenoTemp = int.Parse(Deno);
        }
        if (SymbPow.Equals(""))
            SymbPow = "0";    
        if (NumPow.Equals(""))
            NumPow = "0";
        if (number.Equals("-"))
            number = "-1";
        if (number.Equals(""))
            term = new Term(Symbol, '+', NeuTemp, DenoTemp,int.Parse(NumPow),int.Parse(SymbPow));
        else
            term = new Term(Symbol, number[0], NeuTemp, DenoTemp, int.Parse(number), int.Parse(NumPow), int.Parse(SymbPow));
        terms.Add(term);
    }
}
