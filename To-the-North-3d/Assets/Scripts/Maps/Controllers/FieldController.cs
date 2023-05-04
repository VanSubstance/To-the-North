using UnityEngine;

public class FieldController : MonoBehaviour
{
    private void Awake()
    {
        transform.Rotate(90, 0, 0);
        Camera.main.transparencySortAxis = new Vector3(0f, -1f, 1f);
    }
}
