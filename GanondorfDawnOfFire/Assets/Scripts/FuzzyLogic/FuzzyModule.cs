using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyModule 
{
    public enum DefuzzifyMethod { max_av, centroid };
    public enum DefuzzifySamples{MaxSamples = 15};

    private Dictionary<string, FuzzyVariable > FuzzyVariables = new Dictionary<string, FuzzyVariable>();

    private List<FuzzyRule> rules = new List<FuzzyRule>();

    public FuzzyModule()
    {

    }

    private void setConfidencesOfConsequestToZero()
    {
        for (int i = 0; i < rules.Count; i++)
        {
            rules[i].SetConfidenceOfConsequentToZero();
        }
    }

    public FuzzyVariable CreateFLV(string varName)
    {
       return FuzzyVariables[varName] = new FuzzyVariable();
    }

    public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence)
    {
        rules.Add(new FuzzyRule(antecedent, consequence));
    }

    public void Fuzzify(string nameOfFLV, float val)
    {
        FuzzyVariables[nameOfFLV].Fuzzify(val);
    }

    public float DeFuzzify(string key, DefuzzifyMethod method = DefuzzifyMethod.max_av)
    {
        setConfidencesOfConsequestToZero();

        for (int i = 0; i < rules.Count; i++)
        {
            rules[i].Calculate();   
        }

        switch (method)
        {
            case DefuzzifyMethod.max_av:
                return FuzzyVariables[key].DeFuzzifyMaxAv();
            case DefuzzifyMethod.centroid:
                return 1f;
            default:
                return 1f;
        }
    }
}
