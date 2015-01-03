using UnityEngine;
using System.Collections;

enum NodeTypes { INVALID = -1};

public abstract class Node  
{
    private int index;
    public int Index { get { return index; } set { index = value; } }
    static int indexIterator = 0;

    public Node()
    {
        this.index = indexIterator++;
    }

	public Node(int index)
    {
        this.index = index;
        indexIterator++;
    }
}

public class NavGraphNode : Node
{
    private Vector3 position;
    public Vector3 Position { get { return position; } }

    public NavGraphNode(Vector3 pos)
        : base()
    {
        this.position = pos;
    }
    public NavGraphNode(int index, Vector3 pos)
        : base(index)
    {
        this.position = pos;
    }
}
