using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField]
    private BoxCollider playerCube;
    [SerializeField]
    [Range(.1f, 1f)]
    private float speed = .25f;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField]
    private AudioClip[] blockFall = null;

    private bool locked;
    private Transform originParent;
    private Transform rotator;
    private Queue<System.Action> queuedMoves = new Queue<System.Action>();

    void Start()
    {
        if (playerCube == null)
        {
            playerCube = GetComponentInChildren<BoxCollider>();
        }
    }

    void Update()
    {
        if (GameManager.Instance.gameOver)
            return;

        // Add movement to the queue
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            queuedMoves.Enqueue(() => PivotForMove(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            queuedMoves.Enqueue(() => PivotForMove(Vector3.back));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            queuedMoves.Enqueue(() => PivotForMove(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            queuedMoves.Enqueue(() => PivotForMove(Vector3.left));
        }

        // If there's a movement to make, and one's not currently running, make the move
        if (!locked && queuedMoves.Count > 0)
        {
            queuedMoves.Dequeue().Invoke();
        }
    }
    private void PivotForMove(Vector3 direction)
    {
        // ensure the object isn't alread moving
        if (locked)
            return;

        // set to locked state to prevent other moves from running until we're done
        locked = true;

        // Get the pivot: the farthest point at the bottom toward the direction requested. 
        Vector3 pivot = playerCube.ClosestPointOnBounds(transform.position + direction.normalized * 10f + Vector3.down * 10f) ;
        
        StartCoroutine(Move(Vector3.Cross(direction, Vector3.down), pivot));
    }

    // Rotate rotator object by the requested duration over "speed" time
    private IEnumerator Move(Vector3 axis, Vector3 point)
    {
        Vector3 originalPos = transform.position;
        Quaternion originalRot = transform.rotation;

        AudioPool.instance.PlayOneShot(blockFall[Random.Range(0, blockFall.Length)]);
        if (!locked)
        {
            yield break;
        }
        float elapsedTime = 0f;
        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;
            float lerpTime = curve.Evaluate(Mathf.InverseLerp(0f, speed, elapsedTime));
            transform.position = originalPos;
            transform.rotation = originalRot;
            transform.RotateAround(point, axis, 90f * lerpTime);
            yield return null;
        }
        transform.position = originalPos;
        transform.rotation = originalRot;
        transform.RotateAround(point, axis, 90);

        locked = false;
    }
}
