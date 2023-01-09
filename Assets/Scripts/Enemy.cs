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
    public bool canfire = false;
    public bool isShoot = false;
    public float colldownShoot;
    public float bulletVel;
    public GameObject Bullet;
    [Header("Slash")]
    public GameObject katana;
    public GameObject linePrefab;
    public bool isSlash;
    public bool canSlash;
    public float maxAttacDist;
    public float colldownSlash;
    public float timeToAttack;
    public float timer = 0;
    [Header("Heal")]
    public bool isHeal;
    public float timeToDie;
    Player player;
    Rigidbody2D rb;
    Animator anim;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (isShoot)
        {
            StartCoroutine(ColldownShoot());
        }
        else if (isSlash)
        {
            canSlash = true;
        }
        if (isHeal)
        {
            Destroy(this.gameObject,timeToDie);
        }

    }

    void Update()
    {
        if (canfire && !fireing && isShoot) // aqui verifico se o player está dentro do collider trigger(canfire) e não está atirando (fireing)
        {
            Shoot();
        }
        if (isSlash)
        {

            if ((player.transform.position - transform.position).magnitude < maxAttacDist)
            {
                //rotation for katana
                Vector2 direction = player.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                katana.transform.rotation = Quaternion.Slerp(katana.transform.rotation, rotation, speed * Time.deltaTime);


                follow = false;
                timer += 0.01f;
                if (canSlash && timer > timeToAttack)
                {
                    SlashAttack();
                }
            }
            else
            {
                timer = 0;
                follow = true;
            }
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
            if ((player.transform.position - transform.position).magnitude < minDist)
            {
                rb.MovePosition(transform.position - (player.transform.position - transform.position).normalized * Time.deltaTime * speed);

            }
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity); // A bala é spawnada na posição da turret e com rotação normal
        bullet.GetComponent<Bullet>().speed = bulletVel;
        StartCoroutine(ColldownShoot());
    }
    public IEnumerator ColldownShoot() // Função que conta o tempo (nesse caso é o tempo de coldown).
    {
        fireing = true;                 // antes do Yield return colocamos ações que devem ser executadas antes do temporizador rolar.
        // print("se passaram 0 segundos");// executam no tempo 0
        yield return new WaitForSeconds(colldownShoot);
        // print("se passaram 3 segundos pode atirar"); // as ações colocadas depois são as que ocorrem depois dos tempo de coldown
        fireing = false;
    }
    public void SlashAttack()
    {
        StartCoroutine(ColldownSlash());
        rb.MovePosition(player.transform.position - (transform.position - player.transform.position).normalized * 1.5f);
        CreateLine();
    }
    public IEnumerator ColldownSlash()
    {
        timer = 0;
        canSlash = false;
        yield return new WaitForSeconds(colldownSlash);
        canSlash = true;
    }
    public void CreateLine()
    {
        GameObject currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.transform.position - (transform.position - player.transform.position).normalized * 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision) //função para detectar se o box collider do player está no circle colider da turret
    {
        if (collision.name == "Player")
        {
            canfire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//função para detectar se o box collider do player está saido do circle colider da turret
    {
        if (collision.name == "Player")
        {
            canfire = false;

        }
    }
}
