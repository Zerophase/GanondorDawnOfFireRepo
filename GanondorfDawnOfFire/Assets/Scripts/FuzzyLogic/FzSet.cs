using UnityEngine;
using System.Collections;

public class FzSet : FuzzyTerm
{
    public FuzzySet set;

    public FzSet(FuzzySet fs)
    {
        set = fs;
    }

    public override void ClearDom()
    {
        set.ClearDomain();
    }

    public override float GetDOM()
    {
        return set.Domain;
    }

    public override void ORwithDOM(float val)
    {
        set.ORwithDOM(val);
    }
}
