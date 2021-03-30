using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfHoleController : MonoBehaviour
{
    private static GolfHoleController instance;

    [SerializeField] private float spawnLength;  // length on X-axis
    private Transform spawnPointTransform;
    private Vector3 defaultSpawnPosition;
    private bool hit;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        if (!spawnPointTransform)
        {
            spawnPointTransform = transform;
        }
        defaultSpawnPosition = spawnPointTransform.position;
    }
    public static GolfHoleController GetInstance()
    {
        if (instance == null)
        {
            instance = new GolfHoleController();
        }
        return instance;
    }

    public float GetPositionX()
    {
        return transform.position.x;
    }

    public void PlaceFlag()
    {
        hit = false;
        float randomX = Random.Range(0, spawnLength);
        Vector3 spawnPosition = new Vector3(
            defaultSpawnPosition.x + randomX,
            defaultSpawnPosition.y,
            defaultSpawnPosition.z);
        transform.position = spawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ball hit the hole
        if (collision.tag == "Ball" && !hit)
        {
            hit = true;
            GameController.GetInstance().SetState(GameController.StateType.ENDROUND);
        }
    }

    // For debug, optional
    private void OnDrawGizmosSelected()
    {
        if (!spawnPointTransform)
        {
            defaultSpawnPosition = transform.position;
        }
        Vector3 startPointPosition = new Vector3(
                defaultSpawnPosition.x,
                defaultSpawnPosition.y + 0.5f,
                defaultSpawnPosition.z); ;

        Vector3 endPointPosition = new Vector3(
            startPointPosition.x + spawnLength, 
            startPointPosition.y, 
            startPointPosition.z); 

        // Draws a blue line from this transform to the target
        Gizmos.color = Color.white;
        Debug.DrawLine(startPointPosition, endPointPosition, Color.white, Time.deltaTime);
    }
}
