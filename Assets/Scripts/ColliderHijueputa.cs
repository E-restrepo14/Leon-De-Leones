using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHijueputa : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.tag + collision.transform.name);
        if (collision.transform.CompareTag("espadaTag"))
        {
            print("collider hijueputa");
        }
    }
}
