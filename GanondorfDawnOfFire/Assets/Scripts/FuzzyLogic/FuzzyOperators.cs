using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FzAnd : FuzzyTerm 
{
    private List<FuzzyTerm> terms = new List<FuzzyTerm>();

    public FzAnd(FzAnd and)
    {
        terms.Add(and);
    }

    public FzAnd(FuzzyTerm op1, FuzzyTerm op2)
    {
        terms.Add(op1);
        terms.Add(op2);
    }

    public FzAnd(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3)
    {
        terms.Add(op1);
        terms.Add(op2);
        terms.Add(op3);
    }

    public FzAnd(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4)
    {
        terms.Add(op1);
        terms.Add(op2);
        terms.Add(op3);
        terms.Add(op4);
    }

    public override float GetDOM()
    {
        float smallest = float.MaxValue;
        for (int i = 0; i < terms.Count; i++)
        {
            if (terms[i].GetDOM() < smallest)
            {
                smallest = terms[i].GetDOM();
            }
        }

        return smallest;
    }

    public override void ClearDom()
    {
        for (int i = 0; i < terms.Count; i++)
        {
            terms[i].ClearDom();
        }
    }

    public override void ORwithDOM(float val)
    {
        for (int i = 0; i < terms.Count; i++)
        {
            terms[i].ClearDom();
        }
    }
}

public class FzOR : FuzzyTerm
{
    private List<FuzzyTerm> terms = new List<FuzzyTerm>();

    public FzOR(FzOR and)
    {
        terms.Add(and);
    }

    public FzOR(FuzzyTerm op1, FuzzyTerm op2)
    {
        terms.Add(op1);
        terms.Add(op2);
    }

    public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3)
    {
        terms.Add(op1);
        terms.Add(op2);
        terms.Add(op3);
    }

    public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4)
    {
        terms.Add(op1);
        terms.Add(op2);
        terms.Add(op3);
        terms.Add(op4);
    }

    public override float GetDOM()
    {
        float largest = float.MinValue;
        for (int i = 0; i < terms.Count; i++)
        {
            if (terms[i].GetDOM() > largest)
            {
                largest = terms[i].GetDOM();
            }
        }

        return largest;
    }

    public override void ClearDom()
    {
        throw new System.NotImplementedException();
    }

    public override void ORwithDOM(float val)
    {
        throw new System.NotImplementedException();
    }
}
