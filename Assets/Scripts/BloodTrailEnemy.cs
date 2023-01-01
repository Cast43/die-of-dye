using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTrailEnemy : MonoBehaviour
{
    public float timeToatcivate;
    public int damage;
    public float duration;
    public Color activeColor;
    LineRenderer line;
    EdgeCollider2D edgeCol;
    public List<Vector2> points;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        edgeCol = GetComponent<EdgeCollider2D>();
        StartCoroutine(Activate());
    }

    void Update()
    {

    }
    public IEnumerator Activate()
    {

        yield return new WaitForSeconds(timeToatcivate);
        points[0] = line.GetPosition(0);
        points[1] = line.GetPosition(1);
        edgeCol.SetPoints(points);
        line.endColor = activeColor;
        line.startColor = activeColor;
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            collision.GetComponent<Life>().StartCoroutine(collision.GetComponent<Life>().TakeDmg(damage));
        }
    }
}
