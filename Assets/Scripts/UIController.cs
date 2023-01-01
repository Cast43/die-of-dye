using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject deafeatUi;
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    void Update()
    {

    }
    public void Death()
    {
        deafeatUi.SetActive(true);

    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
