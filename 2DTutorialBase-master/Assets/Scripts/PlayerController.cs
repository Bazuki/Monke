using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Attack_variables
    public float damage;
    public float attackspeed;
    float attacktimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currentHealth;
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
      PlayerRB = GetComponent<Rigidbody2D>();
      attacktimer = 0;
      anim = GetComponent<Animator>();
      currentHealth = maxHealth;

      HPSlider.value = currentHealth / maxHealth;
    }

    private void Update()
    {
      if (isAttacking)
      {
        return;
      }

      x_input = Input.GetAxisRaw("Horizontal");
      y_input = Input.GetAxisRaw("Vertical");

      Move();

      if (Input.GetKeyDown(KeyCode.Q) && attacktimer <= 0)
      {
        Attack();
      }
      else
      {
        attacktimer -= Time.deltaTime;
      }

      if (Input.GetKeyDown(KeyCode.E))
      {
        Interact();
      }

    }
    #endregion

    #region Movement_functions
    private void Move()
    {
      anim.SetBool("Moving", true);
      if (x_input > 0)
      {
        PlayerRB.velocity = Vector2.right * movespeed;
        currDirection = Vector2.right;
      }
      else if (x_input < 0)
      {
        PlayerRB.velocity = Vector2.left * movespeed;
        currDirection = Vector2.left;
      }
      else if (y_input > 0)
      {
        PlayerRB.velocity = Vector2.up * movespeed;
        currDirection = Vector2.up;
      }
      else if (y_input < 0)
      {
        PlayerRB.velocity = Vector2.down * movespeed;
        currDirection = Vector2.down;
      }
      else
      {
        PlayerRB.velocity = Vector2.zero;
        anim.SetBool("Moving", false);
      }
      anim.SetFloat("DirX", currDirection.x);
      anim.SetFloat("DirY", currDirection.y);
    }
    #endregion

    #region Attack_functions
    public void Attack()
    {
      Debug.Log("attacking now");
      Debug.Log(currDirection);
      attacktimer = attackspeed;
      // handles all attack animations and calculates hitboxes
      StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
      isAttacking = true;
      PlayerRB.velocity = Vector2.zero;
      anim.SetTrigger("Attacking");

      FindObjectOfType<AudioManager>().Play("PlayerAttack");

      yield return new WaitForSeconds(hitboxtiming);
      Debug.Log("casting hitbox now");
      RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

      foreach (RaycastHit2D hit in hits)
      {
        Debug.Log(hit.transform.name);
        if (hit.transform.CompareTag("Enemy"))
        {
          hit.transform.GetComponent<Enemy>().TakeDamage(damage);
          Debug.Log("tons of damage");
        }
      }

      yield return new WaitForSeconds(endanimationtiming);

      isAttacking = false;

      yield return null;
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float damage)
    {
      FindObjectOfType<AudioManager>().Play("PlayerHurt");

      currentHealth -= damage;
      Debug.Log("Health is now " + currentHealth.ToString());

      //change UI
      HPSlider.value = currentHealth / maxHealth;

      if (currentHealth <= 0)
      {
        Die();
      }
    }

    public void Heal(float healing)
    {
      currentHealth = Mathf.Min(currentHealth + healing, maxHealth);

      Debug.Log("Health is now " + currentHealth.ToString());
      HPSlider.value = currentHealth / maxHealth;
    }

    private void Die()
    {
      FindObjectOfType<AudioManager>().Play("PlayerDeath");

      // trigger anything to end the game
      Destroy(this.gameObject);

      GameObject gm = GameObject.FindWithTag("GameController");
      gm.GetComponent<GameManager>().LoseGame();

    }
    #endregion

    #region Interact_functions

    private void Interact()
    {
      RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, new Vector2 (0.5f, 0.5f), 0f, Vector2.zero, 0f);

      foreach (RaycastHit2D hit in hits)
      {
        if (hit.transform.CompareTag("Chest"))
        {
          hit.transform.GetComponent<Chest>().Interact();
        }
      }
    }

    #endregion
}
