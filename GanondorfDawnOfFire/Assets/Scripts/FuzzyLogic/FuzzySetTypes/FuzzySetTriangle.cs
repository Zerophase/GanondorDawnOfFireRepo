using UnityEngine;
using System.Collections;

public class FuzzySetTriangle : FuzzySet
{
    private float peakPoint;
    private float rightOffset;
    private float leftOffset;

    public FuzzySetTriangle(float peak, float leftOffSet, float rightOffSet)
        : base(peak)
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
        else if (val <= peakPoint && val >= peakPoint - leftOffset)
        {
            float grad = 1.0f / leftOffset;
            return grad * (val - (peakPoint - leftOffset));
        }
        else if (val > peakPoint && val < peakPoint + rightOffset)
        {
            float grad = 1.0f / -rightOffset;
            return grad * (val - peakPoint) + 1.0f;
        }
        else
            return 0.0f;
    }
}
