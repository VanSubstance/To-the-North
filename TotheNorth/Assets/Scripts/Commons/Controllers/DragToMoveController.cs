using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToMoveController : MonoBehaviour
{
    [SerializeField]
    private Transform tfToMove;
    [SerializeField]
    private Vector2 unitToBoxColliderForce = Vector2.zero;
    private Vector3 correctionDistance;
    private int z;
    private Vector2 objSize;
    // Start is called before the first frame update
    void Start()
    {
        objSize = GetComponent<RectTransform>().sizeDelta;
        if (unitToBoxColliderForce.Equals(Vector2.zero))
        {
            GetComponent<BoxCollider>().size = objSize;
        }
        else
        {
            GetComponent<BoxCollider>().size = unitToBoxColliderForce * GlobalSetting.unitSize;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (tfToMove.tag != "Item")
        {
            correctionDistance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - tfToMove.position;
            z = (int)tfToMove.localPosition.z;
        }
    }

    private void OnMouseDrag()
    {
        tfToMove.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - correctionDistance;
        if (tfToMove.tag == "Item")
        {
            tfToMove.localPosition = new Vector3(
                tfToMove.localPosition.x - (objSize.x / 2),
                tfToMove.localPosition.y + (objSize.y / 2),
                z
                );
        }
        else
        {
            tfToMove.localPosition = new Vector3(
                tfToMove.localPosition.x,
                tfToMove.localPosition.y,
                z
                );
        }
    }

    private void OnMouseUp()
    {
        correctionDistance = Vector3.zero;
    }
}
