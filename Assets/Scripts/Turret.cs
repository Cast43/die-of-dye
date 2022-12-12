using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool fireing = false;
    public bool Canfire = false;
    public float coldown;
     
    public GameObject Bullet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Canfire && !fireing) // aqui verifico se o player está dentro do collider trigger(Canfire) e não está atirando (fireing)
        {
            StartCoroutine(Shoot());
        }
    }

    public IEnumerator Shoot() // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        fireing = true;                 // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        print("se passaram 0 segundos");// executam no tempo 0
        Instantiate(Bullet,transform.position,Quaternion.identity); // A bala é spawnada na posição da turret e com rotação normal
        yield return new WaitForSeconds(coldown);
        print("se passaram 3 segundos pode atirar"); // as ações colocadas depois são as que ocorrem depois dos tempo de coldown
        fireing = false;

    }
    private void OnTriggerEnter2D(Collider2D collision) //função para detectar se o box collider do player está no circle colider da turret
    {
        if (collision.name == "Player")
        {
            Canfire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//função para detectar se o box collider do player está saido do circle colider da turret
    {
        if (collision.name == "Player")
        {
            Canfire = false;
            
        }
    }
}
