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
    public event Action WinEvent;
    private CanvasController canvasController;

    private void Awake()
    {
        tempSpeed = speed;
        animator = GetComponent<Animator>();
        playerController = this;
        canvasController = FindObjectOfType<CanvasController>();
        canvasController.nextLevel += NextLevel;
        //speed += PlayerPrefs.GetInt("PlayerLevel") * .1f;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        RaycastHit hit;
        if (!Physics.Raycast(rayPoint.position, Vector3.down, out hit, 2f, layermask))
        {
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
                WinEvent?.Invoke();
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
        animator.SetTrigger("Run");
    }
}