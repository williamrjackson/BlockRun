using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInstantiator : MonoBehaviour
{
    public static ObstacleInstantiator Instance;

    [SerializeField]
    Wall[] walls;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        InstantiateRandom();
    }

    // Update is called once per frame
    public void InstantiateRandom()
    {
        Transform t = Instantiate(walls[Random.Range(0, walls.Length)]).transform;
        t.localScale = Vector3.zero;
        t.position = transform.position;
    }
}
