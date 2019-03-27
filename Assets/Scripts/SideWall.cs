using UnityEngine;

public class SideWall : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (GameManager.Instance.gameOver)
        {
            return;
        }

        GameManager.Instance.gameOver = true;
    }
}
