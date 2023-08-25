using UnityEngine;

public class CubeManager : MonoBehaviour, IManagerPrimitives
{
    private float _speed;
    private Vector3 _direction;
    public Vector3 Size => transform.localScale;
    public DirectionCube DirectionCubeType { get; private set; }
    public Color Color { get; private set; }

    private void Awake()
    {
        Color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Renderer>().material.color = Color;
    }

    void IManagerPrimitives.Initialize(float speed,DirectionCube directionCube)
    {
        this._speed = speed;
        EventCubesHandler.OnMove += OnMove;
        DirectionCubeType = directionCube;
        switch (directionCube)
        {
            case DirectionCube.XDirection:
                _direction = transform.right;
                break;
            case DirectionCube.ZDirection:
                _direction = transform.forward;
                break;
        }
    }

    private void OnDisable()
    {
        EventCubesHandler.OnMove -= OnMove;
    }

    private void OnMove()
    {
        transform.position += _direction * Time.deltaTime * _speed;
    }

    public bool Split(CubeManager pastCube)
    {
        EventCubesHandler.OnMove -= OnMove;
        return pastCube == null || CubesSplitter.SplitTwoCubes(pastCube, this);
    }
}

public interface IManagerPrimitives
{
    internal void Initialize(float speed, DirectionCube directionCube);
}

public enum DirectionCube
{
    XDirection,
    ZDirection
}
