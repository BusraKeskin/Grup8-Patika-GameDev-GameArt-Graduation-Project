using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventManager.loadSameScene?.Invoke(); //Event calistirma yontemi
        }
    }
}
