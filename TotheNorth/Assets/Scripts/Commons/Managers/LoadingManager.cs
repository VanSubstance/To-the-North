using System.Collections;
using Assets.Scripts.Components.Progress.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Commons.Managers
{
    internal class LoadingManager : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarController progressTf;

        private void Start()
        {
            StartCoroutine(CoroutineLoadSceneWithAsync());
        }

        private IEnumerator CoroutineLoadSceneWithAsync()
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(GlobalStatus.nextScene);
            GlobalStatus.curScene = GlobalStatus.nextScene;
            GlobalStatus.nextScene = string.Empty;
            GlobalStatus.Loading.System.CommonGameManager = false;
            op.allowSceneActivation = true;
            while (!op.isDone)
            {
                progressTf.SetProgress(op.progress);
                yield return null;
            }
        }
    }
}
