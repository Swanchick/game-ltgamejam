using System;
using UnityEngine;

public class Vector3Utils
{
    public static Vector3 ReverseVector(Vector3 vector)
    {
        for (int i = 0; i < 3; i++)
        {
            vector[i] = Convert.ToBoolean(Mathf.Round(vector[i])) ? 0 : 1;
        }

        return vector;
    }

    public static Vector3 Abs(Vector3 vector)
    {
        for (int i = 0; i < 3; i++)
        {
            vector[i] = Mathf.Abs(vector[i]);
        }

        return vector;
    }

    public static bool IsNormalWall(Vector3 normal)
    {
        return Mathf.Round(normal.y) == 0;
    }
}
