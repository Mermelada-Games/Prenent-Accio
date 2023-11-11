using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Person : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI personTextPrefab;
    private TextMeshProUGUI personText; 
    [SerializeField] private Canvas canvas;
    [SerializeField] private int objectiveBreadQuantity = 2;

    private float textPositionOffset = 1f;
    public bool objectiveCompleted = false;
    public int breadQuantity = 0;

    void Start()
    {
        personText = Instantiate(personTextPrefab, new Vector3(transform.position.x, transform.position.y + textPositionOffset, 0), Quaternion.identity, canvas.transform);
        UpdateText();
    }

    public void UpdateText()
    {
        personText.SetText("Bread: " + breadQuantity + "/" + objectiveBreadQuantity);
    }

    public void Complete()
    {
        if (breadQuantity == objectiveBreadQuantity) objectiveCompleted = true;
    }
}
