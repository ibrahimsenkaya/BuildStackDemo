using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float speed; 
    private Animator animator;
    [SerializeField] private LayerMask layermask;
    [SerializeField] private Transform rayPoint;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = this;

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
        if (col.CompareTag("Coin") || col.CompareTag("Diamond")|| col.CompareTag("Star"))
        {
            col.GetComponent<CollectableController>().Collect();
        }
      
    }
}