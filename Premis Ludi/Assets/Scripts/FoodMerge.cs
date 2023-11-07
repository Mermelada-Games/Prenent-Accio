using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMerge : MonoBehaviour
{
    private string[] foodType;
    private Food selectedFood;
    private Food lastSelectedFood;
    private GameObject[] foodMergeObjects;
    private GameObject lastFoodMergeObject;
    private bool creation = false;
    private int sortingOrder = 1;

    private void Start()
    {
        foodMergeObjects = GameObject.FindGameObjectsWithTag("FoodMerge");
        foodType = new string[foodMergeObjects.Length];
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
                    else if (collider.CompareTag("Food") && collider != selectedFood.GetComponent<Collider2D>())
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
}
