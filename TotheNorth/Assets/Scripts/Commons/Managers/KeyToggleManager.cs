
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyToggleManager : MonoBehaviour
{
    [SerializeField]
    private List<string> keysToToggle;
    [SerializeField]
    private List<UnityEvent> actionsForToggle;
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
        if (keysToToggle.Count > 0)
            for (int i = 0; i < keysToToggle.Count; i++)
            {
                if (Input.GetKeyDown(keysToToggle[i]))
                {
                    actionsForToggle[i].Invoke();
                    return;
                }
            }
    }
}
