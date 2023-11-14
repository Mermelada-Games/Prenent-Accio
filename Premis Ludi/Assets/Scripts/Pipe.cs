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

        float currentZRotation = transform.eulerAngles.z;
        foreach (float rotation in correctRotations)
        {
            float normalizedRotation = Mathf.Repeat(rotation, 360f);
            if (Mathf.Abs(currentZRotation - normalizedRotation) < 0.1f ||
                Mathf.Abs(currentZRotation - normalizedRotation) > 359.9f)
            {
                isCorrectRotation = true;
                return;
            }
        }

        isCorrectRotation = false;
    }

}