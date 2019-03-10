using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    public static PlayerCube Instance;

    private bool isExploded;
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

    void Update()
    {
        if (GameManager.Instance.gameOver && !isExploded)
        {
            isExploded = true;

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.AddExplosionForce(350f, PlayerCube.Instance.transform.position, 100f);
                rb.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
