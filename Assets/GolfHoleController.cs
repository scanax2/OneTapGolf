using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfHoleController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnLength;  // length on X-axis

    private void Awake()
    {
        if (!spawnPoint)
        {
            spawnPoint = transform;
        }
    }

    public void PlaceFlag()
    {
        Debug.Log("Flag placed !");
        float randomX = Random.Range(0, spawnLength);
        Vector3 spawnPosition = new Vector3(
            spawnPoint.position.x + randomX,
            spawnPoint.position.y,
            spawnPoint.position.z);
        transform.position = spawnPosition;
    }

    // Debug, optional
    private void OnDrawGizmosSelected()
    {
        Vector3 startPointPosition;
        try
        {
            startPointPosition = new Vector3(
                spawnPoint.position.x,
                spawnPoint.position.y + 0.5f,
                spawnPoint.position.z);
        } catch(UnassignedReferenceException ex)
        {
            spawnPoint = transform;
            startPointPosition = new Vector3(
                spawnPoint.position.x,
                spawnPoint.position.y + 0.5f,
                spawnPoint.position.z);
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
