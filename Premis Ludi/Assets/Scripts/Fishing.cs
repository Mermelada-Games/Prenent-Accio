using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float darkeningSpeed = 0.1f;

    private bool isDragging = false;
    private bool isDescending = true;
    private Material waterMaterial;
    private Color originalColor;

    private void Start()
    {
        waterMaterial = fishingRod.GetComponent<Renderer>().material;
        originalColor = waterMaterial.color;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (isDragging)
        {
            Vector3 currentPosition = fishingRod.transform.position;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 objectPos = fishingRod.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(mousePos));

            if (objectPos.x > 0) fishingRod.transform.Translate(Vector3.right * speed * Time.deltaTime);
            
            else if (objectPos.x < 0) fishingRod.transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        if (isDescending) waterMaterial.color = Color.Lerp(waterMaterial.color, Color.black, darkeningSpeed * Time.deltaTime);
        else waterMaterial.color = Color.Lerp(waterMaterial.color, originalColor, darkeningSpeed * Time.deltaTime);
    }
}
