using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public ReticleBehaviour Reticle;
    private const float originalSpeed = 1.2f;
    private const float speedBoostDuration = 5f;
    public float Speed = originalSpeed;
    public DrivingSurfaceManager drivingSurfaceManager;

    private bool hasCollided = false;

    private Dictionary<string, string> mysteryBoxHandler;

    private void Start()
    {
        mysteryBoxHandler = new Dictionary<string, string>()
        {
            {"mb-double-score",nameof(handleDoubleScore)},
            {"mb-halve-score",nameof(handleHalveScore)},
            {"mb-double-time",nameof(handleDoubleTime)},
            {"mb-halve-time",nameof(handleHalveTime)},
            {"mb-speed-boost",nameof(handleSpeedBoost)}
        };
    }
    private void Update()
    {
        var trackingPosition = Reticle.transform.position;
        if (Vector3.Distance(trackingPosition, transform.position) < 0.1)
        {
            return;
        }

        var lookRotation = Quaternion.LookRotation(trackingPosition - transform.position);
        transform.rotation =
            Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        transform.position =
            Vector3.MoveTowards(transform.position, trackingPosition, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag != "Untagged")
        {
            Debug.Log("MB collision with " + other.gameObject.tag);
        }
        if (!hasCollided && mysteryBoxHandler.Keys.Contains(other.gameObject.tag))
        {
            hasCollided = true;
            Invoke(nameof(enableCollectingMysteryBox), 2f);
            Debug.Log("MB Bumped into a mystery box:" + other.gameObject.tag);
            Invoke(mysteryBoxHandler[other.gameObject.tag], 0f);
            Destroy(other.gameObject);
            return;
        }
        var Package = other.GetComponent<PackageBehaviour>();
        if (Package != null)
        {
            Destroy(other.gameObject);
        }
    }

    private void enableCollectingMysteryBox()
    {
        hasCollided = false;
    }

    private void handleDoubleScore()
    {
        drivingSurfaceManager.Score *= 2;
    }
    private void handleDoubleTime()
    {
        drivingSurfaceManager.TimeRemaining *= 2;
    }

    private void handleHalveScore()
    {
        drivingSurfaceManager.Score /= 2;
    }
    private void handleHalveTime()
    {
        drivingSurfaceManager.TimeRemaining /= 2;
    }

    private void handleSpeedBoost()
    {
        Speed *= 1.25f; // an increment of 25%
        Invoke(nameof(resetSpeed), speedBoostDuration); // Reset speed to normal after 'speedBoostDuration' seconds
    }

    private void resetSpeed()
    {
        Speed = originalSpeed;
    }


}
