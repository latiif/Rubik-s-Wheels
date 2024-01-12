using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class resetHighscoreScript : MonoBehaviour
{
    Button resetButton;
    // Start is called before the first frame update
    void Start()
    {
        resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(delegate
        {
            PlayerPrefs.SetInt("high-score", 0);
        });
    }

}
