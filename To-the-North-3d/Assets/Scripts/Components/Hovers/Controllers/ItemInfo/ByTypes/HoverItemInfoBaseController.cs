using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoBaseController : MonoBehaviour, IHoverItemInfo
    {
        private Image image;
        private TextMeshProUGUI title;
        private TextMeshProUGUI price;
        private void Awake()
        {
            image = transform.Find("Image Container").GetChild(0).GetComponent<Image>();
            title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            price = transform.Find("Price").GetComponent<TextMeshProUGUI>();
        }

        public void OnItemInfoChanged(ItemBaseInfo _info)
        {
            if (image == null)
            {
                Awake();
            }
            if (_info == null)
            {
                gameObject.SetActive(false);
                image.sprite = null;
                title.text = string.Empty;
                price.text = string.Empty;
                return;
            }
            image.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(_info));
            float w = 50 * image.sprite.bounds.size.x;
            float h = 50 * image.sprite.bounds.size.y;
            float l = Mathf.Max(w, h);
            if (l == w)
            {
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(80, h * 80 / w);
            }
            else
            {
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(w * 80 / h, 80);
            }
            title.text = _info.title;
            price.text = $"{_info.price} G";
            gameObject.SetActive(true);
        }
    }
}
