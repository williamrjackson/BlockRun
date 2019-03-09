using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField]
    private BoxCollider playerCube;
    [SerializeField]
    [Range(.1f, 1f)]
    private float speed = .25f;
    [SerializeField]
    AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool locked;
    private Transform originParent;
    private Transform rotator;
    private Queue<UnityAction> queuedMoves = new Queue<UnityAction>();
    private LayerMask mask;

    void Start()
    {
        if (playerCube == null)
        {
            playerCube = GetComponentInChildren<BoxCollider>();
        }
        mask = LayerMask.GetMask(LayerMask.LayerToName(playerCube.gameObject.layer));
        rotator = new GameObject().transform;
        rotator.parent = transform;
        rotator.name = "_rotator";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            queuedMoves.Enqueue(Forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            queuedMoves.Enqueue(Back);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            queuedMoves.Enqueue(Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            queuedMoves.Enqueue(Left);
        }

        if (!locked && queuedMoves.Count > 0)
        {
            queuedMoves.Dequeue().Invoke();
        }
    }

    private void Forward()
    {
        if (locked)
            return;

        locked = true;
        originParent = transform.parent;
        Vector3 pivot = Vector3.zero;
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCube.transform.position + Vector3.forward * 10, Vector3.back, out hitInfo, 10f, mask))
        {
            pivot.z = hitInfo.point.z;
        }
        if (Physics.Raycast(playerCube.transform.position + Vector3.down * 10, Vector3.up, out hitInfo, 10f, mask))
        {
            pivot.y = hitInfo.point.y;
        }
        rotator.parent = null;
        rotator.position = pivot;
        transform.parent = rotator;
        StartCoroutine(Move(new Vector3(90f, 0f, 0f)));
    }

    private void Back()
    {
        if (locked)
            return;

        locked = true;
        originParent = transform.parent;
        Vector3 pivot = Vector3.zero;
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCube.transform.position + Vector3.back * 2, Vector3.forward, out hitInfo, 10f, mask))
        {
            pivot.z = hitInfo.point.z;
        }
        if (Physics.Raycast(playerCube.transform.position + Vector3.down * 2, Vector3.up, out hitInfo, 10f, mask))
        {
            pivot.y = hitInfo.point.y;
        }
        rotator.parent = null;
        rotator.position = pivot;
        transform.parent = rotator;
        StartCoroutine(Move(new Vector3(-90f, 0f, 0f)));
    }

    private void Right()
    {
        if (locked)
            return;

        locked = true;
        originParent = transform.parent;
        Vector3 pivot = Vector3.zero;
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCube.transform.position + Vector3.right * 2, Vector3.left, out hitInfo, 10f, mask))
        {
            pivot.x = hitInfo.point.x;
        }
        if (Physics.Raycast(playerCube.transform.position + Vector3.down * 2, Vector3.up, out hitInfo, 10f, mask))
        {
            pivot.y = hitInfo.point.y;
        }
        rotator.parent = null;
        rotator.position = pivot;
        transform.parent = rotator;
        StartCoroutine(Move(new Vector3(0f, 0f, -90f)));
    }

    private void Left()
    {
        if (locked)
            return;

        locked = true;
        originParent = transform.parent;
        Vector3 pivot = Vector3.zero;
        RaycastHit hitInfo;
        if (Physics.Raycast(playerCube.transform.position + Vector3.left * 2, Vector3.right, out hitInfo, 10f, mask))
        {
            pivot.x = hitInfo.point.x;
        }
        if (Physics.Raycast(playerCube.transform.position + Vector3.down * 2, Vector3.up, out hitInfo, 10f, mask))
        {
            pivot.y = hitInfo.point.y;
        }
        rotator.parent = null;
        rotator.position = pivot;
        transform.parent = rotator;
        StartCoroutine(Move(new Vector3(0f, 0f, 90f)));
    }

    private IEnumerator Move(Vector3 rotation)
    {
        if (!locked)
        {
            yield break;
        }
        float elapsedTime = 0f;
        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;
            float lerpTime = curve.Evaluate(Mathf.InverseLerp(0, speed, elapsedTime));
            rotator.eulerAngles = Vector3.Lerp(Vector3.zero, rotation, lerpTime);
            yield return null;
        }

        transform.parent = originParent;
        rotator.parent = transform;
        rotator.eulerAngles = Vector3.zero;
        locked = false;
    }
}
