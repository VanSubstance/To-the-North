using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToMoveController : MonoBehaviour
{
    [SerializeField]
    private Transform tfToMove;
    [SerializeField]
    private Vector2 unitToBoxColliderForce = Vector2.zero;
    private Vector3 correctionDistance;
    private int z;
    // Start is called before the first frame update
    void Start()
    {
        if (unitToBoxColliderForce.Equals(Vector2.zero))
        {
            GetComponent<BoxCollider>().size = GetComponent<RectTransform>().sizeDelta;
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
        correctionDistance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - tfToMove.position;
        z = (int)tfToMove.localPosition.z;
    }

    private void OnMouseDrag()
    {
        tfToMove.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - correctionDistance;
        tfToMove.localPosition = new Vector3(
            tfToMove.localPosition.x,
            tfToMove.localPosition.y,
            z
            );
    }

    private void OnMouseUp()
    {
        correctionDistance = Vector3.zero;
        z = 0;
    }
}
