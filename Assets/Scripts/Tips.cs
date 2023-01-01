using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public Player player;
    WaveController waveController;
    public GameObject panel;
    void Start()
    {
        waveController = GameObject.Find("WaveController").GetComponent<WaveController>();
    }

    void Update()
    {
        if (player != null)
        {
            if (player.timer > 0.2f && player.lines.Count < 3)
            {
                transform.GetComponent<TMP_Text>().text = "Release in direction of the enemy to attack";
            }
            if (player.lines.Count > 2 && waveController.numEnemysActual == 3)
            {
                transform.GetComponent<TMP_Text>().text = "Press E to Draw in the floor and break armor of the enemys";
            }
            else if (waveController.numEnemysActual < 3)
            {
                transform.GetComponent<TMP_Text>().text = "Kill all enemys that you broke the armor";

            }
        }


        if (waveController.wave == 1)
        {
            Destroy(panel);
        }
    }
}
