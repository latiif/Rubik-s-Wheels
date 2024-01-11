using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleGroup : MonoBehaviour
{
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private GameObject[] _diffGroup;
    public void Toggle(Toggle toggle)
    {
        if(toggle.isOn)
        {
            switch (toggle.name)
            {
                case "Easy":
                    _difficultyText.text = "Difficulty is " + toggle.name;
                    PlayerPrefs.SetInt("difficulty", 0);
                    break;
                case "Medium":
                    _difficultyText.text = "Difficulty is " + toggle.name;
                    PlayerPrefs.SetInt("difficulty", 1);
                    break;
                case "Hard":
                    _difficultyText.text = "Difficulty is " + toggle.name;
                    PlayerPrefs.SetInt("difficulty", 2);
                    break;
                default:
                    _difficultyText.text = "Difficulty is " + toggle.name;
                    PlayerPrefs.SetInt("difficulty", 0);
                    break;
            }
        }
    }
}
