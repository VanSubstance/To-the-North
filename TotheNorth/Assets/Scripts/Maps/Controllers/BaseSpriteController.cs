using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpriteController : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer allForAbsoulteSprite, allForDetectionSprite, backSprite;
    // Start is called before the first frame update
    void Start()
    {
        allForAbsoulteSprite.color = Color.white;
        allForDetectionSprite.color = new Color(1, 1, 1, 1 / 10f);
        backSprite.color = Color.black;
    }
}
