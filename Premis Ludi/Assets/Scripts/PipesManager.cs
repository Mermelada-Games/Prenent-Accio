using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesManager : MonoBehaviour
{
    private Pipe[] pipes;
    private SceneSystem sceneSystem;
    private bool allPipesCorrect = false;
    private bool hasWon = false;

    private void Awake()
    {
        sceneSystem = FindObjectOfType<SceneSystem>();
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
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(6);
        sceneSystem.ChangeScene();
    }
}
