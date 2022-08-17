using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBounds : MonoBehaviour
{
    [SerializeField] private float boundThickness = 0.5f;
    [SerializeField] private float boundHeight = 20f;

    private float areaLength;
    private float areaWidth;

    public void ReceiveValues(float interactableAreaLength, float interactableAreaWidth)
    {
        areaLength = interactableAreaLength;
        areaWidth = interactableAreaWidth;
        CreateBounds();
    }

    private void CreateBounds()
    {
        BoxCollider rightBound = gameObject.AddComponent<BoxCollider>();
        BoxCollider leftBound = gameObject.AddComponent<BoxCollider>();
        BoxCollider forwardBound = gameObject.AddComponent<BoxCollider>();

        rightBound.center = new Vector3(areaWidth / 2, boundHeight / 2, areaLength / 2);
        rightBound.size = new Vector3(boundThickness, boundHeight, areaLength);

        leftBound.center = new Vector3(- areaWidth / 2, boundHeight / 2, areaLength / 2);
        leftBound.size = new Vector3(boundThickness, boundHeight, areaLength);

        forwardBound.center = new Vector3(0, boundHeight / 2, areaLength);
        forwardBound.size = new Vector3(areaWidth, boundHeight, boundThickness);
    }
}
