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

    public GameObject[] skins = new GameObject[4];

    public bool atTarget;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupEnemy(speed, health, damage, attackTime, attackRadius, target, target.transform.position, type);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(targetLocation, transform.position) < attackRadius)
        { 
            agent.ResetPath();
            //transform.LookAt(target.transform.position);
            timer += Time.deltaTime;
            if (timer > attackTime)
            {
                DealDamage();
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DealDamage()
    {

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

        agent.SetDestination(targetLocation);
        agent.speed = speed;
        agent.acceleration = speed * 2f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            health -= collision.gameObject.GetComponent<Object>().stats.damage;
        }
    }
}
