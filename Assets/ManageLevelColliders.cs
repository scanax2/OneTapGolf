using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventWithBool : UnityEvent<bool> { }

public class ManageLevelColliders : MonoBehaviour
{
    public UnityEventWithBool collidersSetActiveEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            collidersSetActiveEvent.Invoke(false);
            BallController.GetInstance().SetHit(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            collidersSetActiveEvent.Invoke(true);
            BallController.GetInstance().SetHit(false);
        }
    }
}
