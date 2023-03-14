using System.Collections;
using Assets.Scripts.Components.Progress.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Commons.Managers
{
    internal class LoadingManager : MonoBehaviour
    {
        public string sceneName;
        [SerializeField]
        private ProgressBarController progressTf;

        private float time;

        private void Start()
        {
            StartCoroutine(CoroutineLoadSceneWithAsync());
        }

        private IEnumerator CoroutineLoadSceneWithAsync()
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            while (!op.isDone)
            {
                time += Time.deltaTime;
                progressTf.SetProgress(time / 10f);
                if (time > 10)
                {
                    op.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
