using UnityEngine;
using System.Collections;

public class FuzzySetSingleton : FuzzySet 
{
    private float peakPoint;
    private float rightOffset;
    private float leftOffset;

    public FuzzySetSingleton(float peak, float leftOffSet, float rightOffSet)
        : base(peak)
    {
        this.peakPoint = peak;
        this.leftOffset = leftOffSet;
        this.rightOffset = rightOffSet;
    }

    public override float CalculateDOM(float val)
    {
        if (val >= peakPoint - leftOffset &&
            val <= peakPoint + rightOffset)
            return 1f;
        else
            return 0.0f;
    }
}
