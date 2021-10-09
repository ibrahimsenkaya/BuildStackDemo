using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float speed;
    private float tempSpeed;
    private Animator animator;
    [SerializeField] private LayerMask layermask;
    [SerializeField] private Transform rayPoint;
    private bool dummyOnce=true;
    

    private void Start()
    {
        tempSpeed = speed;
        animator = GetComponent<Animator>();
        playerController = this;
        speed += PlayerPrefs.GetInt("PlayerLevel") * .02f;
        GameManager.instance.nextLevel += NextLevel;
      
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        RaycastHit hit;
        if (!Physics.Raycast(rayPoint.position, Vector3.down, out hit, 2f, layermask)&&dummyOnce)
        {
            dummyOnce = false;
            GameManager.instance.InvokeStatusAction(GameManager.gameStatusEvents.loseLevel);
            playerController.enabled = false;
            doRagdoll();
        }
    }

    void doRagdoll()
    {
        animator.enabled = false;
        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Coin") || col.CompareTag("Diamond") || col.CompareTag("Star"))
        {
            col.GetComponent<CollectableController>().Collect();
        }
        else if (col.CompareTag("Finish"))
        {
            speed = 0;
            transform.DOMoveZ(col.transform.position.z, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                animator.SetTrigger("Dance");
                PlayerPrefs.SetInt("PlayerLevel", PlayerPrefs.GetInt("PlayerLevel") + 1);
                GameManager.instance.InvokeStatusAction(GameManager.gameStatusEvents.winLevel);
            });
        }
    }

    void NextLevel()
    {
        StartCoroutine(NextLevelCor());
    }
    IEnumerator NextLevelCor()
    {
        yield return new WaitForSeconds(2f);
        speed = tempSpeed;
        speed += PlayerPrefs.GetInt("PlayerLevel") * .02f;
        animator.SetTrigger("Run");
    }
}