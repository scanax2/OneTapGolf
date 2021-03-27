using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool launched;

    private void Awake()
    {
        launched = false;
        rb = GetComponent<Rigidbody2D>();
    }

    public void LaunchBall(Vector2 velocity)
    {
        launched = true;
        rb.AddForce(velocity);
    }

    private void Update()
    {
        if (launched && rb.velocity == new Vector2(0.0f, 0.0f))
        {
            GameController.GetInstance().SetState(GameController.StateType.ENDTURN);
        }
    }
}
