using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 1f;
    public bool isVisible;
    public AudioClip breakAudio;
    private bool isExploded;
    private int passThroughCount = 0;
    void Start()
    {
        foreach(WallBrick brick in GetComponentsInChildren<WallBrick>())
        {
            if (brick.isPassThrough)
            {
                passThroughCount++; 
                brick.hitCallback += Hit;
            }
        }
        //Debug.Log("Total Passthrough count = " + passThroughCount);
        Wrj.Utils.Delay(.5f, () => Show());
    }

    private void Show()
    {
        transform.EaseScale(Vector3.one, .5f);
        isVisible = true;
    }

    private void Hit(WallBrick caller)
    { 
        passThroughCount--;
        caller.hitCallback -= Hit;
        //Debug.Log("Passthrough count = " + passThroughCount);
    }

    void Update()
    {
        if (!GameManager.Instance.gameOver)
            transform.position = transform.position - transform.forward * speed * Time.deltaTime;

        if (!isExploded && transform.position.z < PlayerCube.Instance.explosionPoint.position.z)
        {
            if (passThroughCount == 0)
            {
                Explode();
                GameManager.Instance.AddToScore(10);
            }
            else
                GameManager.Instance.gameOver = true;
        }
    }

    private void Explode()
    {
        isExploded = true;
        ObstacleInstantiator.Instance.InstantiateRandom();
        AudioPool.instance.PlayOneShot(breakAudio);
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
