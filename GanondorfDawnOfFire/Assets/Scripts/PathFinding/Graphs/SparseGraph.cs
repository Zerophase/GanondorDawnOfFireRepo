using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SparseGraph 
{   
    private bool digraph;
    public bool Digrapth { get { return digraph; } }

    private int nextNodeIndex;

    private List<NavGraphNode> navNodes = new List<NavGraphNode>(); //NodeType
    public List<NavGraphNode> NavNodes { get { return navNodes; } }
    private Dictionary<int, List<Edge>> nodeEdges = new Dictionary<int,List<Edge>>(); //EdgeListVector
    public Dictionary<int, List<Edge>> NodeEdges { get { return nodeEdges; } }
    public SparseGraph(bool digraph)
    {
        this.digraph = digraph;
        nextNodeIndex = 0;
    }

    private bool uniqueEdge(int from, int to) 
    {
        foreach (Edge item in nodeEdges[from])
        {
            if (item.To == to)
            {
                return false;
            }
        }

        return true;
    }

    private void cullInvalidEdges() 
    {
        for (int i = 0; i < nodeEdges.Count; i++)
        {
            foreach (Edge item in nodeEdges[i])
            {
                if (navNodes[item.To].Index == (int)NodeTypes.INVALID ||
                    navNodes[item.From].Index == (int)NodeTypes.INVALID)
                {
                    nodeEdges[i].Remove(item);
                }
            }
        }
    }

    public NavGraphNode GetNode(int index)
    {
        try
        {
            return navNodes[index];
        }
        catch (System.Exception)
        {
            
            throw;
        }
        
    }

    public Edge GetEdge(int from, int to)
    {
        foreach (Edge item in nodeEdges[from])
        {
            if (item.To == to)
                return item;
        }

        return null;
    }

    public int GetNextFreeNodeIndex() { return nextNodeIndex; }

    public int AddNode(NavGraphNode node)
    {
        if (node.Index < navNodes.Count)
        {
            navNodes[node.Index] = node;
            return nextNodeIndex;
        }
        else
        {
            navNodes.Add(node);
            nodeEdges.Add(nextNodeIndex, new List<Edge>());
            return nextNodeIndex++;
        }
    }

    public void RemoveNode(int index)
    {
        navNodes[index].Index = (int)NodeTypes.INVALID;

        if (!digraph)
        {
            foreach (Edge item in nodeEdges[index])
            {
                foreach (Edge edge in nodeEdges[item.To])
                {
                    if (edge.To == index)
                    {
                        nodeEdges[edge.To].Remove(edge);
                        break;
                    }
                }
            }

            nodeEdges[index].Clear();
        }
        else
        {
            cullInvalidEdges();
        }
    }

    public void AddEdge(Edge edge)
    {
        if(navNodes[edge.To].Index != (int)NodeTypes.INVALID &&
            navNodes[edge.From].Index != (int)NodeTypes.INVALID)
        {
            if (uniqueEdge(edge.From, edge.To))
                nodeEdges[edge.From].Add(edge);
        }

        //adds an edge going back in the other direction
        if(!digraph)
        {
            if(uniqueEdge(edge.To, edge.From))
            {
                Edge newEdge = new Edge(edge.To, edge.From);
                nodeEdges[edge.To].Add(newEdge);
            }
        }
        
    }

    public void RemoveEdge(int from, int to)
    {
        if (!digraph)
        {
            foreach (Edge item in nodeEdges[to])
            {
                if(item.To == from)
                {
                    nodeEdges[to].Remove(item);
                    break;
                }
            }
        }

        foreach (Edge item in nodeEdges[from])
        {
            if (item.To == to)
            {
                nodeEdges[from].Remove(item);
                break;
            }
        }
    }

    public void SetEdgeCost(int from, int to, float cost)
    {
        foreach (Edge item in nodeEdges[from])
        {
            if (item.To == to)
            {
                item.Cost = cost;
                break;
            }
        }
    }

    public int NumNodes() { return navNodes.Count; }

    public int NumActiveNodes()
    {
        int count = 0;

        for (int i = 0; i < navNodes.Count; i++)
        {
            if (navNodes[i].Index != (int)NodeTypes.INVALID)
                count++;
        }

        return count;
    }

    public int NumEdges()
    {
        int count = 0;
        for (int i = 0; i < nodeEdges.Count; i++)
        {
            count += nodeEdges[i].Count;
        }

        return count;
    }

    public bool IsEmpty()
    {
        if (navNodes.Count == 0)
            return true;
        else
            return false;
    }

    public bool IsNodePresent(int index)
    {
        if (index >= navNodes.Count || navNodes[index].Index == (int)NodeTypes.INVALID)
            return false;
        else
            return true;
    }

    
    public bool IsEdgePresent(int from, int to)
    {
        if (IsNodePresent(from) && IsNodePresent(to))
        {
            foreach (Edge item in nodeEdges[from])
            {
                if (item.To == to)
                    return true;
            }

            return false;
        }
        else
            return false;
    }

    public void Clear()
    {
        nextNodeIndex = 0;
        navNodes.Clear();
        nodeEdges.Clear();
    }

    public void RemoveEdges()
    {
        for (int i = 0; i < nodeEdges.Count; i++)
        {
            nodeEdges[i].Clear();
        }
    }
}


