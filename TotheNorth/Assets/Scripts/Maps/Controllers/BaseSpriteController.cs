using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpriteController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer allForAbsoulteSprite, allForDetectionSprite, backSprite;
    // Start is called before the first frame update
    void Start()
    {
        allForAbsoulteSprite.color = Color.white;
        allForDetectionSprite.color = new Color(1f, 1f, 1f, 0.1f);
        backSprite.color = Color.black;
    }
}
