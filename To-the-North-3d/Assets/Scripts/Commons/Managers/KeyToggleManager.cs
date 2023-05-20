using System;
using Assets.Scripts.Commons;
using UnityEngine;

public class KeyToggleManager : MonoBehaviour
{
    public KeyCode keyToToggle;
    private IControllByKey modalToControll;

    public bool IsOpen
    {
        get
        {
            return modalToControll.IsOpen();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        TrackKeys();
    }

    private void TrackKeys()
    {
        try
        {
            if (Input.GetKeyDown(keyToToggle))
            {
                if (keyToToggle == KeyCode.Escape &&
                    UIManager.Instance.IsClosedInForce
                    )
                {
                    UIManager.Instance.IsClosedInForce = false;
                    return;
                }
                modalToControll.ControllByKey(0);
                return;
            }
        }
        catch (NullReferenceException)
        {
            // 키 할당 안됨
        }
    }

    public void CloseInForce()
    {
        modalToControll.Close();
    }

    public void InitContent(KeyCode _keyToToggle, IControllByKey objectToControll)
    {
        keyToToggle = _keyToToggle;
        modalToControll = objectToControll;
    }
}
