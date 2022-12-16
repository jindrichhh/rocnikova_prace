using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    [SerializeField]
    Slider VisualProgress;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }



    // Sets UI progress value <0;1>
    public void SetProgress(float value) {

        VisualProgress.value = value;
        //Debug.Log(value);
    }

    // Sets ratio value of progress bar (current / capacity)
    public void SetProgress(float current, float capacity) {

        //Debug.Log("cur: " + current);
        //Debug.Log("cap: " + capacity);
        SetProgress(current / capacity);
    }
}
