using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMerge : MonoBehaviour
{
    private string foodType;
    private Food selectedFood;
    private GameObject[] foodMergeObjects;

    private void Start()
    {
        foodMergeObjects = GameObject.FindGameObjectsWithTag("FoodMerge");
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
                        selectedFood.transform.position = collider.transform.position;
                        break;
                    }
                    else
                    {
                        selectedFood.ResetInitialPosition();
                    }
                }
            }
            else
            {
                selectedFood.ResetInitialPosition();
            }

            selectedFood.isDragging = false;
            selectedFood = null;
        }
    }
}
