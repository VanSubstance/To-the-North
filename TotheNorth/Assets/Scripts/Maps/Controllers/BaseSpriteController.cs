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
        if (GlobalSetting.isDev)
        {
            allForAbsoulteSprite.gameObject.SetActive(false);
        }
        allForAbsoulteSprite.color = Color.white;
        allForDetectionSprite.color =
            Color.white;
        backSprite.color = Color.black;
    }
}
