using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    [SerializeField, Range(1, 100)] public int weight;
    public Vector3 position { get; private set; }

    private Vector3 screenPoint;

    [SerializeField]private GameObject sphereWeight;

    private LineRenderer line;
    private float distance;
    private void Start()
    {
        line=GetComponent<LineRenderer>();  
        position = transform.position;
        line.positionCount = 2;
        distance = Vector3.Distance(sphereWeight.transform.position, transform.position);
    }

    private void Update()
    {
        position = transform.position;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, sphereWeight.transform.position);
        weight = (int)(Vector3.Distance(sphereWeight.transform.position, transform.position) - distance);
        if (weight<1) { weight = 1; }
    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        curPosition.z = 0;
        transform.position = curPosition;

    }
}
