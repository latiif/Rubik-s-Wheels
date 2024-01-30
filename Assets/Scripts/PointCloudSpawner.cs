using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PointCloudSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    ARPointCloudManager pointCloudTracker;
    public GameObject doubleScorePrefab;
    public GameObject halveScorePrefab;
    public GameObject doubleTimePrefab;
    public GameObject halveTimePrefab;
    public GameObject speedBoostPrefab;
    private GameObject spawnedBox = null;

    public DrivingSurfaceManager drivingSurfaceManager;

    private const float SpawnInterval = 5f;
    private const float AutoDestroyTimeout = 3f;
    private float countdown = SpawnInterval;
    private Dictionary<string, GameObject> tag2prefabDict;
    private System.Random rng;
    void Awake()
    {
        pointCloudTracker = GetComponent<ARPointCloudManager>();

        tag2prefabDict = new Dictionary<string, GameObject>()
        {
            {"mb-double-score",doubleScorePrefab},
            {"mb-halve-score",halveScorePrefab},
            {"mb-double-time",doubleTimePrefab},
            {"mb-halve-time",halveTimePrefab},
            {"mb-speed-boost",speedBoostPrefab}
        };
        rng = new System.Random();
    }

    void OnEnable()
    {
        pointCloudTracker.pointCloudsChanged += pointCloudChangedEventHandler;
    }
    void OnDisable()
    {
        pointCloudTracker.pointCloudsChanged -= pointCloudChangedEventHandler;
    }

    void Update()
    {
        if (drivingSurfaceManager.TimeRemaining > 0)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
            }
            else
            {
                countdown = SpawnInterval;
            }
        }
    }

    void pointCloudChangedEventHandler(ARPointCloudChangedEventArgs e)
    {
        foreach (ARPointCloud cloudID in e.added.Concat(e.updated))
        {
            handleTracking(cloudID);
        }
    }

    void handleTracking(ARPointCloud cloud)
    {
        if (countdown > 0)
        {
            return;
        }
        // Vector3 position;
        GameObject gameObject;
        List<Vector3> points = new List<Vector3>();
        List<ulong> ids = new List<ulong>();
        List<float> confidences = new List<float>();

        foreach (Vector3 pos in cloud.positions)
        {
            points.Add(pos);
        }
        foreach (ulong id in cloud.identifiers)
        {
            ids.Add(id);
        }
        foreach (float confidence in cloud.confidenceValues)
        {
            confidences.Add(confidence);
        }

        float highestConfidence = confidences.Max();
        Debug.Log("MB Highest confidence is: " + highestConfidence);
        Debug.Log("MB points found: " + points.Count);

        for (int i = 0; i < points.Count; i++)
        {
            if (!spawnedBox)
            {
                if (confidences[i] == highestConfidence)
                {
                    string tag; GameObject prefab;
                    Tuple<string, GameObject> randomPrefab = getRandomPrefab();
                    tag = randomPrefab.Item1;
                    prefab = randomPrefab.Item2;

                    gameObject = GameObject.Instantiate(prefab);
                    //gameObject.tag = tag;
                    Debug.Log("MB Spawned box - " + tag + " | " + gameObject.tag + " | " + gameObject.gameObject.tag);
                    //gameObject.transform.position = points[i];
                    Vector3 boxPos = Vector3.ProjectOnPlane(points[i], drivingSurfaceManager.LockedPlane.infinitePlane.normal);
                    boxPos.y = drivingSurfaceManager.LockedPlane.center.y + 0.1f;
                    gameObject.transform.position = boxPos;
                    gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    spawnedBox = gameObject;
                    Invoke(nameof(DestroyGameObject), AutoDestroyTimeout);
                }
            }
        }
    }

    void DestroyGameObject()
    {
        Destroy(spawnedBox);
        Debug.Log("MB Destroyed Box");
        spawnedBox = null;
    }

    private Tuple<string, GameObject> getRandomPrefab()
    {
        string tag = tag2prefabDict.ElementAt(rng.Next(0, tag2prefabDict.Count)).Key;
        return Tuple.Create<string, GameObject>(tag, tag2prefabDict[tag]);
    }
}
