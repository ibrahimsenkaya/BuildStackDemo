using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{

    public event Action nextLevel;

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {
        nextLevel?.Invoke();
    }
}
