using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings CubesSpawner")]
    [SerializeField] private CubeSpawner _cubeSpawnerX;
    [SerializeField] private CubeSpawner _cubeSpawnerZ;
    [SerializeField,Min(0f)] private float _speed;
    private CubesSpawner _cubesSpawner;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _cubesSpawner = new CubesSpawner(speed : _speed ,cubeSpawnerX: _cubeSpawnerX,cubeSpawnerZ: _cubeSpawnerZ);
        // Initialize all objects in the scene
    }

    private void Update()
    {
        EventCubesHandler.OnMove?.Invoke();
        if (Input.GetButtonUp("Fire1"))
            _cubesSpawner?.SpawnNewCube();
    }
}
