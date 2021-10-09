using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
  [SerializeField] private Transform player;
  [SerializeField] private GameObject finishPlatform;
  private int howManyBlocks;
  private CanvasController canvasController;
  private Transform tempFinish;
  private void Awake()
  {
    canvasController = FindObjectOfType<CanvasController>();
    canvasController.nextLevel += NextLevel;
   
    if (!PlayerPrefs.HasKey("PlayerLevel"))
    {
      PlayerPrefs.SetInt("PlayerLevel",1);
    }
    if (player==null)
    {
      player = FindObjectOfType<PlayerController>().transform;
    }
    PlaceFinish();
  }

  void PlaceFinish()
  {
    
    Debug.Log( PlayerPrefs.GetInt("PlayerLevel"));
    howManyBlocks = PlayerPrefs.GetInt("PlayerLevel")+2;
    if (tempFinish==null)
    {
      tempFinish= Instantiate(finishPlatform, new Vector3(0, -.4f, player.transform.position.z + howManyBlocks+3+(finishPlatform.transform.localScale.z/2f)-.5f),Quaternion.identity).transform;
    }
    else
    {
      tempFinish= Instantiate(finishPlatform, new Vector3(0, -.4f, tempFinish.transform.position.z + howManyBlocks+3+(finishPlatform.transform.localScale.z/2f)),Quaternion.identity).transform;
    }
    
  }
  void NextLevel()
  {
    PlaceFinish();
  }
}
