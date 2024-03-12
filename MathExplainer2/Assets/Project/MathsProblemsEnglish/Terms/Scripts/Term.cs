using UnityEngine;

public class Term : MonoBehaviour
{
    private readonly string Symbol;
    private readonly char sign;
    private readonly int number = 0;

    private readonly int nue;
    private readonly int Deno;
    private readonly int NumPow = 0;
    private readonly int SymbPow = 0;
    public Term(string symbol, char sign , int nue = 0 , int deno = 0, int number = 0,int NumPow = 0,int SymbPow = 0)
    {
        this.Symbol = symbol;
        this.number = number;
        this.sign = sign;
        this.nue = nue;
        this.Deno = deno;
        this.NumPow = NumPow;
        this.SymbPow = SymbPow;
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

    public int GetNumPow()
    {
        return NumPow;
    }    
    public int GetSymbPow()
    {
        return SymbPow;
    }
}
