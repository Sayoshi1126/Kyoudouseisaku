using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBulletSpawner : MonoBehaviour
{
    [SerializeField] GameObject TrackingBullet;
    [SerializeField]Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject _g = Instantiate(TrackingBullet);
        _g.transform.position = this.transform.position + spawnPos;

        float _y = Input.GetAxis("Vertical");
        if (_y > 0.5f)
        {
            _g.transform.rotation = Quaternion.Euler(0, 0, 90);
            return;
        }
        else if (_y < -0.5f)
        {
            _g.transform.rotation = Quaternion.Euler(0, 0, -90);
            return;
        }
        float _x = Input.GetAxis("Horizontal");
        if (_x > 0.5f)
        {
            _g.transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        else if (_x < -0.5f)
        {
            _g.transform.rotation = Quaternion.Euler(0, 0, 180);
            return;
        }

        if (transform.localScale.x > 0.5f)
        {
            _g.transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        else if (transform.localScale.x < -0.5f)
        {
            _g.transform.rotation = Quaternion.Euler(0, 0, 180);
            return;
        }


    }

}
