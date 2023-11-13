using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float totalTime = 60f;
    private float currentTime;
    private TextMeshProUGUI timerText;
    public bool timeOver = false;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        currentTime = totalTime;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
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

}