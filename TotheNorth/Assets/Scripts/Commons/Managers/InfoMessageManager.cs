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
        infoStatQueue = new Queue<InfoStat>();
        infoStatTfQueue = new Queue<GameObject>();
        timer = 0f;
        distanceToMove = 64f;
        curStatus = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                infoStatQueue = new Queue<InfoStat>();
                timer = 0f;
                distanceToMove = 30f;
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
        temp.text = "�׽�Ʈ �޼���";
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
        catch (MissingReferenceException err)
        {
            // ���� �� ���ο� �޼��� �������� ���� �ļ�
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
        catch (MissingReferenceException err)
        {
            // ���� �� ���ο� �޼��� �������� ���� �ļ�
        }
    }

    private void TryPrintMessageFromQueue()
    {
        InfoStat curInfoStat;
        if (infoStatQueue != null && infoStatQueue.Count > 0)
        {
            curInfoStat = infoStatQueue.Dequeue();
            // �޼��� ���
            // ���� ���� ������� �޼����� �ִ� = infoStatTfQueue�� ���� ����ִ� -> ���� �޼��� �ı� = infoStatTfQueue Dequeue �� Destroy
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
