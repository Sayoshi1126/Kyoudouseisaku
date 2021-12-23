using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    [SerializeField] Jumper jumper;
    void Update()
    {
        if(jumper.watchTime )
        {
            Time.timeScale = 0.4f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
