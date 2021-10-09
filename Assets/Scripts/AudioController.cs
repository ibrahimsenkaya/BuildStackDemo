using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private RoadManager roadmanager;
    private AudioSource source;
    private float pitchValue,tempPitchValue;
    private CanvasController canvasController;
    void Start()
    {
        canvasController = FindObjectOfType<CanvasController>();
        canvasController.nextLevel += Reset;
        source = GetComponent<AudioSource>();
        pitchValue = source.pitch;
        tempPitchValue = pitchValue;
        roadmanager = FindObjectOfType<RoadManager>();
        
        roadmanager.fitPerfect += FitPerfect;
        roadmanager.justFit += JustFit;
    }

    
    void JustFit()
    {
        pitchValue = tempPitchValue;
        source.pitch = pitchValue;
        source.Play();
     
    }
    void FitPerfect()
    {
        pitchValue += .1f;
        source.pitch = pitchValue;
        source.Play();
       
       // source.Play();
    }

    private void Reset()
    {
        pitchValue = tempPitchValue;
        source.pitch = pitchValue;
    }
}
