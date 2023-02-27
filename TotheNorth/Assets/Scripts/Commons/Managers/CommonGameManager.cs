using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommonGameManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    private IEnumerator CoroutineFadeScreen(bool isFadeIn)
    {
        float goalOpacity = isFadeIn ? 1.0f : 0.0f, curOpacity = isFadeIn ? 0.0f : 1.0f;
        GlobalStatus.isInFade = true;
        while (isFadeIn ? curOpacity < goalOpacity : curOpacity > goalOpacity)
        {
            yield return new WaitForSeconds(0.01f);
            curOpacity = curOpacity + 0.01f * (GlobalSetting.accelSpeed * (isFadeIn ? 1f : -1f));
            fadeImage.color = new Color(0f, 0f, 0f, curOpacity);
        }
        GlobalStatus.isInFade = false;
    }
    private void FadeScreen(bool isFadeIn)
    {
        if (GlobalStatus.isInFade)
        {
            // 이미 작동중인 페이드 관련 기능이 있다
            // => 현재 페이드 기능은 작동하지 않는다
            // 에러 메세지 출력 필요
            return;
        }
        StartCoroutine(CoroutineFadeScreen(isFadeIn));
    }
    private void TryChangeScene(string trargetSceneName)
    {

    }
    private void TryPrintErr(ErrorType errType, string errStr)
    {

    }
}
