using UnityEngine;

public class WallBrick : MonoBehaviour
{
    // Start is called before the first frame update
    private Wall parentWall;
    public bool isPassThrough = false;
    public delegate void RegisterHit(WallBrick caller);
    public RegisterHit hitCallback;
    private bool hasHit;
    void Start()
    {
        parentWall = GetComponentInParent<Wall>();
        if (isPassThrough)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (hasHit)
            return;

        if (isPassThrough && hitCallback != null)
        {
            hasHit = true;
            hitCallback(this);
            return;
        }

        if (parentWall == null || !parentWall.isVisible || GameManager.Instance.gameOver)
        {
            return;
        }

        GameManager.Instance.gameOver = true;
    }
}
