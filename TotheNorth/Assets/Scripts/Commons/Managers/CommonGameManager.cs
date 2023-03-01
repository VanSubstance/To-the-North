using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CommonGameManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    private int curStatus = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                if (fadeImage != null)
                {
                    FadeScreen(false, () =>
                    {
                        GlobalStatus.Loading.System.CommonGameManager = true;
                    });
                } else
                {
                    GlobalStatus.Loading.System.CommonGameManager = true;
                }
                curStatus = 1;
                break;
            case 1:
                if (GlobalStatus.Loading.System.isSystemLoadingDone())
                {
                    GetComponent<InfoMessageManager>().AddMessageIntoQueue(new InfoStat(GlobalStatus.curScene, InfoType.ERROR));
                    curStatus = 2;
                }
                break;
            case 2:
                break;
        }
    }

    public void MoveScene(string targetSceneName)
    {
        FadeScreen(true, () =>
        {
            GlobalStatus.resetLoading();
            GlobalStatus.curScene = targetSceneName;
            SceneManager.LoadScene(targetSceneName);
        });
    }

    public void FadeScreen(bool isFadein, System.Action actionAfter = null)
    {
        FadeObject(fadeImage.transform, isFadein, 1f, actionAfter);
    }

    public void FadeObject(Transform targetTf, bool isFadeIn, float accelSpeed, System.Action afterAction = null)
    {
        StartCoroutine(CoroutineFadeObject(targetTf, isFadeIn, accelSpeed, afterAction));
    }
    private IEnumerator CoroutineFadeObject(Transform targetTf, bool isFadeIn, float accelSpeed, System.Action afterAction = null)
    {
        float goalOpacity = isFadeIn ? 1.0f : 0.0f, curOpacity = isFadeIn ? 0.0f : 1.0f;
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
        if (afterAction != null) afterAction();
    }

    public void MoveObject(Transform targetTf, DirectionType direction, float accelAmount, float distanceToMove, System.Action afterAction = null)
    {
        StartCoroutine(CoroutineMoveObject(targetTf, direction, accelAmount, distanceToMove, afterAction));
    }
    private IEnumerator CoroutineMoveObject(Transform targetTf, DirectionType direction, float accelAmount, float distanceToMove, System.Action afterAction = null)
    {
        float cnt = 1f;
        Vector3 dirVector = Vector3.zero;
        switch (direction)
        {
            case DirectionType.UP:
                dirVector = Vector3.up;
                break;
            case DirectionType.DOWN:
                dirVector = Vector3.down;
                break;
            case DirectionType.LEFT:
                dirVector = Vector3.left;
                break;
            case DirectionType.RIGHT:
                dirVector = Vector3.right;
                break;
        }
        while (cnt > 0f)
        {
            yield return new WaitForSeconds(0.01f);
            cnt -= 0.01f * GlobalSetting.accelSpeed * accelAmount;
            if (targetTf == null) break;
            targetTf.Translate(dirVector * distanceToMove * 0.01f * GlobalSetting.accelSpeed * accelAmount);
        }
        if (afterAction != null) afterAction();
    }
}
