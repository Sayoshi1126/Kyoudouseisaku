using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstSelectButton : MonoBehaviour
{
    public Button DefaultButton;
    private void Start()
    {
        DefaultButton.Select();
    }
}