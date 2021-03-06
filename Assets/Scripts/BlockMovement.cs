﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField]
    private BoxCollider playerCube;
    [SerializeField]
    [Range(.1f, 1f)]
    private float rotateDuration = .25f;
    [SerializeField]
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField]
    private TouchAxisCtrl swipe = null;

    public delegate void BlockMoveDelegate();
    public BlockMoveDelegate OnBlockMove;

    private bool locked;
    private Queue<System.Action> queuedMoves = new Queue<System.Action>();

    void Start()
    {
        if (playerCube == null)
        {
            playerCube = GetComponentInChildren<BoxCollider>();
        }
        swipe.OnSwipe += OnSwipe;
    }

    private void OnSwipe(TouchAxisCtrl.Direction direction)
    {
        switch (direction)
        {
            case TouchAxisCtrl.Direction.Up:
                queuedMoves.Enqueue(() => MoveOnPivot(Vector3.forward));
                break;
            case TouchAxisCtrl.Direction.Down:
                queuedMoves.Enqueue(() => MoveOnPivot(Vector3.back));
                break;
            case TouchAxisCtrl.Direction.Right:
                queuedMoves.Enqueue(() => MoveOnPivot(Vector3.right));
                break;
            default:
                queuedMoves.Enqueue(() => MoveOnPivot(Vector3.left));
                break;
        }
    }

    void Update()
    {
        if (GameManager.Instance.gameOver)
            return;

        // Add movement to the queue
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            queuedMoves.Enqueue(() => MoveOnPivot(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            queuedMoves.Enqueue(() => MoveOnPivot(Vector3.back));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            queuedMoves.Enqueue(() => MoveOnPivot(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            queuedMoves.Enqueue(() => MoveOnPivot(Vector3.left));
        }

        // If there's a movement to make, and one's not currently running, make the move
        if (!locked && queuedMoves.Count > 0)
        {
            queuedMoves.Dequeue().Invoke();
        }
    }
    private void MoveOnPivot(Vector3 direction)
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

    // Rotate by the requested duration over "rotateDuration" time
    private IEnumerator Move(Vector3 axis, Vector3 point)
    {
        Vector3 originalPos = transform.position;
        Quaternion originalRot = transform.rotation;

        if (OnBlockMove != null)
            OnBlockMove();

        if (!locked)
        {
            yield break;
        }
        float elapsedTime = 0f;
        while (elapsedTime < rotateDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpTime = curve.Evaluate(Mathf.InverseLerp(0f, rotateDuration, elapsedTime));
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
