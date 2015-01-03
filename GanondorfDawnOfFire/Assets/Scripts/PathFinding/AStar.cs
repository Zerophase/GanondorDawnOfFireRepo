using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AStar
{
    private SparseGraph graph;
    private int source;
    private int target;
    private List<float> fCosts = new List<float>();
    private List<float> gCosts = new List<float>();
    public float CostToTarget { get { return gCosts[target]; } }

    private List<Edge> shortestPathTree = new List<Edge>();
    public List<Edge> ShortestPathTree { get { return shortestPathTree; } }

    private List<Edge> searchFrontier = new List<Edge>();

    public AStar(SparseGraph graph, int source, int target)
    {
        this.graph = graph;
        this.source = source;
        this.target = target;

        shortestPathTree.Capacity = graph.NumNodes();
        searchFrontier.Capacity = graph.NumNodes();

        for (int i = 0; i < graph.NumNodes(); i++)
        {
            shortestPathTree.Add(null);
            searchFrontier.Add(null);

            gCosts.Add(0);
            fCosts.Add(0);
        }
        
        Search();
    }

    public void Search()
    {
        IndexedPriorityQueue pq = new IndexedPriorityQueue(fCosts, graph.NumNodes());

        pq.Inset(source);
        while (!pq.Empty())
        {
            int nextClosestNode = pq.Pop();

            shortestPathTree[nextClosestNode] = searchFrontier[nextClosestNode];

            if (nextClosestNode == target)
                return;

            foreach (Edge item in graph.NodeEdges[nextClosestNode])
            {
                float hCost = Heuristic_Squared_Space.Calculate(graph, target, item.To);
                float gCost = gCosts[nextClosestNode] + item.Cost;

                if(searchFrontier[item.To] == null)
                {
                    fCosts[item.To] = gCost + hCost;
                    pq.Keys = fCosts;
                    gCosts[item.To] = gCost;
                    pq.Inset(item.To);

                    searchFrontier[item.To] = item;
                }
                else if(gCost < gCosts[item.To] && shortestPathTree[item.To] == null)
                {
                    fCosts[item.To] = gCost + hCost;
                    gCosts[item.To] = gCost;

                    pq.ChangePriority(item.To);
                    searchFrontier[item.To] = item;
                }
            }
        }
    }

    public List<int> PathToTarget()
    {
        List<int> path = new List<int>();

        if (target < 0)
            return path;

        int index = target;

        path.Add(index);

        while (index != source && shortestPathTree[index] != null)
        {
            index = shortestPathTree[index].From;
            path.Add(index);
        }

        return path;
    }
}
