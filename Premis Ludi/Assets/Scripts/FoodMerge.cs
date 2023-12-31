using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class FoodMerge : MonoBehaviour
{
    [SerializeField] private GameObject resultsImage;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Prefabs")]
    [SerializeField] private GameObject wheatPrefab;
    [SerializeField] private GameObject milkPrefab;
    [SerializeField] private GameObject flourPrefab;
    [SerializeField] private GameObject breadPrefab;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private int wheatQuantity = 0;
    [SerializeField] private int milkQuantity = 0;

    [Header("Grid")]
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;
    [SerializeField] private float spacing = 20f;
    [SerializeField] private Vector3 gridStartPosition;

    private string[] foodType;
    private Food selectedFood;
    private Food lastSelectedFood;
    private GameObject[] foodMergeObjects;
    private GameObject lastFoodMergeObject;
    private bool createBread = false;
    private bool createFlour = false;
    private bool createCheese = false;
    private int sortingOrder = 1;
    public bool[] gridPositionFree;
    private Vector3[] gridPositions;
    private Timer timer;
    private SceneSystem sceneSystem;

    private void Start()
    {
        sceneSystem = FindObjectOfType<SceneSystem>();
        timer = FindObjectOfType<Timer>();
        foodMergeObjects = GameObject.FindGameObjectsWithTag("FoodMerge");
        foodType = new string[foodMergeObjects.Length];
        gridPositionFree = new bool[rows * columns];
        gridPositions = GenerateGridPositions(rows, columns, spacing, gridStartPosition);

        List<GameObject> instances = new List<GameObject>();

        for (int i = 0; i < wheatQuantity; i++)
        {
            instances.Add(wheatPrefab);
        }

        for (int i = 0; i < milkQuantity; i++)
        {
            instances.Add(milkPrefab);
        }

        instances = instances.OrderBy(item => Random.value).ToList();

        foreach (GameObject instancePrefab in instances)
        {
            Instantiate(instancePrefab, Vector3.zero, Quaternion.identity);
        }

        scoreText.SetText("Puntuació: " + sceneSystem.score);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int foodLayerMask = 1 << LayerMask.NameToLayer("Food");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, foodLayerMask);

            if (hit.collider != null && hit.collider.CompareTag("Food"))
            {
                selectedFood = hit.collider.GetComponent<Food>();
                selectedFood.isDragging = true;
                sortingOrder++;
                selectedFood.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedFood != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(selectedFood.transform.position, 0.05f);

            if (colliders.Length > 1)
            {
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("FoodMerge") && collider != selectedFood.GetComponent<Collider2D>())
                    {
                        for (int i = 0; i < foodMergeObjects.Length; i++)
                        {
                            if (collider.gameObject == foodMergeObjects[i].gameObject && foodType[i] == null)
                            {
                                foodType[i] = selectedFood.foodType;

                                if (selectedFood.mergeIdx != -1 && selectedFood.mergeIdx != i) foodType[selectedFood.mergeIdx] = null;

                                lastSelectedFood = selectedFood;
                                selectedFood.mergeIdx = i;
                                selectedFood.positionIdx = -1;
                                lastFoodMergeObject = collider.gameObject;

                                selectedFood.transform.position = collider.transform.position;
                                break;
                            }
                            else if (collider.gameObject == foodMergeObjects[i].gameObject && foodType[i] != null)
                            {
                                if (selectedFood.mergeIdx == i)
                                { 
                                    selectedFood.transform.position = collider.transform.position;
                                    break;
                                }
                                else 
                                {                               
                                    Reset();
                                }
                            }
                        }
                    }
                    else if (collider.CompareTag("Food") && collider != selectedFood.GetComponent<Collider2D>() && collider.GetComponent<Food>().mergeIdx == -1)
                    {
                        Reset();
                    }
                    else if (collider.CompareTag("Person") && collider != selectedFood.GetComponent<Collider2D>())
                    {
                        if (selectedFood.foodType == "Bread")
                        {
                            collider.GetComponent<Person>().breadQuantity++;
                            collider.GetComponent<Person>().UpdateText();
                            Reset();
                            Destroy(selectedFood.gameObject);
                        }
                        else if (selectedFood.foodType == "Cheese")
                        {
                            collider.GetComponent<Person>().cheeseQuantity++;
                            collider.GetComponent<Person>().UpdateText();
                            Reset();
                            Destroy(selectedFood.gameObject);
                        }
                        else Reset();
                    }
                }
                TryCreate();
                ShowResults();
            }
            else Reset();

            selectedFood.isDragging = false;
            selectedFood.GetComponent<SpriteRenderer>().sortingOrder = 1;
            selectedFood = null;
            Debug.Log("food1:" + foodType[0] + "    food2:" + foodType[1] + "   food3:" + foodType[2]);
        }

        if (timer.timeOver)
        {
            timer.timeOver = false;
            ShowResults();
            StartCoroutine(EndGame());
        }
    }

    public void TryCreate()
    {
        bool creationTemp = true;
        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            if (foodType[i] == null || foodType[i] != "Flour")
            {
                creationTemp = false;
                break;
            }
        }
        createBread = creationTemp;
        if (createBread) CreateBread();

        creationTemp = true;
        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            if (foodType[i] == null || foodType[i] != "Wheat")
            {
                creationTemp = false;
                break;
            }
        }
        createFlour = creationTemp;
        if (createFlour) CreateFlour();

        creationTemp = true;
        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            if (foodType[i] == null || foodType[i] != "Milk")
            {
                creationTemp = false;
                break;
            }
        }
        createCheese = creationTemp;
        if (createCheese) CreateCheese();
    }

    private void CreateBread()
    {
        Debug.Log("Bread");
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject foodObject in foodObjects)
        {
            Food foodComponent = foodObject.GetComponent<Food>();
            if (foodComponent != null && foodComponent.foodType == "Flour" && foodComponent.mergeIdx != -1)
            {
                Destroy(foodObject);
            }
        }

        GameObject newFoodObject = Instantiate(breadPrefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            foodType[i] = null;
        }
        createBread = false;

        UpdateScoreText();
    }

    private void CreateFlour()
    {
        Debug.Log("Flour");
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject foodObject in foodObjects)
        {
            Food foodComponent = foodObject.GetComponent<Food>();
            if (foodComponent != null && foodComponent.foodType == "Wheat" && foodComponent.mergeIdx != -1)
            {
                Destroy(foodObject);
            }
        }

        GameObject newFoodObject = Instantiate(flourPrefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            foodType[i] = null;
        }
        createFlour = false;

        UpdateScoreText();
    }

    private void CreateCheese()
    {
        Debug.Log("Cheese");
        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject foodObject in foodObjects)
        {
            Food foodComponent = foodObject.GetComponent<Food>();
            if (foodComponent != null && foodComponent.foodType == "Milk" && foodComponent.mergeIdx != -1)
            {
                Destroy(foodObject);
            }
        }

        GameObject newFoodObject = Instantiate(cheesePrefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < foodMergeObjects.Length; i++)
        {
            foodType[i] = null;
        }
        createFlour = false;

        UpdateScoreText();
    }

    private void Reset()
    {
        if (selectedFood.mergeIdx != -1) foodType[selectedFood.mergeIdx] = null;
        selectedFood.ResetInitialPosition();
    }

    private Vector3[] GenerateGridPositions(int rows, int columns, float spacing, Vector3 start)
    {
        Vector3[] positions = new Vector3[rows * columns];
        gridPositionFree = new bool[rows * columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                float x = start.x + col * spacing;
                float y = start.y - row * spacing;
                positions[index] = new Vector3(x, y, 0);
                gridPositionFree[index] = true;
            }
        }
        return positions;
    }

    public Vector3 GetFirstFreePosition(Food food)
    {
        Debug.Log(gridPositionFree.Length);
        for (int i = 0; i < gridPositionFree.Length; i++)
        {
            if (gridPositionFree[i])
            {
                gridPositionFree[i] = false;
                food.positionIdx = i;
                return gridPositions[i];
            }
        }
        
        return Vector3.zero;
    }

    private void ShowResults()
    {
        bool allObjectivesCompleted = true;
        foreach(Person person in FindObjectsOfType<Person>())
        {
            person.Complete();
            if (!person.objectiveCompleted)
            {
                allObjectivesCompleted = false;
                break;
            }
        }
        if (allObjectivesCompleted) 
        {
            UpdateScoreText();
            StartCoroutine(ShowFinalResults());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        sceneSystem.ChangeScene();
    }

    private IEnumerator ShowFinalResults()
    {
        timer.StopTimer();
        yield return new WaitForSeconds(1);
        if (!resultsImage.activeSelf)
        {
            resultsImage.SetActive(true);
            UpdateFinalScoreText();
            StartCoroutine(EndGame());
        }
    }

    public void UpdateScoreText()
    {
        int newScore = sceneSystem.score + 10;

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

    private void UpdateFinalScoreText()
    {
        sceneSystem.score += timer.timeLeft;
        finalScoreText.SetText("Puntuació: " + sceneSystem.score);
    }

}
