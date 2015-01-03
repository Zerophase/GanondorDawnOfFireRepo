using UnityEngine;
using System.Collections;

public class FzVery : FuzzyTerm
{
    private FuzzySet set;

    public FzVery(FzSet fzSet)
    {
        set = fzSet.set;
    }

    public override void ClearDom()
    {
        set.ClearDomain();
    }

    public override float GetDOM()
    {
        return set.Domain * set.Domain;
    }

    public override void ORwithDOM(float val)
    {
        set.ORwithDOM(val * val);
    }
}

public class FzFairly : FuzzyTerm
{
    private FuzzySet set;

    public FzFairly(FzSet fzSet)
    {
        set = fzSet.set;
    }

    public override void ClearDom()
    {
        set.ClearDomain();
    }

    public override float GetDOM()
    {
        return Mathf.Sqrt(set.Domain);
    }

    public override void ORwithDOM(float val)
    {
        set.ORwithDOM(Mathf.Sqrt(val));
    }
}
