using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CubeSpawner : MonoBehaviour,ISpawner
{
    [SerializeField]private CubeManager prefabCube;
    private CubesSpawner _cubesSpawner;
    private DirectionCube _directionCube;

    void ISpawner.Initialize(CubesSpawner cubesSpawner,DirectionCube directionCube)
    {
        this._cubesSpawner = cubesSpawner;
        this._directionCube = directionCube;
    }

    CubeManager ISpawner.CreateCube(CubeManager currentCube)
    {
        var cube = Instantiate(prefabCube);
        cube.transform.localScale =
            new Vector3(_cubesSpawner.CurrentCube.Size.x, prefabCube.Size.y, _cubesSpawner.CurrentCube.Size.z);
        float ySize = _cubesSpawner.CurrentCube.Size.y/2f +
                           _cubesSpawner.CurrentCube.transform.position.y +
                           cube.Size.y/2f;
        Vector3 positionCube;
        if (_directionCube == DirectionCube.XDirection)
            positionCube = new Vector3(transform.position.x,0,currentCube.transform.position.z);
        else
            positionCube = new Vector3(currentCube.transform.position.x,0,transform.position.z);
        positionCube.y = ySize;
        cube.transform.position = positionCube;
        ((IManagerPrimitives)cube).Initialize(_cubesSpawner.Speed, _directionCube);
        return cube;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position,prefabCube.Size);
    }
}

public interface ISpawner
{
    internal CubeManager CreateCube(CubeManager currentCube);
    internal void Initialize(CubesSpawner cubesSpawner,DirectionCube directionCube);
}
