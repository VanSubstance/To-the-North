using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Commons
{
    public class ScreenHitEffectController : MonoBehaviour
    {
        private Image image;
        public float RotationDegree
        {
            set
            {
                Execute(value);
            }
        }

        private void Awake()
        {
            image = transform.GetChild(0).GetComponent<Image>();
            gameObject.SetActive(false);
        }

        public void Execute(float targetDegree)
        {
            transform.rotation = Quaternion.Euler(0, 0, targetDegree);
            gameObject.SetActive(true);
            StartCoroutine(CoroutineTerminate());
        }

        private IEnumerator CoroutineTerminate()
        {
            yield return new WaitForSeconds(0.5f);
            float i = 1;
            while (i >= 0)
            {
                i -= Time.deltaTime;
                image.color = new Color(1, 1, 1, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            image.color = Color.white;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
