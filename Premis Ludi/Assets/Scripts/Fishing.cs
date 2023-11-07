using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private GameObject camera;
    [SerializeField] private Hook hook;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float darkeningSpeed = 0.1f;
    [SerializeField] private float lighteningSpeed = 0.3f;
    [SerializeField] private float descentSpeed = 1.0f;
    [SerializeField] private float ascentSpeed = 3.0f;

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

        if(hook.isHooked)
        {
            isDescending = false;
        }

        if (isDescending)
        {
            fishingRod.transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
            camera.transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
            waterMaterial.color = Color.Lerp(waterMaterial.color, Color.black, darkeningSpeed * Time.deltaTime);
        }
        else
        {
            fishingRod.transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
            camera.transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
            waterMaterial.color = Color.Lerp(waterMaterial.color, originalColor, lighteningSpeed * Time.deltaTime);
        }
    }

}