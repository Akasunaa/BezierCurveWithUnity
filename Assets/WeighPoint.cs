using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighPoint : MonoBehaviour
{
    private Vector3 screenPoint;
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        curPosition.z = 0;
        transform.position = curPosition;

    }
}
