using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackToMenuScript : MonoBehaviour
{
    Button BackButton;
    void Start()
    {
        BackButton = GetComponent<Button>();
        BackButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(0);
        });
    }
}
