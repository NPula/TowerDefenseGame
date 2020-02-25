using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInEditMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.gameState == GameManager.Gamestate.edit)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
