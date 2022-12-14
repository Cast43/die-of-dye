using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int totalHealth;
    public int currentHealth;
    public bool damaged = false;
    public float timeToDamage = 1;
    public Color colorOfObj;
    // Start is called before the first frame update
    void Start()
    {

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
        Destroy(this.gameObject);
    }
    public IEnumerator TakeDmg(int damage) // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        damaged = true;                 // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Death();
        }
        GetComponent<SpriteRenderer>().color = Color.red; //ao ser atingido 
        yield return new WaitForSeconds(timeToDamage);
        Debug.Log("sdasdad");
        GetComponent<SpriteRenderer>().color = colorOfObj; //ao ser atingido 
        damaged = false;

    }

}
