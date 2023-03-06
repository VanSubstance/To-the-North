using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpriteController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer frontSprite, bodySprite, backSprite;
    // Start is called before the first frame update
    void Start()
    {
        frontSprite.color = Color.white;
        bodySprite.color = Color.white;
        backSprite.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
