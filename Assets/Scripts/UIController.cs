using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject deafeatUi;
    public GameObject waveController;
    public TMP_Text waveTXT;
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        waveController = GameObject.Find("WaveController");
    }

    void Update()
    {

    }
    public void Death()
    {
        deafeatUi.SetActive(true);
        waveTXT.text = "Died in wave"+ waveController.GetComponent<WaveController>().wave;

    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
