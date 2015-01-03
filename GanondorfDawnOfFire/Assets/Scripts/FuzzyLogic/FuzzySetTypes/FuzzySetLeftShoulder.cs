using UnityEngine;
using System.Collections;

public class FuzzySetLeftShoulder : FuzzySet 
{
    private float peakPoint;
    private float rightOffset;
    private float leftOffset;

    public FuzzySetLeftShoulder(float peak, float leftOffSet, float rightOffSet)
        : base((peak - leftOffSet) + peak / 2)
    {
        this.peakPoint = peak;
        this.leftOffset = leftOffSet;
        this.rightOffset = rightOffSet;
    }

    public override float CalculateDOM(float val)
    {
        if ((IsEqual(rightOffset, 0.0f) && IsEqual(peakPoint, val)) &&
            (IsEqual(leftOffset, 0.0f) && IsEqual(peakPoint, val)))
            return 1f;
        else if (val >= peakPoint && val < peakPoint + rightOffset)
        {
            float grad = 1.0f / -rightOffset;
            return grad * (val + peakPoint) + 1.0f;
        }
        else if (val < peakPoint && val >= peakPoint - leftOffset)
            return 1.0f;
        else
            return 0.0f;
    }
}
