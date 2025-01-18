using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class unevenDice : MonoBehaviour
{
    Rigidbody body;

    [SerializeField] private float maxRandomForceValue, startRollingForce;
    private float forceX, forceY, forceZ;
    public int experimentCount;

    static double[] areas =
        new double[] {
            10.4375,
            17.1518,
            13.6163,
            19.5486,
            18.6866,
            14.6086
    };

    private double[] probabilities;

    Vector3 A, B, C, D, E, F, G, H;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialize();
        printProbabilities(probabilities, true);
        printProbabilities(simulate(experimentCount), false);
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
        setPeaks();
        
        double[] sideVolumes = calcSideVolumes();
        double totalVolume = sideVolumes.Sum();
        double[] oppositeProbabilities = sideVolumes.Select(x => x / totalVolume).ToArray();
        probabilities = oppositeProbabilities.Reverse().ToArray();

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

    private double[] calcSideVolumes()
    {
        Vector3 G1 = A + F / 2;
        Vector3 G2 = B + E / 2;
        Vector3 G3 = C + G / 2;
        Vector3 G4 = D + H / 2;
        Vector3 GTestAtlok = (G1 + G2 + G3 + G4) / 4;

        Vector3 side1mass = calcCenterOfMass2D(C, D, E, F);
        Vector3 side2mass = calcCenterOfMass2D(A, D, E, G);
        Vector3 side3mass = calcCenterOfMass2D(A, B, C, D);
        Vector3 side4mass = calcCenterOfMass2D(E, F, G, H);
        Vector3 side5mass = calcCenterOfMass2D(B, C, F, H);
        Vector3 side6mass = calcCenterOfMass2D(A, B, G, H);

        double side1Height = Vector3.Distance(side1mass, GTestAtlok);
        double side2Height = Vector3.Distance(side2mass, GTestAtlok);
        double side3Height = Vector3.Distance(side3mass, GTestAtlok);
        double side4Height = Vector3.Distance(side4mass, GTestAtlok);
        double side5Height = Vector3.Distance(side5mass, GTestAtlok);
        double side6Height = Vector3.Distance(side6mass, GTestAtlok);

        Debug.Log("Side 1 height: " + side1Height);
        Debug.Log("Side 2 height: " + side2Height);
        Debug.Log("Side 3 height: " + side3Height);
        Debug.Log("Side 4 height: " + side4Height);
        Debug.Log("Side 5 height: " + side5Height);
        Debug.Log("Side 6 height: " + side6Height);

        return new double[] {
            areas[0] * side1Height / 3,
            areas[1] * side2Height / 3,
            areas[2] * side3Height / 3,
            areas[3] * side4Height / 3,
            areas[4] * side5Height / 3,
            areas[5] * side6Height / 3
        };
    }

    private Vector3 calcCenterOfMass2D(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4) {
        return (P1 + P2 + P3 + P4) / 4;
    }

    private void setPeaks()
    {
        A = GameObject.Find("A").transform.position;
        B = GameObject.Find("B").transform.position;
        C = GameObject.Find("C").transform.position;
        D = GameObject.Find("D").transform.position;
        E = GameObject.Find("E").transform.position;
        F = GameObject.Find("F").transform.position;
        G = GameObject.Find("G").transform.position;
        H = GameObject.Find("H").transform.position;
    }
}
