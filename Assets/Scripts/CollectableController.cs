using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AnimationCurve curve;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 180, 0), 1f).SetLoops(-1, LoopType.Incremental).SetLink(this.gameObject).SetEase(Ease.Linear);
    }

    public void Collect()
    {
        StartCoroutine(moveToPosition(1f));
        particle.Play();
    }
    
    IEnumerator moveToPosition(float duration)
    {
        yield return null;
        transform.GetComponent<Collider>().enabled = false;
        float time = 0;
        while (time < duration)
        {
            #region Position & Scale & Rotation

           
            transform.position = Vector3.Lerp(transform.position,  Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,20)), (time/duration)*curve.Evaluate(time));
            //transform.localScale = Vector3.Lerp(transform.position,  Vector3.zero, (time/duration)*curve.Evaluate(time));
            

            time += Time.deltaTime;
            #endregion

            yield return null;
        }
        Destroy(this.gameObject);
    }
}
