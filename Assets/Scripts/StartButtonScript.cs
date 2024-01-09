using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(1);
        });
    }

    // Update is called once per frame

}
