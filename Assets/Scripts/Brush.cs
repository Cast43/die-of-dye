using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public float speed;
    Player player;
    Animator anim;
    void Start()
    {
        player = transform.GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

        anim.SetBool("Prepare", player.prepareAttack);
        anim.SetBool("Dash", player.dashAttack);



    }
}
