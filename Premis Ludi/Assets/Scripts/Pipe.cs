using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float correctRotation = 0;
    private float rotationStep = 60.0f;

    public bool isCorrectRotation = false;

    private bool isMouseOver = false;

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    private void Update()
    {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, -rotationStep));
        float currentRotation = NormalizeAngle(transform.eulerAngles.z);
        float correctedRotation = NormalizeAngle(correctRotation);

        if (Mathf.Approximately(currentRotation, correctedRotation))
        {
            isCorrectRotation = true;
        }
        else
        {
            isCorrectRotation = false;
        }
    }

    private float NormalizeAngle(float angle)
    {
        return (angle + 360.0f) % 360.0f;
    }
}
