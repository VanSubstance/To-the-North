using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasicEditorManager : MonoBehaviour
{
    [SerializeField]
    private Button btnMainStartNew, btnMainStartLoad, btnMainExit;
    [SerializeField]
    private Transform TfMainMenu;
    private KeyToggleManager keyToggleManager;
    [SerializeField]
    private List<string> keys;
    [SerializeField]
    private List<AIContentModalContentController> modalPrefabs;
    private List<AIContentModalContentController> modals;
    // Start is called before the first frame update
    void Start()
    {
        Transform keyToggleManagerTf = new GameObject().transform;
        keyToggleManagerTf.SetParent(null);
        keyToggleManagerTf.gameObject.AddComponent<KeyToggleManager>();
        keyToggleManager = keyToggleManagerTf.GetComponent<KeyToggleManager>();

        btnMainStartNew.onClick.AddListener(() =>
        {
            StartGame(true);
        });
        btnMainStartLoad.onClick.AddListener(() =>
        {
            StartGame(false);
        });
        btnMainExit.onClick.AddListener(() =>
        {
            ExitGame();
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ToggleMainMenuUI(bool isHide)
    {
        TfMainMenu.gameObject.SetActive(isHide);
    }
    public abstract void StartGame(bool isGameNew);
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public KeyToggleManager GetKeyToggleManager()
    {
        return keyToggleManager;
    }

    public void InitModals()
    {
        if (keys != null && keys.Count > 0)
        {
            modals = new List<AIContentModalContentController>();
            for (int i = 0; i < modalPrefabs.Count; i++)
            {
                Transform temp = Instantiate(modalPrefabs[i].transform);
                temp.SetParent(GameObject.Find("UI").transform);
                temp.localPosition = Vector3.zero;
                temp.localScale = Vector3.one;
                modals.Add(temp.GetComponent<AIContentModalContentController>());
            }
            GetKeyToggleManager().InitKeysAndModals(
                keys,
                modals
                );
        }
    }
}
