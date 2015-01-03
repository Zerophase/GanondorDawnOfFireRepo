using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyVariable   
{
    private Dictionary<string, FuzzySet> memberSet = new Dictionary<string, FuzzySet>();
    private List<string> names = new List<string>();

    float minRange;
    float maxRange;

    private void adjustRangeToFit(float min, float max)
    {
        if (min < minRange)
            minRange = min;
        if (max > maxRange)
            maxRange = max;
    }

    public FuzzyVariable()
    {
        minRange = 0.0f;
        maxRange = 0.0f;
    }

    public FzSet AddLeftShoulderSet(string name, float minBound, float peak, float maxBound)
    {
        names.Add(name);
        memberSet.Add(name, new FuzzySetLeftShoulder(peak, peak - minBound, maxBound - peak));
        adjustRangeToFit(minBound, maxBound);
        return new FzSet(memberSet[name]);
    }

    public FzSet AddRightSHoulderSet(string name, float minBound, float peak, float maxBound)
    {
        names.Add(name);
        memberSet.Add(name, new FuzzySetRightShoulder(peak, peak - minBound, maxBound - peak));
        adjustRangeToFit(minBound, maxBound);
        return new FzSet(memberSet[name]);
    }

    public FzSet AddTriangularSet(string name, float minBound, float peak, float maxBound)
    {
        names.Add(name);
        memberSet.Add(name, new FuzzySetTriangle(peak, peak - minBound, maxBound - peak));
        adjustRangeToFit(minBound, maxBound);
        return new FzSet(memberSet[name]);
    }

    public FzSet AddSingletonSet(string name, float minBound, float peak, float maxBound)
    {
        names.Add(name);
        memberSet.Add(name, new FuzzySetSingleton(peak, peak - minBound, maxBound - peak));
        adjustRangeToFit(minBound, maxBound);
        return new FzSet(memberSet[name]);
    }

    public void Fuzzify( float val)
    {
        for (int i = 0; i < names.Count; i++)
        {
            //TODO how do new membersets get added
            memberSet[names[i]].Domain = memberSet[names[i]].CalculateDOM(val);
        }
    }

    public float DeFuzzifyMaxAv()
    {
        float bottom = 0.0f;
        float top = 0.0f;

        for (int i = 0; i < names.Count; i++)
        {
            bottom += memberSet[names[i]].Domain;

            top += memberSet[names[i]].RepresentativeValue * memberSet[names[i]].Domain;
        }

//        if (FuzzySet.IsEqual(0f, bottom))
//            return 0.0f;
		if (float.IsNaN(0f / bottom)) 
		return 0.0f;
		else
        	return top / bottom;
    }

    public float DefuzzifyCentroid(int numSamples)
    {
        //Not Used
        return 1f;
    }
}
