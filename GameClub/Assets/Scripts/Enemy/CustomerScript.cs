using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerScript : MonoBehaviour
{

    NavMeshAgent agent;
    public float speed;
    public float health;
    public float damage;
    public float attackRadius;
    public float attackTime;
    public int type;
    public GameObject target;
    public Vector3 targetLocation;

    public Material[] skins = new Material[4];
    public GameObject deathEffect;

    public bool atTarget;
    private float timer;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetComponentInChildren<SkinnedMeshRenderer>().material = skins[Random.Range(0, skins.Length)];
        animator.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(new Vector2(targetLocation.x, targetLocation.z), new Vector2(transform.position.x, transform.position.z)) < attackRadius)
        {
            animator.SetBool("isRunning", false);
            agent.ResetPath();
            float currentX = transform.rotation.eulerAngles.x;
            float currentZ = transform.rotation.eulerAngles.z;
            transform.LookAt(target.transform.position);
            transform.rotation = Quaternion.Euler(currentX, transform.rotation.eulerAngles.y, currentZ);
            timer += Time.deltaTime;
            if (timer > attackTime)
            {
                timer = 0;
                StartCoroutine(DealDamage());
            }
        }

        if (health <= 0)
        {
            Object.Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


    IEnumerator DealDamage()
    {
        animator.SetBool("isPunch", true);
        yield return new WaitForSeconds(0.5f);
        target.GetComponent<MonumentScript>().health -= damage;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isPunch", false);
        
    }    
    
    public void SetupEnemy(float speed, float health, float damage, float attackTime, float attackRadius, GameObject target, Vector3 targetLocation, int type)
    {
        this.speed = speed;
        this.health = health;
        this.damage = damage;
        this.attackTime = attackTime;
        this.attackRadius = attackRadius;
        this.target = target;
        this.targetLocation = targetLocation;
        this.type = type;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targetLocation);
        agent.speed = speed;
        agent.acceleration = speed * 2f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Debug.Log("hit hit hit");
            health -= collision.gameObject.GetComponent<Object>().stats.damage;
        }
    }
}
