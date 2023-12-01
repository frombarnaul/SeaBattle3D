using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    public Sprite MissSprite;
    public void SpriteCreate(Vector3 position)
    {
        var spriteObject = new GameObject("SpriteObject");
        SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        BoxCollider spriteCollider = spriteObject.AddComponent<BoxCollider>();
        spriteRenderer.sprite = MissSprite;
        spriteObject.transform.position = position + new Vector3(0, 0.01f, 0);
        spriteObject.transform.Rotate(new Vector3(90, 0));
        spriteObject.tag = "Shoted";
        GetComponent<AudioSource>().Play();
    }
}
