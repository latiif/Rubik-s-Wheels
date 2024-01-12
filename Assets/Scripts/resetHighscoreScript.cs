using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class resetHighscoreScript : MonoBehaviour
{
    Button resetButton;
    [SerializeField] private TMP_Text _resetText;

    // Start is called before the first frame update
    void Start()
    {
        _resetText.enabled = false;
        resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(delegate
        {
            PlayerPrefs.SetInt("high-score", 0);
            _resetText.text = "Highscore reset to 0";
            _resetText.enabled = true;
        });
    }

}
