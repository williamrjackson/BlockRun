using UnityEngine;

public class DeathAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip deathAudio = null;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        AudioPool.instance.PlayOneShot(deathAudio);
    }
}
