using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TContent : contentPrefab을 초기화하기 위한 VO
// ContentModalContentGridSingleController 의 빈 칸 = 슬롯으로 사용되는 프리펩의 경우, 반드시 IContentModalGridSlot을 implement 해야함
// ContentModalContentGridSingleController 의 컨텐츠 = 아이템 = 슬롯 위에 설치되는 오브젝트로 사용되는 프리펩의 경우, 반드시 IContentModalGridSlot을 implement 해야함
public class ContentModalContentGridSingleController<TContent> : AContentModalController<List<TContent>>
{
    // slotPrefab: 빈칸 채우기용 프리펩 = 타일같은, contentPrefab : 실제 컨텐츠용 프리펩
    // = contentPrefab이 slotPrefab에 설치되는 형태
    [SerializeField]
    private Transform slotPrefab, contentPrefab;

    private GridLayoutGroup grid;
    private bool isInit = false;

    private Vector2 size = new Vector2(6, 8);
    private Transform[][] slotsTf = new Transform[6][];
    public sealed override void ClearContent()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                slotsTf[i][j].GetComponent<IContentModalGridSlot>().ClearContent();
            }
        }
    }

    public sealed override void InitCompositionByType()
    {
        if (isInit) return;
        Transform temp = base.GetContentContainer();
        grid = temp.GetComponent<GridLayoutGroup>();
        for (int i = 0; i < size.x; i++)
        {
            slotsTf[i] = new Transform[(int)size.y];
            for (int j = 0; j < size.y; j++)
            {
                Transform emptySlot = Instantiate(slotPrefab);
                emptySlot.SetParent(grid.transform);
                emptySlot.localScale = Vector3.one;
                slotsTf[i][j] = emptySlot;
            }
        }
        isInit = true;
    }

    public sealed override void InitContentByType(List<TContent> contentToInit)
    {
        if (grid == null) InitCompositionByType();
        ClearContent();
        for (int i = 0; i < contentToInit.Count; i++)
        {
            Transform newItem = Instantiate(contentPrefab);
            newItem.GetComponent<IContentModalGridItem>().InstallOnSlot(slotsTf[i % 6][i / 6].GetComponent<IContentModalGridSlot>());
            newItem.GetComponent<IContentModalGridItem>().InitContent(contentToInit[i]);
        }
    }
}
