using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] Rigidbody2D rigidbody2d;
    [SerializeField] GameObject particlePrefab; //��
    public static event Action<string> OnGoal;

    public override void OnStartServer()
    {
        rigidbody2d.simulated = true;
        rigidbody2d.velocity = Vector2.right * speed;
    }

    [ServerCallback]
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.GetComponent<Player>()) return;
        float x = rigidbody2d.velocity.x > 0 ? -1 : 1;
        float y = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
        Vector2 direction = new Vector2(x, y).normalized;
        rigidbody2d.velocity = direction * speed;

        //��
        var particle = Instantiate(particlePrefab, transform.position, transform.rotation);
        NetworkServer.Spawn(particle);
        StartCoroutine(DestroyParticles(particle));
    }

    IEnumerator DestroyParticles(GameObject particle) //��
    {
        yield return new WaitForSeconds(2f);
        NetworkServer.Destroy(particle);
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Left":
            case "Right":
                OnGoal?.Invoke(collision.tag);
                break;
        }
    }
}
