using UnityEngine;
using UnityEngine.UI;

public class ContentModalGridMapTileController : MonoBehaviour, IContentModalGridSlot, IContentModalGridItem
{
    private MapTileVO mapTileVO;
    private Image image;
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
    }

    public void InstallOnSlot(IContentModalGridSlot targetSlot)
    {
        // 설치 불가
        return;
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
