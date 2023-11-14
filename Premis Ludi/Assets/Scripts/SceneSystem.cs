using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    [SerializeField] private string[][] scenes;
    private int currentRow = 0;
    private int currentCol = 0;
    public int score = 0;

    public static SceneSystem Instance;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeScenes();
    }

    private void InitializeScenes()
    {
        scenes = new string[][]
        {
            new string[] { "Fishing1", "Fishing2", "Fishing3" },
            new string[] { "Pipes1", "Pipes2", "Pipes3" },
            new string[] { "Food1", "Food2"}
        };
    }

    public void ChangeScene()
    {
        currentRow++;
        if (currentRow >= 3)
        {
            currentRow = 0;
            currentCol++;
            if (currentCol >= 3)
            {
                currentCol = 0;
            }
        }
        else if (currentCol >= 2 && currentRow >= 2)
        {
            currentRow = 0;
            currentCol = 0;
        
        }

        string sceneName = scenes[currentRow][currentCol];
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneIntro()
    {
        Debug.Log("ChangeSceneIntro");
        SceneManager.LoadScene("Fishing1");
    }

}