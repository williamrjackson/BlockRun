using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    public static PlayerCube Instance;
    public Transform explosionPoint;
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
        if (explosionPoint == null)
        {
            explosionPoint = transform;
        }
    }

    void Update()
    {
        if (GameManager.Instance.gameOver && !isExploded)
        {
            isExploded = true;
            Wrj.Utils.MapToCurve.StopAllOnTransform(transform);
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(500f, explosionPoint.position, 100f);
                //rb.GetComponent<Collider>().enabled = false;
            }
            Wrj.Utils.Delay(3f, () => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
        }
    }
}
