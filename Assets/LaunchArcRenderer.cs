using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaunchArcRenderer : MonoBehaviour
{
    [Header("Arc attributes")]
    [SerializeField] private float speedOfParabola = 1.0f;
    [SerializeField] private int resolution = 10;
    [SerializeField] float angle = 45;

    private bool display = false;
    private LineRenderer lr;

    private float g; // force of gravity on Y-axis
    [SerializeField] private float velocity;

    private UnityEvent launchBall;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonUp(0) && display)
        {
            DisplayParabola();
            velocity += speedOfParabola * Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0) && display)
        {
            display = false;

            Transform parent = transform.parent;
            transform.parent = null;

            // Launch
            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector2 resultVelocity = new Vector2(
                50.20458f * Mathf.Cos(angleInRadians) * velocity,
                50.20458f * Mathf.Sin(angleInRadians) * velocity
                );
            parent.gameObject.GetComponent<BallController>().LaunchBall(resultVelocity);

            GameController.GetInstance().SetState(GameController.StateType.BALLWAIT);
            Destroy(gameObject);
        }
    }

    public void StartTurn()
    {
        velocity = 2.0f;
        Debug.Log("Turn Launched !");
        display = true;
    }

    private void DisplayParabola()
    {
        lr.SetVertexCount(resolution + 1);
        lr.SetPositions(CalculateArcArray());
    }
    public Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        float radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance, radianAngle);
        }

        return arcArray;
    }

    private Vector3 CalculateArcPoint(float t, float maxDistance, float radianAngle)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }

    public void DestroyArc()
    {
        Destroy(gameObject);
    }
}
