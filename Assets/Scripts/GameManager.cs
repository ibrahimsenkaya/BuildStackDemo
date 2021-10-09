using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action nextLevel,winLevel,loseLevel;
    public event Action fitPerfect,justFit;

    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void InvokeStatusAction(gameStatusEvents type)
    {
        switch (type)
        {
            case gameStatusEvents.nextLevel:
                nextLevel?.Invoke();
                break;
            case gameStatusEvents.winLevel:
                winLevel?.Invoke();
                break;
            case gameStatusEvents.loseLevel:
                loseLevel?.Invoke();
                break;
            
        }
    }
    public void InvokeGameAction(gameEvents type)
    {
        switch (type)
        {
            case gameEvents.fitPerfect:
                fitPerfect?.Invoke();
                break;
            case gameEvents.justFit:
                justFit?.Invoke();
                break;
            
            
        }
    }

    public enum gameStatusEvents
   {
       nextLevel,
       winLevel,
       loseLevel
   }
    public enum gameEvents
    {
        fitPerfect,
        justFit,
       
    }

}
