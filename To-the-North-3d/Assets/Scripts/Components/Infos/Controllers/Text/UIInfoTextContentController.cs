using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Infos
{
    public class UIInfoTextContentController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        [ColorUsage(false, order = int.MaxValue)]
        private Color textColor;

        public bool IsInActive
        {
            set
            {
                if (value)
                {
                    transform.SetParent(UIInfoTextContainerController.Instance.visualTf);
                    gameObject.SetActive(true);
                } else
                {
                    gameObject.SetActive(false);
                    text.text = string.Empty;
                    transform.SetParent(UIInfoTextContainerController.Instance.poolingTf);
                }
            }
            get
            {
                return gameObject.activeInHierarchy;
            }
        }

        private float TextOpacity
        {
            set
            {
                text.color = new Color(
                    textColor.r,
                    textColor.g,
                    textColor.b,
                    value
                    );
            }
        }

        public void ActivateText(string _text)
        {
            if (IsInActive) return;
            IsInActive = true;
            StartCoroutine(CoroutineActivateText(_text));
        }

        private IEnumerator CoroutineActivateText(string _text)
        {
            text.text = _text;
            float t = 0;
            // 등장
            text.transform.localPosition = Vector3.down * 40;
            while (t <= .2f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                t += Time.deltaTime;
                TextOpacity = t * 5;
                text.transform.localPosition = Vector3.down * 200 * (.2f - t);
            }
            text.transform.localPosition = Vector3.zero;
            // 유지
            t = 0;
            while (t <= 1.6f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                t += Time.deltaTime;
            }
            // 퇴장
            t = 0.2f;
            while (t >= 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                t -= Time.deltaTime;
                TextOpacity = t * 5;
                text.transform.localPosition = Vector3.right * 20 * (.2f - t);
            }
            text.transform.localPosition = Vector3.zero;
            IsInActive = false;
            UIInfoTextContainerController.Instance.NoticeVacancy();
        }
    }
}
