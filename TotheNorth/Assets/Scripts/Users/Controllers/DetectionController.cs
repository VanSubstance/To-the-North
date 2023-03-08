using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionController : MonoBehaviour
    {
        [SerializeField]
        private float range;

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = range;
        }
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.GetComponent<SpriteRenderer>() == null) return;
        //    Color temp = collision.GetComponent<SpriteRenderer>().color;
        //    switch (collision.transform.name)
        //    {
        //        case "All":
        //            // 흐릿하게 만들기
        //            collision.GetComponent<SpriteRenderer>().color = new Color(
        //                temp.r,
        //                temp.g,
        //                temp.b,
        //                0.1f
        //                );
        //            break;
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.GetComponent<SpriteRenderer>() == null) return;
        //    Color temp = collision.GetComponent<SpriteRenderer>().color;
        //    switch (collision.transform.name)
        //    {
        //        case "All":
        //            // 멀쩡하게 만들기
        //            collision.GetComponent<SpriteRenderer>().color = new Color(
        //                temp.r,
        //                temp.g,
        //                temp.b,
        //                1f
        //                );
        //            break;
        //    }
        //}
    }
}
