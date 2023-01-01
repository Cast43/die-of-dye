using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [Header("LifeInfo")]
    public int totalHealth;
    public int currentHealth;
    public bool damaged = false;
    public float timeToDamage = 1;
    public GameObject shield;
    public Color colorOfObj;
    [Header("UI")]
    public GameObject[] Lifes;
    public GameObject baseLife;
    public GameObject UIcontroller;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.name == "Player")
        {
            for (int i = 0; i < currentHealth; i++)
            {
                Lifes[i].SetActive(true);
            }
        }
        player = GameObject.Find("Player").GetComponent<Player>();

    }
    // utilizei a função awake pois a start não tava funcionando
    void Awake() // a função awake é a 1 função (antes do objeto ser instanciado) a rodar de um GameObject
    {
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Death()
    {
        if (transform.name == "Player")
        {
            transform.gameObject.SetActive(false);
            UIcontroller.GetComponent<UIController>().Death();
        }
        else
        {
            GameObject.Find("WaveController").GetComponent<WaveController>().numEnemysActual--;
            Destroy(this.gameObject);
        }
    }

    public IEnumerator TakeDmg(int damage) // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        damaged = true;                 // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        if (transform.name == "Player")
        {
            Lifes[currentHealth - 1].SetActive(false);
            StartCoroutine(ShowLife());
        }

        if (shield == null)
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
        GetComponent<SpriteRenderer>().color = Color.red; //ao ser atingido 
        yield return new WaitForSeconds(timeToDamage);
        GetComponent<SpriteRenderer>().color = colorOfObj; //ao ser atingido 
        damaged = false;
    }
    public IEnumerator ArmorDmg(int damage)
    {
        damaged = true;

        Life armor = shield.GetComponent<Life>();
        armor.currentHealth -= damage;
        if (armor.currentHealth < 1)
        {
            GameObject temp = Instantiate(shield, transform.position, Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = null;
            shield.transform.SetParent(temp.transform);
            GetComponent<Animator>().SetBool("NoShield", true);
            shield.GetComponent<Animator>().SetBool("Destroy", true);
            Destroy(temp, 5);
            shield = null;
        }
        yield return new WaitForSeconds(timeToDamage);
        damaged = false;

    }


    public IEnumerator ShowLife() // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        baseLife.SetActive(true);
        yield return new WaitForSeconds(2);
        baseLife.SetActive(false);

    }

}
