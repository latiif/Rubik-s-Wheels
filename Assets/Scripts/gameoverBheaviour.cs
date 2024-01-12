using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameoverBheaviour : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI highScoreText;
    void Start()
    {
        int score = PlayerPrefs.GetInt("score");
        int highestScore = PlayerPrefs.GetInt("high-score");

        scoreText.text = score + "!";

        if (score == highestScore)
        {
            highScoreText.enabled = true;
        }
        else
        {
            highScoreText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
