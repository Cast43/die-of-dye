using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("WaveInfo")]
    public int wave = 0;
    public float timeSpawn = 1;
    public int numSpawn = 0;
    public int numEnemysActual = 0;
    public bool spawn = false;
    public bool waveDone = false;
    [Header("EnemysInfo")]
    public GameObject[] enemys;
    public float maxPosInstX;
    public float maxPosInstY;
    [Header("HUD")]
    public GameObject waveCout;
    public GameObject canvas;

    void Start()
    {
        StartCoroutine(FirstWave());

    }

    void Update()
    {
        if (!spawn && numSpawn < wave * 1.5f + 2)
        {
            int enemyType = Random.Range(0,enemys.Length);
            StartCoroutine(Spawn(enemyType));
        }
        else if (numEnemysActual == 0 && numSpawn >= (int)(wave * 1.5f + 2))
        {
            wave++;
            StartCoroutine(NextWave());
        }

    }
    public IEnumerator Spawn(int type) // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        spawn = true;
        numSpawn++;
        numEnemysActual++;

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
        GameObject UIWave = Instantiate(waveCout, canvas.transform.position, Quaternion.identity, canvas.transform);
        UIWave.GetComponentInChildren<TMP_Text>().text = "Wave " + wave.ToString();
        timeSpawn *= 0.7f;
        numEnemysActual = 0;
        numSpawn = 0;
        yield return new WaitForSeconds(8);
        Destroy(UIWave);

    }
    public IEnumerator FirstWave()
    {
        GameObject UIWave = Instantiate(waveCout, canvas.transform.position, Quaternion.identity, canvas.transform);
        UIWave.GetComponentInChildren<TMP_Text>().text = "Wave " + wave.ToString();
        yield return new WaitForSeconds(8);
        Destroy(UIWave);

    }
}
