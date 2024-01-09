using UnityEngine;
using UnityEngine.UI;

public class DropDownScript : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        //Fetch the Dropdown GameObject
        slider = GetComponent<Slider>();
        //Add listener for when the value of the Dropdown changes, to take action
        slider.onValueChanged.AddListener(delegate
        {

            Debug.Log("DIFF CHANGED");
            DropdownValueChanged(slider);
        });

        Debug.Log("START DIFF CHANGED");
        PlayerPrefs.SetInt("difficulty", 0);
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Slider change)
    {
        Debug.Log("DIFF CHANGED" + change.value);
        PlayerPrefs.SetInt("difficulty", (int)change.value);
    }
}
