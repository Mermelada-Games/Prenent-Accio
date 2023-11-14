using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipesManager : MonoBehaviour
{
    [SerializeField] private GameObject resultsImage;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private Pipe[] pipes;
    private SceneSystem sceneSystem;
    private Timer timer;
    private bool allPipesCorrect = false;
    public bool hasWon = false;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        sceneSystem = FindObjectOfType<SceneSystem>();
        pipes = GetComponentsInChildren<Pipe>();
    }

    private void Start()
    {
        scoreText.SetText("Puntuació: " + sceneSystem.score);
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
            timer.StopTimer();
            if (!resultsImage.activeSelf)
            {
                resultsImage.SetActive(true);
                UpdateFinalScoreText();
            }
            StartCoroutine(EndGame());
        }

        if (timer.timeOver)
        {
            timer.timeOver = false;
            if (!resultsImage.activeSelf)
            {
                resultsImage.SetActive(true);
                UpdateFinalScoreText();
            }
            StartCoroutine(EndGame());
        }
    }

    private void UpdateFinalScoreText()
    {
        sceneSystem.score += timer.timeLeft;
        finalScoreText.SetText("Puntuació: " + sceneSystem.score);
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
        sceneSystem.ChangeScene();
    }
}
