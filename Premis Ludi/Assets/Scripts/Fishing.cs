using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting.FullSerializer;

public class Fishing : MonoBehaviour
{
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private Hook hook;
    [SerializeField] private GameObject background;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float darkeningSpeed = 0.1f;
    [SerializeField] private float lighteningSpeed = 0.3f;
    [SerializeField] private float descentSpeed = 1.0f;
    [SerializeField] private float ascentSpeed = 3.0f;
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private TextMeshProUGUI trashCountText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject image;

    private bool isDragging = false;
    private bool isDescending = true;
    private bool hasReset = false;
    private Material waterMaterial;
    private Color originalColor;
    private float positionY = 0;
    private Timer timer;
    private SceneSystem sceneSystem;
    private bool levelCompleted = false;
    private int score = 0;

    private void Start()
    {
        sceneSystem = FindObjectOfType<SceneSystem>();
        timer = FindObjectOfType<Timer>();
        waterMaterial = background.GetComponent<Renderer>().material;
        originalColor = waterMaterial.color;
        UpdateScoreText();
        positionY = fishingRod.transform.position.y;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (isDragging)
        {
            Vector3 currentPosition = fishingRod.transform.position;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 objectPos = fishingRod.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(mousePos));

            if (objectPos.x > 0) fishingRod.transform.Translate(Vector3.right * speed * Time.deltaTime);

            else if (objectPos.x < 0) fishingRod.transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        if(hook.isHooked || fishingRod.transform.position.y <= -26.0f)
        {
            isDescending = false;
        }

        if (isDescending)
        {
            fishingRod.transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
            Camera.main.transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
            waterMaterial.color = Color.Lerp(waterMaterial.color, Color.black, darkeningSpeed * Time.deltaTime);
        }
        else if (!isDescending && fishingRod.transform.position.y < positionY)
        {
            fishingRod.transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
            Camera.main.transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
            waterMaterial.color = Color.Lerp(waterMaterial.color, originalColor, lighteningSpeed * Time.deltaTime);

        }
        
        if (!hasReset && fishingRod.transform.position.y >= positionY && hook.isHooked)
        {
            hasReset = true;
            Debug.Log("Resetting");
            StartCoroutine(RestartDescend());
        }

        if (timer.timeOver)
        {
            if (!image.activeSelf)
            {
                image.SetActive(true);
                if (hook.fishCount == 0 && hook.trashCount >= 1) levelCompleted = true;
                StartCoroutine(EndGame());
            }
        }

    }

    public void UpdateText()
    {
        fishCountText.SetText(hook.fishCount.ToString());
        trashCountText.SetText(hook.trashCount.ToString());
    }

    public void UpdateScoreText()
    {
        int newScore = hook.trashCount * 20 - hook.fishCount * 5;

        if (newScore > score)
        {
            StartCoroutine(UpdateScoreProgressively(score, newScore, 1));
        }
        else if (newScore < score)
        {
            StartCoroutine(UpdateScoreProgressively(score, newScore, -1));
        }
        else if (newScore == score)
        {
            scoreText.SetText("Score: " + newScore);
        }

        score = newScore;
    }

    private IEnumerator UpdateScoreProgressively(int currentScore, int targetScore, int step)
    {
        while (currentScore != targetScore)
        {
            currentScore += step;
            scoreText.SetText("Score: " + currentScore);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RestartDescend()
    {
        UpdateScoreText();
        yield return new WaitForSeconds(1);
        foreach (GameObject fish in hook.fishList)
        {
            Destroy(fish);
        }
        hook.fishList.Clear();

        foreach (GameObject trash in hook.trashList)
        {
            Destroy(trash);
        }
        hook.trashList.Clear();
        yield return new WaitForSeconds(1);
        isDescending = true;
        hook.isHooked = false;
        hasReset = false;
        Debug.Log("Restarted");
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(6);
        sceneSystem.ChangeScene();
    }

}