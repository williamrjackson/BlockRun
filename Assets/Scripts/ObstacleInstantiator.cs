using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInstantiator : MonoBehaviour
{
    public static ObstacleInstantiator Instance;

    [SerializeField]
    private Wall[] walls;
    private float speed = 5f;

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
        Wall newWall = Instantiate(walls[Random.Range(0, walls.Length)]);
        newWall.speed = Mathf.Min(speed += .1f, 10f);
        newWall.transform.localScale = Vector3.zero;
        newWall.transform.position = transform.position;

    }
}
