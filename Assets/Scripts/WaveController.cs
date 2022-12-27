using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("WaveInfo")]
    public int wave = 0;
    public float timeSpawn = 1;
    public int numEnemys = 0;
    public bool spawn = false;
    [Header("EnemysInfo")]
    public GameObject[] enemys;
    public float maxPosInstX;
    public float maxPosInstY;
    [Header("HUD")]
    public GameObject waveCout;
    public GameObject canvas;

    void Start()
    {

    }

    void Update()
    {
        if (!spawn && numEnemys < wave * 1.5f + 2)
        {
            StartCoroutine(Spawn(0));
        }
        else if (numEnemys >= wave * 1.5f + 2)
        {
            
            wave++;
            StartCoroutine(NextWave());
            numEnemys = 0;
        }

    }
    public IEnumerator Spawn(int type) // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        spawn = true;
        numEnemys++;

        if (Random.Range(1, 3) == 1)
        {
            if (Random.Range(1, 3) == 1)
            {
                Instantiate(enemys[type], new Vector2(maxPosInstX, Random.Range(maxPosInstY, -maxPosInstY)), Quaternion.identity);
            }
            else
            {
                Instantiate(enemys[type], new Vector2(-maxPosInstX, Random.Range(maxPosInstY, -maxPosInstY)), Quaternion.identity);

            }
        }
        else
        {
            if (Random.Range(1, 3) == 1)
            {
                Instantiate(enemys[type], new Vector2(Random.Range(maxPosInstX, -maxPosInstX), maxPosInstY), Quaternion.identity);
            }
            else
            {
                Instantiate(enemys[type], new Vector2(Random.Range(maxPosInstX, -maxPosInstX), -maxPosInstY), Quaternion.identity);

            }
        }

        yield return new WaitForSeconds(timeSpawn);
        spawn = false;

    }
    public IEnumerator NextWave()
    {
        GameObject UIWave = Instantiate(waveCout,canvas.transform.position,Quaternion.identity,canvas.transform);
        UIWave.GetComponentInChildren<TMP_Text>().text = "Wave " + wave.ToString();
        timeSpawn *= 0.7f;
        yield return new WaitForSeconds(8);
        Destroy(UIWave);

    }
}
