using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoMessageManager : MonoBehaviour
{
    [SerializeField]
    private Transform infoTextParentTf;
    [SerializeField]
    private TMP_FontAsset fontAsset;
    private Queue<InfoStat> infoStatQueue { get; set; }
    private Queue<GameObject> infoStatTfQueue { get; set; }
    private float timer { get; set; }
    private float distanceToMove { get; set; }
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
                infoStatQueue = new Queue<InfoStat>();
                infoStatTfQueue = new Queue<GameObject>();
                timer = 0f;
                distanceToMove = 64f;
                GlobalStatus.Loading.System.InfoMessageManager = true;
                curStatus = 1;
                break;
            case 1:
                TryPrintMessageFromQueue();
                break;
        }
    }

    public void testBtn()
    {
        InfoStat temp = new InfoStat();
        temp.text = "테스트 메세지";
        temp.type = InfoType.NORMAL;
        infoStatQueue.Enqueue(temp);
    }

    public void AddMessageIntoQueue(InfoStat infoStat)
    {
        infoStatQueue.Enqueue(infoStat);
    }

    private IEnumerator CoroutinePrintMessageFromQueue(InfoStat infoStat)
    {
        bool isAnimating = false;
        GameObject newInfoTextGb = new GameObject();
        infoStatTfQueue.Enqueue(newInfoTextGb);
        TextMeshProUGUI newInfoText = null;
        try
        {
            newInfoTextGb.transform.SetParent(infoTextParentTf);
            newInfoTextGb.transform.localPosition = Vector3.up * distanceToMove;
            newInfoTextGb.transform.localScale = Vector3.one;
            newInfoTextGb.AddComponent<TextMeshProUGUI>();
            newInfoText = newInfoTextGb.GetComponent<TextMeshProUGUI>();
            newInfoText.font = fontAsset;
            newInfoText.fontSize = 36f;
            newInfoText.text = infoStat.text;
            Color c = Color.white;
            switch (infoStat.type)
            {
                case InfoType.NORMAL:
                    c = Color.white;
                    break;
                case InfoType.ERROR:
                    c = Color.red;
                    break;
            }
            newInfoText.color = new Color(c.r, c.g, c.b, 0f);
            isAnimating = true;
            GetComponent<CommonGameManager>().MoveObject(newInfoText.transform, DirectionType.DOWN, 3f, distanceToMove / 128f);
            GetComponent<CommonGameManager>().FadeObject(newInfoText.transform, true, 3f, () => { isAnimating = false; });
        }
        catch (MissingReferenceException)
        {
            // 실행 중 새로운 메세지 삽입으로 인한 파손
        }
        while (isAnimating)
        {
            yield return new WaitForSeconds(0.01f);
        }
        timer = 0f;
        while (timer < 1f)
        {
            if (newInfoTextGb == null) break;
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f;
        }
        try
        {
            isAnimating = true;
            GetComponent<CommonGameManager>().MoveObject(newInfoText.transform, DirectionType.DOWN, 3f, distanceToMove / 128f);
            GetComponent<CommonGameManager>().FadeObject(newInfoText.transform, false, 3f, () =>
            {
                Destroy(newInfoTextGb);
                timer = 0f;
            });
        }
        catch (MissingReferenceException)
        {
            // 실행 중 새로운 메세지 삽입으로 인한 파손
        }
    }

    private void TryPrintMessageFromQueue()
    {
        InfoStat curInfoStat;
        if (infoStatQueue != null && infoStatQueue.Count > 0)
        {
            curInfoStat = infoStatQueue.Dequeue();
            // 메세지 출력
            // 만약 지금 재생중인 메세지가 있다 = infoStatTfQueue에 뭔가 들어있다 -> 기존 메세지 파괴 = infoStatTfQueue Dequeue 후 Destroy
            GameObject tempGo = null;
            if (infoStatTfQueue.TryDequeue(out tempGo))
            {
                Destroy(tempGo);
                timer = 0f;
            }
            StartCoroutine(CoroutinePrintMessageFromQueue(curInfoStat));
        }
    }
}
