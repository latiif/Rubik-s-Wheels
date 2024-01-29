using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PointCloudSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    ARPointCloudManager pointCloudTracker;
    public GameObject mysteryBoxPrefab;
    private GameObject spawnedBox = null;

    public DrivingSurfaceManager drivingSurfaceManager;

    private const float SpawnInterval = 10f;
    private const float AutoDestroyTimeout = 5f;
    private float countdown = SpawnInterval;
    void Awake()
    {
        pointCloudTracker = GetComponent<ARPointCloudManager>();
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

        for (int i = 0; i < points.Count; i++)
        {
            if (!spawnedBox)
            {
                if (confidences[i] == highestConfidence)
                {
                    gameObject = GameObject.Instantiate(mysteryBoxPrefab);
                    gameObject.tag = "mystery-box";
                    gameObject.transform.position = points[i];
                    gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    spawnedBox = gameObject;
                    //confidences[i] = 0; // So that it doesnt get picked again
                    Invoke(nameof(DestroyGameObject), AutoDestroyTimeout);
                }
            }
        }
    }

    void DestroyGameObject()
    {
        Destroy(spawnedBox);
        spawnedBox = null;
    }
}
