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

    private void Start()
    {
        foodMergeObjects = GameObject.FindGameObjectsWithTag("FoodMerge");
        foodType = new string[foodMergeObjects.Length];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Food"))
            {
                selectedFood = hit.collider.GetComponent<Food>();
                selectedFood.isDragging = true;
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
                            if (collider.gameObject == foodMergeObjects[i].gameObject)
                            {
                                foodType[i] = selectedFood.foodType;

                                if (selectedFood.mergeIdx != -1 && selectedFood.mergeIdx != i) foodType[selectedFood.mergeIdx] = null;

                                lastSelectedFood = selectedFood;
                                selectedFood.mergeIdx = i;
                                lastFoodMergeObject = collider.gameObject;

                            }
                        }

                        selectedFood.transform.position = collider.transform.position;
                        break;
                    }
                }

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
            else
            {
                if (selectedFood.mergeIdx != -1) foodType[selectedFood.mergeIdx] = null;
                selectedFood.ResetInitialPosition();
            }

            if (creation)
            {
                Debug.Log("Bread");

                creation = false;
            }

            selectedFood.isDragging = false;
            selectedFood = null;
            Debug.Log("food1:" + foodType[0] + "    food2:" + foodType[1] + "   food3:" + foodType[2]);
        }
    }
}
