using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoBaseController : MonoBehaviour, IHoverItemInfo
    {
        private Image image;
        private TextMeshProUGUI title;
        private TextMeshProUGUI price;
        private void Awake()
        {
            image = transform.Find("Image").GetComponent<Image>();
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
            title.text = _info.title;
            price.text = $"{_info.price} G";
            gameObject.SetActive(true);
        }
    }
}
