using System;
using UnityEngine;

public class KeyToggleManager : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyToToggle;
    [SerializeField]
    private MonoBehaviourControllByKey modalToControll;

    // Update is called once per frame
    void Update()
    {
        TrackKeys();
    }

    private void TrackKeys()
    {
        try
        {
            if (Input.GetKeyDown(keyToToggle))
            {
                modalToControll.ControllByKey(0);
                return;
            }
        }
        catch (NullReferenceException)
        {
            // 키 할당 안됨
        }
    }

    public void InitContent(KeyCode _keyToToggle, MonoBehaviourControllByKey objectToControll)
    {
        keyToToggle = _keyToToggle;
        modalToControll = objectToControll;
    }
}
