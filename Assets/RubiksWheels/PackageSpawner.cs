﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class PackageSpawner : MonoBehaviour
{
    public DrivingSurfaceManager DrivingSurfaceManager;
    public PackageBehaviour Package;
    public GameObject PackagePrefab;



    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;

    public float increment = 5;
    public bool timerIsRunning = false;

    private void Start()
    {
        int difficulty = PlayerPrefs.GetInt("difficulty");

        switch (difficulty)
        {
            case 0:
                DrivingSurfaceManager.TimeRemaining = 15;
                increment = 5;
                break;
            case 1:
                DrivingSurfaceManager.TimeRemaining = 10;
                increment = 4;
                break;
            case 2:
                DrivingSurfaceManager.TimeRemaining = 5;
                increment = 2;
                break;
        }
        timeText.text = DrivingSurfaceManager.TimeRemaining.ToString("0.00");
    }
    public static Vector3 RandomInTriangle(Vector3 v1, Vector3 v2)
    {
        float u = Random.Range(0.0f, 1.0f);
        float v = Random.Range(0.0f, 1.0f);
        if (v + u > 1)
        {
            v = 1 - v;
            u = 1 - u;
        }

        return (v1 * u) + (v2 * v);
    }

    public static Vector3 FindRandomLocation(ARPlane plane)
    {
        // Select random triangle in Mesh
        var mesh = plane.GetComponent<ARPlaneMeshVisualizer>().mesh;
        var triangles = mesh.triangles;
        var triangle = triangles[(int)Random.Range(0, triangles.Length - 1)] / 3 * 3;
        var vertices = mesh.vertices;
        var randomInTriangle = RandomInTriangle(vertices[triangle], vertices[triangle + 1]);
        var randomPoint = plane.transform.TransformPoint(randomInTriangle);

        Debug.Log("Random Point:" + randomPoint);
        //randomPoint.y = 0;
        return randomPoint;
    }

    public void SpawnPackage(ARPlane plane)
    {
        var packageClone = GameObject.Instantiate(PackagePrefab);

        packageClone.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        packageClone.transform.position = FindRandomLocation(plane);

        Package = packageClone.GetComponent<PackageBehaviour>();
    }

    private void Update()
    {
        var lockedPlane = DrivingSurfaceManager.LockedPlane;
        if (lockedPlane != null)
        {
            if (Package == null)
            {
                SpawnPackage(lockedPlane);
                if (!timerIsRunning)
                {
                    timerIsRunning = true;
                }
                updateScore();
            }
            else
            {
                Package.transform.Rotate(Vector3.up * (50f * Time.deltaTime));

            }

            var packagePosition = Package.gameObject.transform.position;
            packagePosition.Set(packagePosition.x, lockedPlane.center.y, packagePosition.z);
        }
        if (timerIsRunning)
        {
            if (DrivingSurfaceManager.TimeRemaining > 0)
            {
                DrivingSurfaceManager.TimeRemaining -= Time.deltaTime;
                updateTime();
            }
            else
            {
                timerIsRunning = false;
                Debug.Log("Time is up");
                int currentScore = DrivingSurfaceManager.Score;
                int highestScore = PlayerPrefs.GetInt("high-score", 0);

                if (currentScore > highestScore)
                {
                    highestScore = currentScore;
                }

                PlayerPrefs.SetInt("score", currentScore);
                PlayerPrefs.SetInt("high-score", highestScore);
                SceneManager.LoadScene(3);
            }
        }
    }

    private void updateScore()
    {
        if (timerIsRunning)
        {
            scoreText.text = "Score: " + ++DrivingSurfaceManager.Score;
            DrivingSurfaceManager.TimeRemaining += increment;
        }
    }

    private void updateTime()
    {
        if (timerIsRunning)
        {
            timeText.text = "" + DrivingSurfaceManager.TimeRemaining.ToString("0.00") + "s";
            scoreText.text = "Score: " + DrivingSurfaceManager.Score;
        }
    }
}
