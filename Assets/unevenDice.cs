using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class unevenDice : MonoBehaviour
{
    Rigidbody body;

    [SerializeField] private float maxRandomForceValue, startRollingForce;
    private float forceX, forceY, forceZ;
    public int experimentCount;

    static double totalArea = 14.6086 + 18.6866 + 19.5486 + 13.6163 + 17.1518 + 10.4375;
    private double[] probabilities = new double[] {
        (double) 14.6086 / totalArea,
        (double) 18.6866 / totalArea,
        (double) 19.5486 / totalArea,
        (double) 13.6163 / totalArea,        
        (double) 17.1518 / totalArea,
        (double) 10.4375 / totalArea

    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialize();
        printProbabilities(probabilities, true);
        printProbabilities(simulate(experimentCount), false); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (body != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RollDice();

                // Get the value of ExperimentCount input field and put it in experimentCounnt variable
                experimentCount = int.Parse(GameObject.Find("ExperimentCount").GetComponent<TMPro.TMP_InputField>().text);
                printProbabilities(simulate(experimentCount), false); ;
            }
        }
    }

    private double[] simulate(int numberOfRolls)
    {
        double[] diceFaceNum = new double[] { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < numberOfRolls; i++)
        {
            double random = Random.Range(0.0f, 1.0f);

            if (random < ((float)probabilities[0]))
            {
                diceFaceNum[0]++;
            }
            else if (random < probabilities[0] + probabilities[1])
            {
                diceFaceNum[1]++;
            }
            else if (random < probabilities[0] + probabilities[1] + probabilities[2])
            {
                diceFaceNum[2]++;
            }
            else if (random < probabilities[0] + probabilities[1] + probabilities[2] + probabilities[3])
            {
                diceFaceNum[3]++;
            }
            else if (random < probabilities[0] + probabilities[1] + probabilities[2] + probabilities[3] + probabilities[4])
            {
                diceFaceNum[4]++;
            }
            else
            {
                diceFaceNum[5]++;
            }
        }
        
        for (int i = 0; i < probabilities.Length; i++)
        {
            diceFaceNum[i] /= numberOfRolls;
        }

        return diceFaceNum;
    }

    private void printProbabilities(double[] probabilities, bool theoretical)
    {
        string scoreListText = "";
        scoreListText += "\n1: " + probabilities[0].ToString("0.0000");
        scoreListText += "\n2: " + probabilities[1].ToString("0.0000");
        scoreListText += "\n3: " + probabilities[2].ToString("0.0000");
        scoreListText += "\n4: " + probabilities[3].ToString("0.0000");
        scoreListText += "\n5: " + probabilities[4].ToString("0.0000");
        scoreListText += "\n6: " + probabilities[5].ToString("0.0000");

        if (theoretical)
        {
            GameObject.Find("Theoretical").GetComponent<TMPro.TextMeshProUGUI>().text = "Oldalak elmeleti valoszinűségéi:" + scoreListText;
        }
        else
        {
            GameObject.Find("Simulated").GetComponent<TMPro.TextMeshProUGUI>().text = "Oldalak gyakorlati valoszinűségéi:" + scoreListText;
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
        startRollingForce = 500;
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
