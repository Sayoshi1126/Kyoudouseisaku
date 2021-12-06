using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    // Start is called before the first frame update

    public enum func
    {
        none,
        jump,
        toggle_show_trail,
        toggle_show_camera_maker,
        new_style,
        previous_style,
        pause,
        step_forward
    }

    [SerializeField] func [] joyStickButton = new func[9];

    [HideInInspector] public bool jumpButtonDown;
    [HideInInspector] public bool jumpButtonUp;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        jumpButtonDown = false;
        jumpButtonUp = false;

        if (Input.GetKeyDown("joystick button 0"))
        {
            UseFunctionButton(joyStickButton[0]);
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("button1");
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("button2");
        }
        if (Input.GetKeyDown("joystick button 3"))
        {
            Debug.Log("button3");
        }
        if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("button4");
        }
        if (Input.GetKeyDown("joystick button 5"))
        {
            Debug.Log("button5");
        }
        if (Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("button6");
        }
        if (Input.GetKeyDown("joystick button 7"))
        {
            Debug.Log("button7");
        }
        if (Input.GetKeyDown("joystick button 8"))
        {
            Debug.Log("button8");
        }
        if (Input.GetKeyDown("joystick button 9"))
        {
            Debug.Log("button9");
        }

        if (Input.GetKeyUp("joystick button 0"))
        {
            FalseFunctionButton(joyStickButton[0]);
        }
        if (Input.GetKeyUp("joystick button 1"))
        {
            Debug.Log("button1");
        }
        if (Input.GetKeyUp("joystick button 2"))
        {
            Debug.Log("button2");
        }
        if (Input.GetKeyUp("joystick button 3"))
        {
            Debug.Log("button3");
        }
        if (Input.GetKeyUp("joystick button 4"))
        {
            Debug.Log("button4");
        }
        if (Input.GetKeyUp("joystick button 5"))
        {
            Debug.Log("button5");
        }
        if (Input.GetKeyUp("joystick button 6"))
        {
            Debug.Log("button6");
        }
        if (Input.GetKeyUp("joystick button 7"))
        {
            Debug.Log("button7");
        }
        if (Input.GetKeyUp("joystick button 8"))
        {
            Debug.Log("button8");
        }
        if (Input.GetKeyUp("joystick button 9"))
        {
            Debug.Log("button9");
        }

        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if ((hori != 0) || (vert != 0))
        {
            //Debug.Log("stick:" + hori + "," + vert);
        }
    }

    void UseFunctionButton(func function)
    {
        if (function == func.none)
        {

        } else if (function == func.jump)
        {
            jumpButtonDown = true;
        }
        else if (function == func.new_style)
        {

        } else if (function == func.pause)
        {

        } else if (function == func.previous_style)
        {

        } else if (function == func.step_forward)
        {

        } else if (function == func.toggle_show_camera_maker)
        {

        } else if (function == func.toggle_show_trail)
        {

        }
    }

    void FalseFunctionButton(func function)
    {
        if (function == func.none)
        {

        }
        else if (function == func.jump)
        {
            jumpButtonUp = true;
        }
        else if (function == func.new_style)
        {

        }
        else if (function == func.pause)
        {

        }
        else if (function == func.previous_style)
        {

        }
        else if (function == func.step_forward)
        {

        }
        else if (function == func.toggle_show_camera_maker)
        {

        }
        else if (function == func.toggle_show_trail)
        {

        }
    }

    public bool CheckButtonDown(func function)
    {
        for (int i = 0; i<joyStickButton.Length ; i++)
        {
            if(joyStickButton[i]==function&&Input.GetKey("joystick button " + joyStickButton.ToString()))
            {
                return true;
            }
        }
        return false;
    }
}
