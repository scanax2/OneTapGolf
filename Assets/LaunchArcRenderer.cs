using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaunchArcRenderer : MonoBehaviour
{
    [SerializeField] private int resolution = 10;
    [SerializeField] float angle = 45;

    private bool display = false;
    private LineRenderer lr;

    private float g; // force of gravity on Y-axis
    private float parabolaSpeed;

    [SerializeField] private float velocity;

    private UnityEvent launchBall;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    private void Update()
    {
        Vector3 farRightPoint = lr.GetPosition(lr.positionCount - 1);

        // Left click is holding down
        if (!Input.GetMouseButtonUp(0) && !IsOutOfBounds(farRightPoint) && display)
        {
            DisplayParabola();
            velocity += parabolaSpeed * Time.deltaTime;
        }
        // Left click is released or trajectory is out of bounds
        else if ((Input.GetMouseButtonUp(0) || IsOutOfBounds(farRightPoint)) && display)
        {
            display = false;

            Transform parent = transform.parent;
            transform.parent = null;

            // Launch ball
            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector2 resultVelocity = new Vector2(
                50.20458f * Mathf.Cos(angleInRadians) * velocity,
                50.20458f * Mathf.Sin(angleInRadians) * velocity
                );
            parent.gameObject.GetComponent<BallController>().LaunchBall(resultVelocity);

            GameController.GetInstance().SetState(GameController.StateType.WAITING);
        }
    }
    private bool IsOutOfBounds(Vector3 position)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1.3;
        return !onScreen;
    }

    public void StartTurn(float speed)
    {
        parabolaSpeed = speed;
        velocity = 2.0f;
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
            float t = (float)i/resolution;
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
