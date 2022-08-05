using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] float speed = 1500f;
    [SerializeField] Rigidbody2D rigidbody2d;

    void FixedUpdate()
    {
        if (isLocalPlayer)
            rigidbody2d.velocity = new Vector2(0, 
                Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime);
    }
}
