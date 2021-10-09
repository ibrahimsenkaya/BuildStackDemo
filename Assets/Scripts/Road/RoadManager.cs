using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject roadElement,StartPlatform;
    [SerializeField] private Transform lastCube, currentCube;
    [SerializeField] private AnimationCurve scaleCurve;
    private RoadMovement roadMovemnet;
    private int blockCount, howManyBlocks;
    [SerializeField] private List<Material> blockMaterials = new List<Material>();
    private CanvasController canvasController;
    bool canTouch = true;

    public event Action fitPerfect, justFit;

    private void Start()
    {
        canvasController = FindObjectOfType<CanvasController>();
        Init();
    }

    private void Init()
    {
        createRoad();
        blockCount = 0;
        howManyBlocks = PlayerPrefs.GetInt("PlayerLevel") + 2;
        moveSpeed += PlayerPrefs.GetInt("PlayerLevel") * .1f;
        
        canvasController.nextLevel += Reset;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentCube != null && lastCube != null && canTouch)
        {
            if (blockCount == howManyBlocks)
            {
                return;
            }

            blockCount++;
            if (splitLastRoad() && blockCount < howManyBlocks)
            {
                createRoad();
                if (blockCount == howManyBlocks)
                {
                    canTouch = false;
                }
            }
        }
    }

    void createRoad()
    {
        Vector3 direction = Random.Range(0, 2) == 1 ? Vector3.right : Vector3.left;

        float zPos = lastCube.position.z + lastCube.localScale.z;
        lastCube = Instantiate(roadElement, new Vector3(direction == Vector3.right ? -5 : 5, 0, zPos),
            Quaternion.identity, this.transform).transform;

        roadMovemnet = lastCube.GetComponent<RoadMovement>();
        roadMovemnet.enabled = true;
        roadMovemnet.speed = moveSpeed;
        roadMovemnet.direction = direction;

        lastCube.name = this.transform.childCount.ToString();
        lastCube.GetComponent<Renderer>().material = blockMaterials[transform.childCount % (blockMaterials.Count-1)];
    }

    bool splitLastRoad()
    {
        roadMovemnet.speed = 0;
        float xPositionDifference = currentCube.position.x - lastCube.position.x;
        if (Mathf.Abs(xPositionDifference) < .07f)
        {
            lastCube.position = new Vector3(currentCube.position.x, lastCube.position.y, lastCube.position.z);
            roadElement = lastCube.gameObject;
            currentCube = lastCube;

            fitPerfect?.Invoke();
            return true;
        }

        float direction = xPositionDifference > 0 ? -1f : 1f;
        float newSize = lastCube.transform.localScale.x - Mathf.Abs(xPositionDifference);
        if (newSize < 0)
        {
            StartCoroutine(scaleAnimation(1f, lastCube));
            lastCube = null;
            Debug.Log("Lose");
            return false;
        }

        justFit?.Invoke();
        float newPosition = lastCube.position.x + xPositionDifference / 2f;
        float fallingBlockSize = lastCube.transform.localScale.x - newSize;

        lastCube.localScale = new Vector3(newSize, lastCube.localScale.y, lastCube.localScale.z);

        lastCube.position = new Vector3(newPosition, lastCube.position.y, lastCube.position.z);

        float roadEdge = lastCube.position.x + (newSize / 2f * direction);

        float fallingBlockPos = roadEdge + (fallingBlockSize / 2f * direction);

        SpawnFallingCube(fallingBlockPos, fallingBlockSize);

        //lastCube.transform.position = new Vector3(0, lastCube.position.y, lastCube.position.z);
        roadMovemnet.enabled = false;
        roadElement = lastCube.gameObject;
        currentCube = lastCube;

        return true;
    }

    #region FallingCubes

    void SpawnFallingCube(float pos, float scale)
    {
        Transform fallingRoad = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        fallingRoad.localScale = new Vector3(scale, lastCube.localScale.y, lastCube.localScale.z);
        fallingRoad.position = new Vector3(pos, lastCube.position.y, lastCube.position.z);
        fallingRoad.GetComponent<Renderer>().material = blockMaterials[transform.childCount % (blockMaterials.Count-1)];

        StartCoroutine(moveRoadToCenter(1f, lastCube));

        StartCoroutine(scaleAnimation(1f, fallingRoad));
    }

    #endregion


    #region Moving and scaling

    IEnumerator scaleAnimation(float duration, Transform a_object)
    {
        yield return null;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            a_object.transform.localScale =
                Vector3.Lerp(a_object.transform.localScale, Vector3.zero, scaleCurve.Evaluate(time));
            yield return null;
        }

        Destroy(a_object.gameObject);
    }

    IEnumerator moveRoadToCenter(float duration, Transform a_object)
    {
        yield return null;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            a_object.transform.position = Vector3.Lerp(a_object.transform.position,
                new Vector3(0, a_object.position.y, a_object.position.z), scaleCurve.Evaluate(time));
            yield return null;
        }
    }

    #endregion

    void Reset()
    {
        canvasController.nextLevel -= Reset;
        PreparingNextLevel();
        StartCoroutine(startNextLevel());

    }

    IEnumerator startNextLevel()
    {   
        yield return new WaitForSeconds(2f);
    
        Init();
        canTouch = true;
    }
    void PreparingNextLevel()
    {
        int childCount = transform.childCount;
        for (int i = 0; i <childCount; i++)
        { 
            transform.GetChild(0).parent = null;
        }
        Instantiate(StartPlatform, new Vector3(0, StartPlatform.transform.position.y, lastCube.position.z + 3f), Quaternion.identity);
        Transform tempLastPlatform=Instantiate(StartPlatform, new Vector3(0, StartPlatform.transform.position.y, lastCube.position.z + 4f), Quaternion.identity).transform;
        roadElement = tempLastPlatform.gameObject;
        lastCube = tempLastPlatform;
        currentCube = tempLastPlatform;
    }
}