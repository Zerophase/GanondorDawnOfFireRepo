using UnityEngine;
using System.Collections;

public abstract class FuzzyTerm 
{
    //TODO figure out what to do in place of CLone
    public abstract float GetDOM();
    public abstract void ClearDom();
    public abstract void ORwithDOM(float val);
}
