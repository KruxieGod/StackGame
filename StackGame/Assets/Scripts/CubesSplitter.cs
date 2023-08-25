using System.Globalization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

internal static class CubesSplitter
{
    public static bool SplitTwoCubes(CubeManager past,CubeManager current)
    {
        return current.DirectionCubeType == DirectionCube.ZDirection
            ? SplitZAxis(past, current)
            : SplitXAxis(past, current);
    }

    private static bool SplitXAxis(CubeManager past, CubeManager current)
    {
        Debug.Log("X");
        Func<Vector3, float> funcX = vector => vector.x;
        Func<float, Vector3, Vector3> createNewVectorX = (x, vector) => new Vector3(x,vector.y,vector.z);
        return Split(past,current ,funcX,createNewVectorX);
    }
    
    private static bool SplitZAxis(CubeManager past, CubeManager current)
    {
        Func<Vector3, float> funcZ = vector => vector.z;
        Func<float, Vector3, Vector3> createNewVectorZ = (z, vector) => new Vector3(vector.x,vector.y,z);
        return Split(past,current,funcZ,createNewVectorZ);
    }

    private static bool Split(CubeManager past, CubeManager current,Func<Vector3,float> funcBetweenXZ, Func<float,Vector3,Vector3> createNewVectorXZ)
    {
        var currentTransform = current.transform;
        var currentPosition = currentTransform.position;
        float difference = funcBetweenXZ(past.transform.position)-funcBetweenXZ(currentPosition);
        int direction = difference > 0 ? 1 : -1;
        difference = Mathf.Abs(difference);
        if (difference > funcBetweenXZ(current.Size))
        {
            current.AddComponent<Rigidbody>();
            return false;
        }
        Vector3 scale = createNewVectorXZ(funcBetweenXZ(current.Size) - difference, current.Size);
        float remains = (funcBetweenXZ(current.Size) - funcBetweenXZ(scale))/2f;
        currentTransform.position = createNewVectorXZ(funcBetweenXZ(currentPosition) + remains*direction,currentPosition);
        CreateRemainCube(current, scale, direction,funcBetweenXZ,createNewVectorXZ);
        currentTransform.localScale = scale;
        return true;
    }

    private static void CreateRemainCube(CubeManager currentCube, Vector3 scale, int direction,Func<Vector3,float> funcBetweenXZ, Func<float,Vector3,Vector3> createNewVectorXZ)
    {
        var current = currentCube.transform;
        Vector3 remainScale = createNewVectorXZ(funcBetweenXZ(current.localScale) - funcBetweenXZ(scale), current.localScale);
        var remainCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        remainCube.GetComponent<Renderer>().material.color = currentCube.Color;
        remainCube.transform.localScale = remainScale;
        remainCube.transform.position =
            createNewVectorXZ(funcBetweenXZ(current.position) - direction * (funcBetweenXZ(scale) / 2f + funcBetweenXZ(remainScale) / 2f),
                current.position);
        remainCube.AddComponent<Rigidbody>();
        UnityEngine.Object.Destroy(remainCube,4f);
    }
}