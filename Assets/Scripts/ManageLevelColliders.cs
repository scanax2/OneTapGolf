using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ManageLevelColliders : MonoBehaviour
{
    // Deactivate colliders while the ball is in the trigger
    public UnityEventWithBool collidersDeactivateEvent;
    // Activate colliders while the ball is in the trigger
    public UnityEventWithBool collidersActivateEvent;

    private void Start()
    {
        collidersDeactivateEvent.Invoke(true);
        collidersActivateEvent.Invoke(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            collidersDeactivateEvent.Invoke(false);
            collidersActivateEvent.Invoke(true);
            BallController.GetInstance().SetHit(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            collidersDeactivateEvent.Invoke(true);
            collidersActivateEvent.Invoke(false);
            BallController.GetInstance().SetHit(false);
        }
    }
}
