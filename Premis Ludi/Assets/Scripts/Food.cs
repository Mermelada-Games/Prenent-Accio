using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] public string foodType;
    public bool isDragging = false;
    public int mergeIdx;
    public int positionIdx;
    private Vector3 initialPosition;
    private FoodMerge foodMerge;

    private void Start()
    {
        foodMerge = GameObject.Find("FoodMerge").GetComponent<FoodMerge>();
        ResetInitialPosition();
    }

    private void Update()
    {
        if (isDragging)
        {
            if (mergeIdx == -1) foodMerge.gridPositionFree[positionIdx] = true;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    public void ResetInitialPosition()
    {
        transform.position = foodMerge.GetFirstFreePosition(this);
        mergeIdx = -1;
    }
}
