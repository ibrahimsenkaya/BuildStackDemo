using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followCam;
    [SerializeField] private CinemachineFreeLook Rotatecam;
    private bool canRotate;
    private float RotateVal;

    private void Start()
    {
        GameManager.instance.nextLevel += InGame;
        GameManager.instance.winLevel += InFinish;
        InGame();
       
    }

    void InGame()
    {
        RotateVal = 0;
        canRotate = false;
        followCam.Priority = 12;
    }

    public void InFinish()
    {
        //DOTween.To(()=> RotateVal, x=> RotateVal = x, 360, 15f).SetEase(Ease.Linear);
        Debug.Log("InFinishcam ");
        canRotate = true;
        followCam.Priority = 8;
    }

    private void Update()
    {
        if (canRotate)
        {
            RotateVal += .1f;
            Rotatecam.m_XAxis.Value = RotateVal;
        }
       
    }

}
