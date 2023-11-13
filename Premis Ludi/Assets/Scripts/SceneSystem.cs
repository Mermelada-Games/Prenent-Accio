using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    [SerializeField] private string[][] scenes;
    private int currentRow = 0;
    private int currentCol = 0;

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
            new string[] { "Food1", "Food2", "Food3" }
        };
    }

    public void ChangeScene()
    {
        currentRow++;
        if (currentRow >= scenes[currentRow].Length)
        {
            currentRow = 0;
            currentCol++;
            if (currentCol >= scenes.Length)
            {
                currentCol = 0;
            }
        }

        string sceneName = scenes[currentRow][currentCol];
        SceneManager.LoadScene(sceneName);
    }

}