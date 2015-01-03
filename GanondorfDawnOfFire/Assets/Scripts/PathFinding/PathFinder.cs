using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder 
{
    private enum NodesFound { NO_CLOSET_NODE = -1 };

    private List<int> path;
    private List<Edge> subTree;
    private List<NavGraphNode> currentNeighbor = new List<NavGraphNode>();

    SparseGraph graph;

    float range;

    float costToTarget;
    int source;
    int prevNode;

    public bool firstSearchDone = false;
    int closestNode;
    int prevClosetNode;

    public PathFinder()
    {
        path = new List<int>();
        subTree = new List<Edge>();
        graph = new SparseGraph(true);
        costToTarget = 0.0f;
        source = 0;
    }

    public void UpdateGraph(int index, Vector3 location)
    {
        if(!graph.IsNodePresent(index))
        {
            graph.AddNode(new NavGraphNode(index, location));
        }
    }

    public Vector3 CreatePathAStarDistanceSquared(Vector3 pos)
    {
        range = CalculateAverageGraphEdgeLength() + 1;
        
        AStar aStar = new AStar(graph, GetSourceNode(), GetClosestNodeToPosition(pos));

        if (!firstSearchDone)
            firstSearchDone = true;

        source = prevNode;
        path = aStar.PathToTarget();
        return graph.GetNode(path[0]).Position;
    }

    public void AddAllNeighborsToNode(int index)
    {
        for (int i = -1; i < 2; i++)
        {
            if (i == 0)
                continue;

            if (index + i < 0)
                i = 3;

            Vector3 posNode = graph.GetNode(index).Position;
            Vector3 posNeighbor;
            if (index == 3 && i == 1)
                posNeighbor = graph.GetNode(0).Position;
            else
                posNeighbor = graph.GetNode(index + i).Position;

            float dist = Heuristic_Squared_Space.vec3DDistanceSQ(posNode, posNeighbor);

            Edge edge;
            if (index == 3 && i == 1)
                edge = new Edge(index, 0, dist);
            else
                edge = new Edge(index, index + i, dist);

            if (i == 3)
                i = -1;

            graph.AddEdge(edge);
        }
    }

    public int GetSourceNode()
    {
        return source;
    }

    public int GetClosestNodeToPosition(Vector3 pos)
    {
        float closestSoFar = float.MaxValue;
        closestNode = (int)NodesFound.NO_CLOSET_NODE;
        if (currentNeighbor.Count > 0)
            currentNeighbor.Clear();

        CalculateNeighbors(pos);
        for (int i = 0; i < currentNeighbor.Count; i++)
        {
            float dist = Heuristic_Squared_Space.vec3DDistanceSQ(pos, currentNeighbor[i].Position);
            if (dist < closestSoFar && dist > 1f && currentNeighbor[i].Index != prevNode && 
                currentNeighbor[i].Index != prevClosetNode)
            {
                closestSoFar = dist;
                closestNode = currentNeighbor[i].Index;
            }
        }

        prevClosetNode = source;
        prevNode = closestNode;
        return closestNode;
    }

    public void CalculateNeighbors(Vector3 pos)
    {
        Vector3 queryBox = new Vector3(pos.x - range, range, pos.z + range);
        for (int i = 0; i < graph.NumNodes(); i++)
		{
            if (Heuristic_Squared_Space.vec3DDistanceSQ(graph.GetNode(i).Position, pos) < queryBox.sqrMagnitude)
                currentNeighbor.Add(graph.GetNode(i));
		}
    }

    public float CalculateAverageGraphEdgeLength()
    {
        float totalLength = 0f;
        int numEdgesCounted = 0;

        for (int i = 0; i < graph.NumNodes(); i++)
        {
            for (int j = 0; j < 2; j++)
			{
			    numEdgesCounted++;

                totalLength += Heuristic_Squared_Space.vec3DDistanceSQ(graph.GetNode(graph.NodeEdges[i][j].From).Position, graph.GetNode(graph.NodeEdges[i][j].To).Position);
			}
        }

        return totalLength / (float)numEdgesCounted;
    }

    public float UpdateDistanceTraveled(Vector3 pos)
    {
        return Heuristic_Squared_Space.vec3DDistanceSQ(graph.GetNode(closestNode).Position, pos);
    }
}
