using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSpawner
{
    public float Speed { get; }

    private readonly ISpawner _cubeSpawnerZ;
    private readonly ISpawner _cubeSpawnerX;
    public CubeManager CurrentCube { get; private set; }
    public CubeManager PastCube { get; private set; }
    private int _index = 0;

    public CubesSpawner(float speed,CubeSpawner cubeSpawnerZ,CubeSpawner cubeSpawnerX)
    {
        Speed = speed;
        _cubeSpawnerX = cubeSpawnerX;
        _cubeSpawnerZ = cubeSpawnerZ;
        CurrentCube = Object.FindObjectOfType<CubeManager>();
        _cubeSpawnerX.Initialize(this,DirectionCube.XDirection);
        _cubeSpawnerZ.Initialize(this,DirectionCube.ZDirection);
        SpawnNewCube();
    }

    internal void SpawnNewCube()
    {
        var spawners = new List<ISpawner>
        {
            _cubeSpawnerZ,
            _cubeSpawnerX
        };
        if (!CurrentCube.Split(PastCube)) return;
        PastCube = CurrentCube;
        CurrentCube = spawners[_index++%spawners.Count].CreateCube(CurrentCube);
    }
}


