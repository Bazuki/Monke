using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionPrefab;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currentHealth;
    #endregion

    #region Unity_functions
      // runs once on creation
    private void Awake()
    {
      EnemyRB = GetComponent<Rigidbody2D>();
      currentHealth = maxHealth;
    }

      // runs every frame
    private void Update()
    {
      if (player == null)
      {
        return;
      }

      Move();
    }
    #endregion

    #region Movement_functions
    private void Move()
    {
      Vector2 direction = player.position - transform.position;
      EnemyRB.velocity = direction.normalized * movespeed;
    }
    #endregion

    #region Attack_functions
    private void Explode()
    {

      FindObjectOfType<AudioManager>().Play("Explosion");

      RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);

      foreach (RaycastHit2D hit in hits)
      {
        if (hit.transform.CompareTag("Player"))
        {
          // cause damage
          Debug.Log("Hit player with explosion");
          hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
          Instantiate(explosionPrefab, transform.position, transform.rotation);

        }
      }

      Destroy(this.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
      if (col.transform.CompareTag("Player"))
      {
        Explode();
      }
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {
      FindObjectOfType<AudioManager>().Play("BatHurt");

      currentHealth -= value;

      Debug.Log("Enemy health is now:" + currentHealth.ToString());
      if (currentHealth <= 0)
      {
        Die();
      }
    }

    private void Die()
    {
      Destroy(this.gameObject);
    }
    #endregion
}
