using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Image : MonoBehaviour
{
    public SpriteRenderer image;

    private Image im;
    private void Start()
    {
        im = transform.GetComponent<Image>();
        im.color = image.color;
        im.sprite = image.sprite;
    }
}
