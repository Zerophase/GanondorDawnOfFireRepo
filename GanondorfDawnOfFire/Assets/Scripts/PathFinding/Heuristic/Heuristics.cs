using UnityEngine;
using System.Collections;

public interface Heuristics 
{

}

public class Heuristic_Squared_Space : Heuristics
{
    public static float Calculate(SparseGraph g, int index1, int index2)
    {
       return vec3DDistanceSQ(g.GetNode(index1).Position, g.GetNode(index2).Position);
    }

    public static float vec3DDistanceSQ(Vector3 v1, Vector3 v2)
    {
        float ySeperation = v2.y - v1.y;
        float xSeperation = v2.x - v1.x;
        float zSeperation = v2.z - v1.z;

        return ySeperation * ySeperation + xSeperation * xSeperation + zSeperation * zSeperation;
    }
}
