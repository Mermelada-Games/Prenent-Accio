using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject tv;

    private void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        title.SetActive(false);
        tv.SetActive(false);
    }
}
