using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BracketTerms : MonoBehaviour
{
    private List<Term> terms = new List<Term>();
    private int index = 0;
    private int number = 0;
    public BracketTerms(List<Term> terms, int index, int number )
    {
        this.index = index;
        this.terms = terms;
        this.number = number;
    }

    public List<Term> GetTerms() { 
        return terms;
    }
    public int GetIndex()
    {
        return index;
    }
    public int GetNumber()
    {
        return number;
    }
}
