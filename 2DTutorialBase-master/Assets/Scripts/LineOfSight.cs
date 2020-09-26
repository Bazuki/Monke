using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
      if (col.CompareTag("Player"))
      {
        GetComponentInParent<Enemy>().player = col.transform;
        Debug.Log("SEE PLAYER RUN AT PLAYER");
      }
    }
}
