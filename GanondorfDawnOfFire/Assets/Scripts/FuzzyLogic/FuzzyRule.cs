using UnityEngine;
using System.Collections;

public class FuzzyRule
{
    private FuzzyTerm antecedent;
    private FuzzyTerm consequence;

    public FuzzyRule(FuzzyTerm antecedent, FuzzyTerm consequence)
    {
        this.antecedent = antecedent;
        this.consequence = consequence;
    }

    public void SetConfidenceOfConsequentToZero()
    {
        consequence.ClearDom();
    }

    public void Calculate()
    {
        consequence.ORwithDOM(antecedent.GetDOM());
    }
}
