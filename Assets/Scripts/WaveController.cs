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
    public int maxInWave;
    public float multipliyer;
    [Header("EnemysInfo")]
    public GameObject[] enemys;
    public float maxPosInstX;
    public float maxPosInstY;
    [Header("HUD")]
    public GameObject canvas;
    public GameObject waveCout;
    public GameObject Dica;

    void Start()
    {
        if (wave == 0)
        {
            Tutorial();
        }

    }

    void Update()
    {
        if (!spawn && numSpawn < wave * 1.5f + 2 && numEnemysActual < maxInWave)
        {
            int enemyType = 0;
            int lucky = Random.Range(0, 50);
            if (lucky > 45)
            {
                enemyType = 3;
            }
            else
            {
                enemyType = Random.Range(0,enemys.Length-1);
            }

            if (wave == 5)
            {
                enemyType = 2;
            }
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
                GameObject enemy = Instantiate(enemys[type], new Vector2(maxPosInstX, Random.Range(maxPosInstY, -maxPosInstY)), Quaternion.identity);

                enemy.GetComponent<Enemy>().colldownShoot -= multipliyer * wave;
                enemy.GetComponent<Enemy>().colldownSlash += multipliyer * wave;
                enemy.GetComponent<Enemy>().maxAttacDist += multipliyer * wave;
                enemy.GetComponent<Enemy>().timeToAttack -= multipliyer * wave;
                enemy.GetComponent<Enemy>().bulletVel += multipliyer * wave;
                enemy.GetComponent<Enemy>().speed += multipliyer * wave;
                if (enemy.GetComponent<Life>().shield != null && wave % 2 == 5)
                {
                    enemy.GetComponent<Life>().shield.GetComponent<Life>().totalHealth++;
                }

            }
            else
            {
                GameObject enemy = Instantiate(enemys[type], new Vector2(-maxPosInstX, Random.Range(maxPosInstY, -maxPosInstY)), Quaternion.identity);
                enemy.GetComponent<Enemy>().colldownShoot -= multipliyer * wave;
                enemy.GetComponent<Enemy>().colldownSlash += multipliyer * wave;
                enemy.GetComponent<Enemy>().maxAttacDist += multipliyer * wave;
                enemy.GetComponent<Enemy>().timeToAttack -= multipliyer * wave;
                enemy.GetComponent<Enemy>().bulletVel += multipliyer * wave;
                enemy.GetComponent<Enemy>().speed += multipliyer * wave;
                if (enemy.GetComponent<Life>().shield != null && wave % 2 == 5)
                {
                    enemy.GetComponent<Life>().shield.GetComponent<Life>().totalHealth++;
                }

            }
        }
        else
        {
            if (Random.Range(1, 3) == 1)
            {
                GameObject enemy = Instantiate(enemys[type], new Vector2(Random.Range(maxPosInstX, -maxPosInstX), maxPosInstY), Quaternion.identity);
                enemy.GetComponent<Enemy>().colldownShoot -= multipliyer * wave;
                enemy.GetComponent<Enemy>().colldownSlash += multipliyer * wave;
                enemy.GetComponent<Enemy>().maxAttacDist += multipliyer * wave;
                enemy.GetComponent<Enemy>().timeToAttack -= multipliyer * wave;
                enemy.GetComponent<Enemy>().bulletVel += multipliyer * wave;
                enemy.GetComponent<Enemy>().speed += multipliyer * wave;
                if (enemy.GetComponent<Life>().shield != null && wave % 2 == 5)
                {
                    enemy.GetComponent<Life>().shield.GetComponent<Life>().totalHealth++;
                }

            }
            else
            {
                GameObject enemy = Instantiate(enemys[type], new Vector2(Random.Range(maxPosInstX, -maxPosInstX), -maxPosInstY), Quaternion.identity);
                enemy.GetComponent<Enemy>().colldownShoot -= multipliyer * wave;
                enemy.GetComponent<Enemy>().colldownSlash += multipliyer * wave;
                enemy.GetComponent<Enemy>().maxAttacDist += multipliyer * wave;
                enemy.GetComponent<Enemy>().timeToAttack -= multipliyer * wave;
                enemy.GetComponent<Enemy>().bulletVel += multipliyer * wave;
                enemy.GetComponent<Enemy>().speed += multipliyer * wave;
                if (enemy.GetComponent<Life>().shield != null && wave % 2 == 5)
                {
                    enemy.GetComponent<Life>().shield.GetComponent<Life>().totalHealth++;
                }

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
    private void Tutorial()
    {
        GameObject inimigo1 = Instantiate(enemys[1], new Vector2(0, -1), Quaternion.identity);
        GameObject inimigo2 = Instantiate(enemys[1], new Vector2(2, 1), Quaternion.identity);
        GameObject inimigo3 = Instantiate(enemys[1], new Vector2(-2, 1), Quaternion.identity);
        inimigo1.GetComponent<Enemy>().speed = 0;
        inimigo2.GetComponent<Enemy>().speed = 0;
        inimigo3.GetComponent<Enemy>().speed = 0;
        numEnemysActual = 3;
        numSpawn = 3;
        GameObject UIWave = Instantiate(waveCout, canvas.transform.position, Quaternion.identity, canvas.transform);
        UIWave.GetComponentInChildren<TMP_Text>().text = "Wave Tutorial";
        Destroy(UIWave, 8);
    }
}
