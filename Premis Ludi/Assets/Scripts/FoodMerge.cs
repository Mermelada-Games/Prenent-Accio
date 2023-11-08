using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMerge : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;

    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;
    [SerializeField] private float spacing = 20f;
    [SerializeField] private Vector3 gridStartPosition;

    private string[] foodType;
    private Food selectedFood;
    private Food lastSelectedFood;
    private GameObject[] foodMergeObjects;
    private GameObject lastFoodMergeObject;
    private bool creation = false;
    private int sortingOrder = 1;
    public bool[] gridPositionFree;
    public Vector3[] gridPositions;

    private void Start()
    {
        foodMergeObjects = GameObject.FindGameObjectsWithTag("FoodMerge");
        foodType = new string[foodMergeObjects.Length];
        gridPositionFree = new bool[rows * columns];
        gridPositions = GenerateGridPositions(rows, columns, spacing, gridStartPosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int foodLayerMask = 1 << LayerMask.NameToLayer("Food");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, foodLayerMask);

            if (hit.collider != null && hit.collider.CompareTag("Food"))
            {
                selectedFood = hit.collider.GetComponent<Food>();
                selectedFood.isDragging = true;
                sortingOrder++;
                selectedFood.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedFood != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(selectedFood.transform.position, 0.05f);

            if (colliders.Length > 1)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("FoodMerge") && collider != selectedFood.GetComponent<Collider2D>())
                    {
                        for (int i = 0; i < foodMergeObjects.Length; i++)
                        {
                            if (collider.gameObject == foodMergeObjects[i].gameObject && foodType[i] == null)
                            {
                                foodType[i] = selectedFood.foodType;

                                if (selectedFood.mergeIdx != -1 && selectedFood.mergeIdx != i) foodType[selectedFood.mergeIdx] = null;

                                lastSelectedFood = selectedFood;
                                selectedFood.mergeIdx = i;
                                selectedFood.positionIdx = -1;
                                lastFoodMergeObject = collider.gameObject;

                                selectedFood.transform.position = collider.transform.position;
                                break;
                            }
                            else if (collider.gameObject == foodMergeObjects[i].gameObject && foodType[i] != null)
                            {
                                if (selectedFood.mergeIdx == i)
                                { 
                                    selectedFood.transform.position = collider.transform.position;
                                    break;
                                }
                                else 
                                {                               
                                    Reset();
                                }
                            }
                        }
                    }
                    else if (collider.CompareTag("Food") && collider != selectedFood.GetComponent<Collider2D>() && collider.GetComponent<Food>().mergeIdx == -1)
                    {
                        Reset();
                    }
                }
                TryCreateBread();
            }
            else Reset();

            if (creation) Creation();

            selectedFood.isDragging = false;
            selectedFood.GetComponent<SpriteRenderer>().sortingOrder = 1;
            selectedFood = null;
            Debug.Log("food1:" + foodType[0] + "    food2:" + foodType[1] + "   food3:" + foodType[2]);
        }
    }

    private void TryCreateBread()
    {
        bool creationTemp = true;
        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            if (foodType[i] == null || foodType[i] != "Wheat")
            {
                creationTemp = false;
                break;
            }
        }
        creation = creationTemp;
    }

    private void Creation()
    {
        Debug.Log("Bread");
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject foodObject in foodObjects)
        {
            Food foodComponent = foodObject.GetComponent<Food>();
            if (foodComponent != null && foodComponent.foodType == "Wheat" && foodComponent.mergeIdx != -1)
            {
                Destroy(foodObject);
            }
        }

        GameObject newFoodObject = Instantiate(prefabToSpawn, new Vector3(6, 0, 0), Quaternion.identity);


        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            foodType[i] = null;
        }
        creation = false;
    }

    private void Reset()
    {
        if (selectedFood.mergeIdx != -1) foodType[selectedFood.mergeIdx] = null;
        selectedFood.ResetInitialPosition();
    }

    private Vector3[] GenerateGridPositions(int rows, int columns, float spacing, Vector3 start)
    {
        Vector3[] positions = new Vector3[rows * columns];
        gridPositionFree = new bool[rows * columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                float x = start.x + col * spacing;
                float y = start.y - row * spacing;
                positions[index] = new Vector3(x, y, 0);
                gridPositionFree[index] = true;
            }
        }
        return positions;
    }

    public Vector3 GetFirstFreePosition(Food food)
    {
        Debug.Log(gridPositionFree.Length);
        for (int i = 0; i < gridPositionFree.Length; i++)
        {
            if (gridPositionFree[i])
            {
                gridPositionFree[i] = false;
                food.positionIdx = i;
                return gridPositions[i];
            }
        }
        
        return Vector3.zero;
    }

}
