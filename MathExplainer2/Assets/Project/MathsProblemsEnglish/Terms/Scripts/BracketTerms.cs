using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BracketTerms : MonoBehaviour
{
    private List<Term> terms = new List<Term>();
    private int number = 0;
    private Term term;

    public BracketTerms(List<Term> terms,Term term = null, int number = 0)
    {
        this.number = number;
        this.terms = terms;
        this.term = term;
    }
    
    public int GetNumber()
    {
        return number;
    }
    public List<Term> GetTerms() { 
        return terms;
    }
    public Term GetTerm()
    {
        return term;
    }
}
