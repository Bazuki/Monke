using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    #region Healthpack_Variables
    [SerializeField]
    [Tooltip("the amount the player heals")]
    private int healAmount;
    #endregion

    #region Heal_functions
    private void OnTriggerEnter2D(Collider2D col)
    {
      if (col.transform.CompareTag("Player"))
      {
        col.transform.GetComponent<PlayerController>().Heal(healAmount);
        Destroy(this.gameObject);
      }
    }
    #endregion
}
