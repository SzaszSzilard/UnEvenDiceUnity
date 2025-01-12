using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class dice : MonoBehaviour
{
    Rigidbody body;

    [SerializeField] private float maxRandomForceValue, startRollingForce;
    private float forceX, forceY, forceZ;
    public int diceFaceNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (body != null) {
            if (Input.GetMouseButtonDown(0))
            {
                RollDice();
            }
        }
    }

    private void RollDice()
    {
        forceX = Random.Range(0, maxRandomForceValue);
        forceY = Random.Range(0, maxRandomForceValue);
        forceZ = Random.Range(0, maxRandomForceValue);

        body.AddForce(Vector3.up * startRollingForce, ForceMode.Acceleration);
        body.AddTorque(forceX, forceY, forceZ);

        Debug.Log(forceX + " " + forceY + " " + forceZ);
    }

    private void initialize()
    {
        startRollingForce = 100;
        maxRandomForceValue = 300;


        body = GetComponent<Rigidbody>();
        body.isKinematic = false;
        transform.rotation = new Quaternion(
            Random.Range(0, 360),
            Random.Range(0, 360),
            Random.Range(0, 360),
            0
        );
    }
}
