using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float totalTime = 60f;
    private float currentTime;
    private bool timerStarted = false;
    private TextMeshProUGUI timerText;
    public bool timeOver = false;
    public int timeLeft = 0;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        currentTime = totalTime;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (timerStarted && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        timeLeft = (int)currentTime;
    }

    private void UpdateTimerDisplay()
    {
        if (currentTime <= 0)
        {
            currentTime = 0;
            timeOver = true;
        }

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{00:00}:{01:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        currentTime = totalTime;
        timeOver = false;
        timerStarted = true;
    }

    public void StopTimer()
    {
        timerStarted = false;
    }

}