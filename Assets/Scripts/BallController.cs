using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(GameObject))]
public class BallController : MonoBehaviour
{
    private static BallController instance;

    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private GameObject arcTrajectoryPrefab;

    private Rigidbody2D rb;
    private GameObject arc;

    private float parabolaSpeed;

    private bool launched;
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

        rb = GetComponent<Rigidbody2D>();
        if (!spawnPointTransform)
        {
            spawnPointTransform = transform;
        }
    }
    public static BallController GetInstance()
    {
        if (instance == null)
        {
            instance = new BallController();
        }
        return instance;
    }
    public void SetParabolaSpeed(float speed)
    {
        parabolaSpeed = speed;
    }
    public void IncreaseParabolaSpeed(float speed)
    {
        parabolaSpeed += speed;
    }

    public void SetHit(bool flag)
    {
        hit = flag;
    }

    public void PlaceBall()
    {
        transform.eulerAngles = Vector3.zero;
        rb.velocity = new Vector2(0.0f, 0.0f);
        rb.angularVelocity = 0.0f;

        launched = false;
        hit = false;

        transform.position = spawnPointTransform.position;
    }
    public void StartArc()
    {
        arc = Instantiate(arcTrajectoryPrefab, transform.position, Quaternion.identity);
        arc.transform.parent = transform;
        arc.GetComponent<LaunchArcRenderer>().InitParabola(parabolaSpeed);
    }
    public void DestroyArc()
    {
        Destroy(arc);
    }

    public void LaunchBall(Vector2 velocity)
    {
        rb.AddForce(velocity);
        StartCoroutine(SetLaunched(true));
    }

    private IEnumerator SetLaunched(bool flag)
    {
        yield return new WaitForSeconds(0.25f);
        launched = flag;
    }

    private void Update()
    {
        if (GameController.GetInstance().GetState() == GameController.StateType.WAITINGBALL)
        {
            bool behindGolfHole = (GolfHoleController.GetInstance().GetPositionX() + 0.5f < transform.position.x);
            bool inFrontOfGolfHole = (launched && rb.velocity == new Vector2(0.0f, 0.0f));

            // Ball fails to hit 
            if ((inFrontOfGolfHole || behindGolfHole) && !hit)
            {
                GameController.GetInstance().SetState(GameController.StateType.GAMEOVER);
            }
        }
    }
}
