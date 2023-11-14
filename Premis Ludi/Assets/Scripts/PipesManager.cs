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
            StartCoroutine(ShowResults());
            StartCoroutine(EndGame());
        }

        if (timer.timeOver)
        {
            timer.timeOver = false;
            StartCoroutine(ShowResults());
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        sceneSystem.ChangeScene();
    }

    private IEnumerator ShowResults()
    {
        UpdateScoreText();
        yield return new WaitForSeconds(1);
        if (!resultsImage.activeSelf)
        {
            resultsImage.SetActive(true);
            UpdateFinalScoreText();
        }
    }
    private void UpdateFinalScoreText()
    {
        finalScoreText.SetText("Puntuació: " + sceneSystem.score);
    }

    public void UpdateScoreText()
    {
        int newScore = sceneSystem.score + timer.timeLeft;

        StartCoroutine(UpdateScoreProgressively(sceneSystem.score, newScore, 1));

        sceneSystem.score = newScore;
    }

    private IEnumerator UpdateScoreProgressively(int currentScore, int targetScore, int step)
    {
        while (currentScore != targetScore)
        {
            currentScore += step;
            scoreText.SetText("Puntuació: " + currentScore);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
