using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject losePanel, successPanel;

    private void Start()
    {
        GameManager.instance.winLevel += activeSuccesspanelHelper;
        GameManager.instance.loseLevel += activeLosepanelHelper;
    }

    public void Retry()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        GameManager.instance.InvokeStatusAction(GameManager.gameStatusEvents.nextLevel);

    }

    void activeSuccesspanelHelper()
    {
        StartCoroutine(activeSuccesspanel());
    }
    void activeLosepanelHelper()
    {
        StartCoroutine(activeLosepanel());
    }
    IEnumerator activeSuccesspanel()
    {
        yield return new WaitForSeconds(1f);
        successPanel.SetActive(true);
    }

    IEnumerator activeLosepanel()
    {
        yield return new WaitForSeconds(1f);

        losePanel.SetActive(true);
    }
}