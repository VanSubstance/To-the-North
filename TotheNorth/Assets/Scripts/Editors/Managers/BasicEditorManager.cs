using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEditorManager : MonoBehaviour
{
    [SerializeField]
    private Button btnMainStartNew, btnMainStartLoad, btnMainExit;
    [SerializeField]
    private Transform TfMainMenu;
    // Start is called before the first frame update
    void Start()
    {
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

    }
    public virtual void StartGame(bool isGameNew)
    {
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
