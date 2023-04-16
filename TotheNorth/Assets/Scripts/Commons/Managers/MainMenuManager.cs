using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Commons
{
    class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private Transform fadeImageTf;
        private Image fadeImage;

        private void Awake()
        {
            fadeImage = fadeImageTf.GetComponent<Image>();
        }

        private void Start()
        {
            FadeScreen(false);
        }

        public void MoveScene(string targetSceneName)
        {
            FadeScreen(true, () =>
            {
                GlobalStatus.resetLoading();
                GlobalStatus.nextScene = targetSceneName;
                SceneManager.LoadScene("Loading");
            });
        }

        /// <summary>
        /// 화면 페이드 인/아웃
        /// </summary>
        /// <param name="isFadein">true -> 화면 감추기 (페이드인); false -> 화면 나타내기 (페이드아웃)</param>
        /// <param name="actionAfter">화면 애니메이션 이후 실행할 함수</param>
        /// <param name="actionBefore">화면 애니메이션 이전 실행할 함수</param>
        public void FadeScreen(bool isFadein, System.Action actionAfter = null, System.Action actionBefore = null)
        {
            FadeObject(fadeImage.transform, isFadein, 1f, actionAfter, actionBefore);
        }

        public void FadeObject(Transform targetTf, bool isFadeIn, float accelSpeed, System.Action afterAction = null, System.Action actionBefore = null)
        {
            StartCoroutine(CoroutineFadeObject(targetTf, isFadeIn, accelSpeed, afterAction, actionBefore));
        }
        private IEnumerator CoroutineFadeObject(Transform targetTf, bool isFadeIn, float accelSpeed, System.Action afterAction = null, System.Action actionBefore = null)
        {
            float goalOpacity = isFadeIn ? 1.0f : 0.0f, curOpacity = isFadeIn ? 0.0f : 1.0f;
            if (actionBefore != null) actionBefore();
            targetTf.SetAsLastSibling();
            while (isFadeIn ? curOpacity < goalOpacity : curOpacity > goalOpacity)
            {
                yield return new WaitForSeconds(0.01f);
                curOpacity = curOpacity + (0.01f * (GlobalSetting.accelSpeed * (isFadeIn ? 1f : -1f)) * accelSpeed);
                if (targetTf == null) break;
                if (targetTf.GetComponent<Image>() != null) targetTf.GetComponent<Image>().color = new Color(
                    targetTf.GetComponent<Image>().color.r,
                    targetTf.GetComponent<Image>().color.g,
                    targetTf.GetComponent<Image>().color.b,
                    curOpacity);
                if (targetTf == null) break;
                if (targetTf.GetComponent<TextMeshProUGUI>() != null) targetTf.GetComponent<TextMeshProUGUI>().color = new Color(
                    targetTf.GetComponent<TextMeshProUGUI>().color.r,
                    targetTf.GetComponent<TextMeshProUGUI>().color.g,
                    targetTf.GetComponent<TextMeshProUGUI>().color.b,
                    curOpacity);
            }
            targetTf.SetAsFirstSibling();
            if (afterAction != null) afterAction();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
