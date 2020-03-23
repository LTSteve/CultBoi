using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] ToChooseFrom;

    public SpriteRenderer Renderer;

    private void Awake()
    {
        Renderer.sprite = ToChooseFrom[Random.Range(0, ToChooseFrom.Length)];
    }
}
