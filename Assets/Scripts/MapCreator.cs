using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapCreator : MonoBehaviour
{
  [SerializeField] private Transform player;
  [SerializeField] private GameObject finishPlatform;
  private int howManyBlocks;
  private Transform tempFinish;
  [SerializeField] private List<GameObject> collectables = new List<GameObject>();
  private void Awake()
  {
  
    GameManager.instance.nextLevel += NextLevel;
   
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
      Instantiate(collectables[Random.Range(0, collectables.Count)], new Vector3(tempFinish.position.x,.75f,tempFinish.position.z), Quaternion.identity);
    }
    else
    {
      tempFinish= Instantiate(finishPlatform, new Vector3(0, -.4f, tempFinish.transform.position.z + howManyBlocks+3+(finishPlatform.transform.localScale.z/2f)),Quaternion.identity).transform;
      Instantiate(collectables[Random.Range(0, collectables.Count)], new Vector3(tempFinish.position.x,.75f,tempFinish.position.z), Quaternion.identity);
    }
    
  }
  void NextLevel()
  {
    PlaceFinish();
  }
}
