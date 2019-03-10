using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 1f;
    public bool isVisible;
    private bool isExploded;
    void Update()
    {
        if (!GameManager.Instance.gameOver)
            transform.position = transform.position - transform.forward * speed * Time.deltaTime;

        if (!isVisible && Vector3.Distance(transform.position, PlayerCube.Instance.transform.position) < 10)
        {
            transform.EaseScale(Vector3.one, .5f);
            isVisible = true;
        }

        if (!isExploded && transform.position.z < PlayerCube.Instance.transform.position.z)
        {
            Explode();
        }
    }

    private void Explode()
    {
        isExploded = true;
        ObstacleInstantiator.Instance.InstantiateRandom();

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.AddExplosionForce(350f, PlayerCube.Instance.transform.position, 100f);
            rb.GetComponent<WallBrick>().enabled = false;
            rb.GetComponent<Collider>().enabled = false;
        }
        Wrj.Utils.Delay(1f, () => Destroy(gameObject));
    }
}
