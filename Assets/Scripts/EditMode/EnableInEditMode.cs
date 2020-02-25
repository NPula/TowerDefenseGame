using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInEditMode : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.instance.gameState == GameManager.Gamestate.edit)
        {
            gameObject.SetActive(true);
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }
}
