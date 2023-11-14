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
    [SerializeField] private GameObject background2;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float darkeningSpeed = 0.1f;
    [SerializeField] private float lighteningSpeed = 0.3f;
    [SerializeField] private float descentSpeed = 1.0f;
    [SerializeField] private float ascentSpeed = 3.0f;
    [SerializeField] private TextMeshProUGUI fishCountText;
    [SerializeField] private TextMeshProUGUI trashCountText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject image;

    private bool isDragging = false;
    private bool isDescending = false;
    private bool hasReset = false;
    private Material waterMaterial;
    private Material waterMaterial2;
    private Color originalColor;
    private Color originalColor2;
    private float positionY = 0;
    private Timer timer;
    private SceneSystem sceneSystem;
    private bool gameEnded = false;
    private int previousFishCount = 0;
    private int previousTrashCount = 0;

    private void Start()
    {
        sceneSystem = FindObjectOfType<SceneSystem>();
        timer = FindObjectOfType<Timer>();
        waterMaterial = background.GetComponent<Renderer>().material;
        waterMaterial2 = background2.GetComponent<Renderer>().material;
        originalColor = waterMaterial.color;
        originalColor2 = waterMaterial.color;
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

        if (!gameEnded)
        {
            if (isDescending)
            {
                fishingRod.transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
                Camera.main.transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
                waterMaterial.color = Color.Lerp(waterMaterial.color, Color.black, darkeningSpeed * Time.deltaTime);
                waterMaterial2.color = Color.Lerp(waterMaterial2.color, Color.black, darkeningSpeed * Time.deltaTime);

            }
            else if (!isDescending && fishingRod.transform.position.y < positionY)
            {
                fishingRod.transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
                Camera.main.transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
                waterMaterial.color = Color.Lerp(waterMaterial.color, originalColor, lighteningSpeed * Time.deltaTime);
                waterMaterial2.color = Color.Lerp(waterMaterial2.color, originalColor, lighteningSpeed * Time.deltaTime);
            }
            
            if (!hasReset && fishingRod.transform.position.y >= positionY && hook.isHooked)
            {
                hasReset = true;
                Debug.Log("Resetting");
                StartCoroutine(RestartDescend());
            }
        }

        if (timer.timeOver)
        {
            StartCoroutine(ShowResults());
        }

    }

    public void UpdateText()
    {
        fishCountText.SetText(hook.fishCount.ToString());
        trashCountText.SetText(hook.trashCount.ToString());
    }

    public void UpdateScoreText()
    {
        int newTrashScore = (hook.trashCount - previousTrashCount) * 20;
        int newFishScore = (hook.fishCount - previousFishCount) * 5;

        int newScore = newTrashScore - newFishScore + sceneSystem.score;

        if (newScore > sceneSystem.score)
        {
            StartCoroutine(UpdateScoreProgressively(sceneSystem.score, newScore, 1));
        }
        else if (newScore < sceneSystem.score)
        {
            StartCoroutine(UpdateScoreProgressively(sceneSystem.score, newScore, -1));
        }
        else if (newScore == sceneSystem.score)
        {
            scoreText.SetText("Puntuació: " + newScore);
        }

        sceneSystem.score = newScore;
        previousTrashCount = hook.trashCount;
        previousFishCount = hook.fishCount;
    }

    private void UpdateFinalScoreText()
    {
        finalScoreText.SetText("Puntuació: " + sceneSystem.score);
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
        yield return new WaitForSeconds(4);
        sceneSystem.ChangeScene();
    }

    private IEnumerator ShowResults()
    {
        timer.timeOver = false;
        gameEnded = true;
        yield return new WaitForSeconds(1);
        if (!image.activeSelf)
        {
            image.SetActive(true);
            UpdateFinalScoreText();
            StartCoroutine(EndGame());
        }
    }

    public void EnableDescend()
    {
        isDescending = true;
    }
}