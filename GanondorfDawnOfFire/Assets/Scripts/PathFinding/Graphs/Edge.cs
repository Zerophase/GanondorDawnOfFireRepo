using UnityEngine;
using System.Collections;

public class Edge 
{
    private int from;
    public int From { get { return from; } }
    private int to;
    public int To { get { return to; } }

    private float cost;
    public float Cost { get { return cost; } set { cost = value; } }

    public Edge()
    {
        this.cost = 1.0f;
        this.from = (int)NodeTypes.INVALID;
        this.to = (int)NodeTypes.INVALID;
    }

    public Edge(int from, int to)
    {
        this.cost = 1.0f;
        this.from = from;
        this.to = to;
    }

    public Edge(int from, int to, float cost)
    {
        this.cost = cost;
        this.from = from;
        this.to = to;
    }

    public bool EqualityCheck(Edge edge)
    {
        if (edge.from == this.from && edge.to == this.to && edge.cost == this.cost)
            return true;
        else
            return false;
    }
}
