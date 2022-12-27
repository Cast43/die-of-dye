using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public bool stop;
    public bool follow;
    public float minDist;
    public float maxDist;

    [Header("Shoot")]
    public bool fireing = false;
    public bool Canfire = false;
    public float coldown;
    public GameObject Bullet;

    Player player;
    Rigidbody2D rb;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(colldownShoot());

    }

    void Update()
    {
        if (Canfire && !fireing) // aqui verifico se o player está dentro do collider trigger(Canfire) e não está atirando (fireing)
        {
            Shoot();
        }
    }
    void FixedUpdate()
    {
        if (follow)
        {

            if ((player.transform.position - transform.position).magnitude > maxDist)
            {
                rb.MovePosition(transform.position + (player.transform.position - transform.position).normalized * Time.deltaTime * speed);
            }
            else if ((player.transform.position - transform.position).magnitude < minDist)
            {
                rb.MovePosition(transform.position - (player.transform.position - transform.position).normalized * Time.deltaTime * speed);

            }
        }
    }

    public IEnumerator colldownShoot() // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        fireing = true;                 // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        // print("se passaram 0 segundos");// executam no tempo 0
        yield return new WaitForSeconds(coldown);
        // print("se passaram 3 segundos pode atirar"); // as ações colocadas depois são as que ocorrem depois dos tempo de coldown
        fireing = false;
    }
    void Shoot()
    {
        Instantiate(Bullet, transform.position, Quaternion.identity); // A bala é spawnada na posição da turret e com rotação normal
        StartCoroutine(colldownShoot());
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
