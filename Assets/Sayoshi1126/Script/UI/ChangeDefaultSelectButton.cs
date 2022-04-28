using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDefaultSelectButton : MonoBehaviour
{
    public Button DefaultButton;
    void OnEnable()
    {
        DefaultButton.Select();
    }
}
