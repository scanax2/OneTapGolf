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
    [SerializeField] private float parabolaSpeed;

    private Rigidbody2D rb;
    private GameObject arc;
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
        launched = false;
        hit = false;
        rb.velocity = new Vector3(0.0f, 0.0f);
        transform.position = spawnPointTransform.position;
    }
    public void StartArc()
    {
        arc = Instantiate(arcTrajectoryPrefab, transform.position, Quaternion.identity);
        arc.transform.parent = transform;
        arc.GetComponent<LaunchArcRenderer>().StartTurn(parabolaSpeed);
    }
    public void DestroyArc()
    {
        Destroy(arc);
    }

    public void LaunchBall(Vector2 velocity)
    {
        rb.AddForce(velocity);
        StartCoroutine(SetIsLaunched());
    }

    private IEnumerator SetIsLaunched()
    {
        yield return new WaitForSeconds(0.25f);
        launched = true;
    }

    private void Update()
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
