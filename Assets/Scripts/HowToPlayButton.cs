using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowToPlayButton : MonoBehaviour
{
    Button HTPButton;
    void Start()
    {
        HTPButton = GetComponent<Button>();
        HTPButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(2);
        });
    }
}
