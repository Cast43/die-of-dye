using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTrails : MonoBehaviour
{
    Player player;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D collision) //função para detectar se o box collider do player está no polygon colider
    {
        if (collision.tag == "Enemy" && collision.GetComponent<Life>().currentHealth > 0 && !collision.isTrigger)
        {
            collision.GetComponent<Life>().StartCoroutine("TakeDmg", player.dashDmg);

        }
    }
    IEnumerator TakePoint()
    {
        yield return new WaitForSeconds(0);

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            yield return new WaitForSeconds(1.5f);
            lineRenderer.SetPosition(i, Vector3.zero);
            print("asdasd");
        }

        Destroy(this.gameObject);

    }

}
