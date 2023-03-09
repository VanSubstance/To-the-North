using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowModalGridMapTileController : MonoBehaviour, IWindowModalGridSlot, IWindowModalGridItem
{
    private MapTileVO mapTileVO;
    private Image image;
    private bool isMouseIn = false;
    private System.Action actionCallback;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClearContent()
    {
        //Debug.Log("해당 슬롯에 설치된 객체 삭제하기");
    }

    public void InitContent<T>(T contentToInit)
    {
        if (typeof(T) != typeof(MapTileVO)) return;
        if (image == null) image = GetComponent<Image>();
        mapTileVO = new MapTileVO((MapTileVO)(object)contentToInit);
        image.sprite = Resources.Load<Sprite>($"{PathInfo.Image.Map.tile}{mapTileVO.imagePath}");
        image.color = Color.white;
        GetComponent<BoxCollider>().size = new Vector3(GlobalSetting.gridUnitSize, GlobalSetting.gridUnitSize, 1f);
    }

    public void InstallOnSlot(IWindowModalGridSlot targetSlot)
    {
        // 설치 불가
        return;
    }

    private void OnMouseEnter()
    {
        isMouseIn = true;
    }
    private void OnMouseExit()
    {
        isMouseIn = false;
    }
    private void OnMouseUp()
    {
        if (isMouseIn)
        {
            Debug.Log("해당 타일 선택:: " + mapTileVO.imagePath);
            actionCallback();
            // 해당 타일 선택
            // = 마우스 이벤트 재정의가 필요
            // 타일창 닫기
        }
    }

    public void SetCallbackAfterClick(Action actionCallback)
    {
        this.actionCallback = actionCallback;
    }

    [System.Serializable]
    public class MapTileVO
    {
        public int id;
        public string imagePath;

        public MapTileVO()
        {
            id = 0;
            imagePath = string.Empty;
        }

        public MapTileVO(int id, string imagePath)
        {
            this.id = id;
            this.imagePath = imagePath;
        }

        public MapTileVO(MapTileVO mapTileVO)
        {
            id = mapTileVO.id;
            imagePath = mapTileVO.imagePath;
        }
    }
}
