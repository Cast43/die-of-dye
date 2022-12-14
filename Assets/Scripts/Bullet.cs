using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public int damage;
    public Rigidbody2D rb;
    public GameObject target; //defino onde será armazenado o GM(GameObject)
    void Start()
    {
        target = GameObject.Find("Player"); // utilizo o sistema de encontrar o objeto pelo o nome(não recomendavel ja que os objetos podem 
                                            // ter seus nomes alterados)
        rb = GetComponent<Rigidbody2D>();

        Vector2 playerPos = new Vector2(target.transform.position.x, target.transform.position.y);// armazenamento da posição do vetor X e Y do player 
                                                                                                  // em um vetor utilizando o GM target anterior
        Vector2 distance = playerPos - rb.position;// gerar um vetor calculando a posição final(posição do player) menos a inicial(posição da turret)
        rb.velocity = distance.normalized * speed;//a pós isso  normalizar o vetor distancia e multiplicar pela velocidade. e atribuir a sua vel a isso
        Destroy(this.gameObject, 5); // destruir o GameObject após 5 segundos da sua instanciação
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision) //função para detectar se o circle colider da bala entrou em contato com o collider do player
    {
        if (collision.name == "Player")
        {
            // essa linha pega o script vida do objeto colidido (que nesse caso é o objeto com tag player)
            if (!collision.GetComponent<Life>().damaged)
            {
                collision.GetComponent<Life>().StartCoroutine(collision.GetComponent<Life>().TakeDmg(damage));
            }
            else
            {
                Destroy(this.gameObject, 3);
                return;
            }
        }
        if(collision.tag == "Enemy")
        {
            return;
        }
        Destroy(this.gameObject);

    }
}
