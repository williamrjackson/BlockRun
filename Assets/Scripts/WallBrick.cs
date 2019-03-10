using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBrick : MonoBehaviour
{
    // Start is called before the first frame update
    private Wall parentWall;

    void Start()
    {
        parentWall = GetComponentInParent<Wall>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (parentWall == null || !parentWall.isVisible || GameManager.Instance.gameOver)
        {
            return;
        }

        GameManager.Instance.gameOver = true;
    }
}
