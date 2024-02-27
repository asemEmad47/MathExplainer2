using UnityEngine;

public class Term : MonoBehaviour
{
    private string Symbol;
    private char sign;
    private int number = 0;

    private int nue;
    private int Deno;
    public Term(string symbol, char sign , int nue = 0 , int deno = 0, int number = 0)
    {
        this.Symbol = symbol;
        this.number = number;
        this.sign = sign;
        this.nue = nue;
        this.Deno = deno;
    }

    public string GetSymbol()
    {
        return Symbol;
    }

    public int GetNumber()
    {
        return number;
    }   
    public int GetDeno()
    {
        return Deno;
    }   
    public int GetNue()
    {
        return nue;
    }

    public char GetSign()
    {
        return sign;
    }
}
