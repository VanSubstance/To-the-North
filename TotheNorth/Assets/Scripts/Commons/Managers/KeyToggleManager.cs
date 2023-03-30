using UnityEngine;

public class KeyToggleManager : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyToToggle;
    [SerializeField]
    private MonoBehaviourControllByKey modalToControll;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TrackKeys();
    }

    private void TrackKeys()
    {
        if (keyToToggle != KeyCode.None)
            if (Input.GetKeyDown(keyToToggle))
            {
                modalToControll.ControllByKey(0);
                return;
            }
    }
}
