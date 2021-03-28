using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfHoleController : MonoBehaviour
{
    private static GolfHoleController instance;

    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private float spawnLength;  // length on X-axis

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
        Debug.Log("Flag placed !");
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
            Debug.Log("Hit !");
            hit = true;
            GameController.GetInstance().SetState(GameController.StateType.ENDTURN);
        }
    }

    // Debug, optional
    private void OnDrawGizmosSelected()
    {
        Vector3 startPointPosition;
        try
        {
            startPointPosition = new Vector3(
                defaultSpawnPosition.x,
                defaultSpawnPosition.y + 0.5f,
                defaultSpawnPosition.z);
        } catch(UnassignedReferenceException ex)
        {
            spawnPointTransform = transform;
            startPointPosition = new Vector3(
                defaultSpawnPosition.x,
                defaultSpawnPosition.y + 0.5f,
                defaultSpawnPosition.z);
            Debug.Log("Exception: spawn point is not assigned !");
        }
        Vector3 endPointPosition = new Vector3(
            startPointPosition.x + spawnLength, 
            startPointPosition.y, 
            startPointPosition.z); 

        // Draws a blue line from this transform to the target
        Gizmos.color = Color.white;
        Debug.DrawLine(startPointPosition, endPointPosition, Color.white, Time.deltaTime);
    }
}
