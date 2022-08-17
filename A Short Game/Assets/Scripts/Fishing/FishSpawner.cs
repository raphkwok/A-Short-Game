using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fish;
    [SerializeField] private float initialSpawns;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float maxFish;
    [SerializeField] private float areaDepth = 5f;
    private float timeLeft;
    private BoxCollider areaBounds;
    [SerializeField] private List<GameObject> currentFish = new List<GameObject>();

    //Interactable Variables
    private float areaWidth;
    private float areaLength;

    public void ReceiveValues(float interactableAreaWidth, float interactableAreaLength)
    {
        areaWidth = interactableAreaWidth;
        areaLength = interactableAreaLength;

        StartSpawning();
    }

    public void StartSpawning()
    {
        areaBounds = gameObject.AddComponent<BoxCollider>();
        areaBounds.center = new Vector3(0, - areaDepth / 2, areaLength / 2);
        areaBounds.size = new Vector3(areaWidth, areaDepth, areaLength);
        areaBounds.isTrigger = true;

        timeLeft = spawnTimer;

        for (int i = 0; i < initialSpawns; i++)
        {
            print("spawn");

            StartCoroutine(WaitForTime(1f, 5f));
        }
    }

    IEnumerator WaitForTime(float minTime, float maxTime)
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        GameObject chosenFish = fish[Random.Range(0, fish.Length)];
        GameObject instantiatedFish = Instantiate(chosenFish, GetRandomPointInsideCollider(areaBounds), Quaternion.identity);
        currentFish.Add(instantiatedFish);
        instantiatedFish.GetComponent<FishMovement>().ReceiveValues(areaBounds);
    }

    public Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
    {

        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;

        return boxCollider.transform.TransformPoint(point);
    }

    public void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            if (currentFish.Count < maxFish)
            {
                GameObject chosenFish = fish[Random.Range(0, fish.Length)];
                GameObject instantiatedFish = Instantiate(chosenFish, GetRandomPointInsideCollider(areaBounds), Quaternion.identity);
                currentFish.Add(instantiatedFish);
                instantiatedFish.GetComponent<FishMovement>().ReceiveValues(areaBounds);
                timeLeft = spawnTimer;
            }
        }
    }
}
