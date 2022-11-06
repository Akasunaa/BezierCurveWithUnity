using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    [SerializeField, Range(1, 100)] public int weight;
    public Vector3 position { get; private set; }

    private Vector3 screenPoint;

    private void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        position = transform.position;

    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        curPosition.z = 0;
        transform.position = curPosition;

    }
}
