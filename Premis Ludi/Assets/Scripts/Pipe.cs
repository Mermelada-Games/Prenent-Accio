using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float[] correctRotations;
    private float rotationStep = 60.0f;
    public bool isCorrectRotation = false;
    private bool isMouseOver = false;

    private void Start()
    {
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 6) * rotationStep));
        VerifyRotation();
    }

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
        VerifyRotation();
    }

    private void VerifyRotation()
    {
        if (correctRotations.Length == 0)
        {
            isCorrectRotation = true;
            return;
        }

        float currentRotation = NormalizeAngle(transform.eulerAngles.z);

        isCorrectRotation = false;

        foreach (float correctedRotation in correctRotations)
        {
            if (Mathf.Approximately(currentRotation, NormalizeAngle(correctedRotation)) || Mathf.Approximately(currentRotation, 360.0f))
            {
                isCorrectRotation = true;
                break;
            }
        }

    }

    private float NormalizeAngle(float angle)
    {
        return (angle + 360.0f) % 360.0f;
    }
}