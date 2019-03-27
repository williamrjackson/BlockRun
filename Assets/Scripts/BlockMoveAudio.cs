using UnityEngine;

public class BlockMoveAudio : MonoBehaviour
{
    [SerializeField]
    private BlockMovement blockMovement = null;
    [SerializeField]
    private AudioClip[] blockFallAudio = null;

    void Start()
    {
        blockMovement.OnBlockMove += BlockMoved;
    }

    private void BlockMoved()
    {
        AudioPool.instance.PlayOneShot(blockFallAudio[Random.Range(0, blockFallAudio.Length)]);
    }
}
