using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÚG‚µ‚½•¨‘Ì‚ª‰½‚©’²‚×‚é
/// </summary>
public class CheckColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D TriggerEnterTag;
    public Collider2D TriggerExitTag;
    public bool OrTriggerStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnterTag = collision;
        OrTriggerStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExitTag = collision;
        OrTriggerStay = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OrTriggerStay = true;
    }
}
