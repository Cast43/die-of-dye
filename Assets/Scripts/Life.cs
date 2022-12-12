using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int TotalHealth;
    public int ActualHealth;
    public bool damaged = false;
    public float TimeToDamage = 1;
    // Start is called before the first frame update
    void Start()
    {

    }
    // utilizei a função awake pois a start não tava funcionando
    void Awake() // a função awake é a 1 função (antes do objeto ser instanciado) a rodar de um GameObject
    {
        ActualHealth = TotalHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
    public IEnumerator TakeDmg() // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        damaged = true;                 // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        ActualHealth--;
        GetComponent<SpriteRenderer>().color = Color.red; //ao ser atingido 
        yield return new WaitForSeconds(TimeToDamage);
        Debug.Log("sdasdad");
        GetComponent<SpriteRenderer>().color = Color.white; //ao ser atingido 
        damaged = false;

    }

}
