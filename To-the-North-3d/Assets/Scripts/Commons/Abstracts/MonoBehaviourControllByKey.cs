using Assets.Scripts.Commons;
using UnityEngine;

public abstract class MonoBehaviourControllByKey : MonoBehaviour, IControllByKey
{
    [SerializeField]
    private KeyCode keyToToggle;

    protected void Start()
    {
        if (keyToToggle != KeyCode.None)
        {
            UIManager.Instance.gameObject.AddComponent<KeyToggleManager>().InitContent(keyToToggle, this);
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 열기
    /// </summary>
    public void Open()
    {
        InGameStatus.User.isPause = true;
        OnOpen();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 닫기
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
        OnClose();
        InGameStatus.User.isPause = false;
    }

    public void ControllByKey(int purpose)
    {
        bool toOpen = purpose == 1 ? true : purpose == 2 ? false :
            gameObject.activeSelf ? false : true;
        ;
        transform.SetAsLastSibling();
        if (toOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public abstract void OnOpen();

    public abstract void OnClose();
}
