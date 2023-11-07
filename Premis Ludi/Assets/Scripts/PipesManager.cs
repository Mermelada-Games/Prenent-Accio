using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesManager : MonoBehaviour
{
    private Pipe[] pipes;
    private bool allPipesCorrect = false;
    private bool hasWon = false;

    private void Awake()
    {
        pipes = GetComponentsInChildren<Pipe>();
    }

    private void Update()
    {
        bool allPipesCorrectTemp = true;

        foreach (Pipe pipe in pipes)
        {
            if(!pipe.isCorrectRotation)
            {
                allPipesCorrectTemp = false;
                break;
            }
        }

        allPipesCorrect = allPipesCorrectTemp;

        if (allPipesCorrect && !hasWon)
        {
            Debug.Log("You win!");
            hasWon = true;
        }
    }
}
