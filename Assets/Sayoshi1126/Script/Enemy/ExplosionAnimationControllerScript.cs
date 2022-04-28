using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ExplosionAnimationControllerScript : MonoBehaviour
{
    [SerializeField] float AnimationFrameLength;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BreakEffect", AnimationFrameLength);
    }

    void BreakEffect()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
}
