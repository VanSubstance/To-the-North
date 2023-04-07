using UnityEngine;

public class BaseSpriteController : MonoBehaviour
{
    protected SpriteRenderer spriteOutSight, spriteInSight, spriteBottom;
    // Start is called before the first frame update
    protected void Awake()
    {
        Transform temp;
        if (temp = transform.Find("InSight"))
            spriteInSight = temp.GetComponent<SpriteRenderer>();
        if (temp = transform.Find("OutSight"))
            spriteOutSight = temp.GetComponent<SpriteRenderer>();
        if (temp = transform.Find("Bottom"))
            spriteBottom = temp.GetComponent<SpriteRenderer>();
        if (spriteOutSight) spriteOutSight.color = Color.white;
        if (spriteInSight) spriteInSight.color = new Color(1, 1, 1, 1 / 10f);
        if (spriteBottom) spriteBottom.color = Color.black;
    }
}
