using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public delegate void modeChangeDelegate(bool mode);
    public static event modeChangeDelegate onModeChange;
    public bool _isStart = false;
    
   public void isPlayModeOn()
    {
        _isStart = true;
        onModeChange(_isStart);
    }
    public void isPlayModeOff()
    {
        _isStart = false;
    }
   
    

}
