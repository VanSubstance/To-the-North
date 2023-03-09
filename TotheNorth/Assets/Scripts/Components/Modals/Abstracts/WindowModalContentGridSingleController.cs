using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TContent : contentPrefab을 초기화하기 위한 VO
// ContentModalContentGridSingleController: 타일같이 칸에 뭔가 띄웠다 꼈다 하는 것이 아닌 고정인 경우에 사용하는 그리드 컨텐츠 모달 컨트롤러
// ContentModalContentGridSingleController 의 컨텐츠 = slotPrefab = 반드시 IContentModalGridSlot, IContentModalGriditem을 implement 해야함
public class WindowModalContentGridSingleController<TContent> : AWindowModalController<List<TContent>>
{
    // slotPrefab: 빈칸 채우기용 프리펩 = 타일같은, contentPrefab : 실제 컨텐츠용 프리펩
    // = contentPrefab이 slotPrefab에 설치되는 형태
    [SerializeField]
    private Transform slotPrefab;

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
                slotsTf[i][j].GetComponent<IWindowModalGridSlot>().ClearContent();
            }
        }
    }

    public sealed override void InitCompositionByType()
    {
        if (isInit) return;
        isInit = true;
        Transform temp = base.GetContentContainerTf();
        grid = temp.GetComponent<GridLayoutGroup>();
        grid.cellSize = Vector2.one * GlobalSetting.gridUnitSize;
        for (int i = 0; i < size.x; i++)
        {
            slotsTf[i] = new Transform[(int)size.y];
            for (int j = 0; j < size.y; j++)
            {
                Transform emptySlot = Instantiate(slotPrefab);
                emptySlot.SetParent(grid.transform);
                emptySlot.localPosition = new Vector3(0f, 0f, -2f);
                emptySlot.localScale = Vector3.one;
                slotsTf[i][j] = emptySlot;
            }
        }
    }

    public sealed override void InitContentByType(List<TContent> contentToInit)
    {
        if (grid == null) InitCompositionByType();
        ClearContent();
        for (int i = 0; i < contentToInit.Count; i++)
        {
            slotsTf[i / 6][i % 6].GetComponent<IWindowModalGridItem>().InitContent(contentToInit[i]);
            slotsTf[i / 6][i % 6].GetComponent<IWindowModalGridItem>().SetCallbackAfterClick(() => ControllByKey(2));
        }
    }
}
