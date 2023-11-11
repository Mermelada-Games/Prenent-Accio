using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] public int breadQuantity = 0;
    public bool objectiveCompleted = false;

    public void Complete()
    {
        if (breadQuantity == 0) objectiveCompleted = true;
    }
}
