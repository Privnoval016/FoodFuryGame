using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject[] spawnPoints = new GameObject[2];
    public GameObject enemyPrefab;
    public GameObject[] monuments = new GameObject[3];

    public float spawnTimer = 0;
    public float spawnRate = 1.0f;
    public float waveLength = 0;
    public float currentCount = 0;
    public float waveTimer = 0;
    public float waveRate = 5.0f;
    public int waveNumber = 0;

    public float agentSpeed = 0;
    public float health = 2;
    public float damage = 1;
    public float attackSpeed = 1;
    public float attackRadius = 3;
    public int enemyType;

    public TMP_Text waveText;
    public GameObject waveUI;

    public GameObject endText;


    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {

        waveLength = 0;
        spawnTimer = 0;
        waveTimer = waveRate;
        currentCount = 0;
        waveNumber = 0;

        endText.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentCount >= waveLength)
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= waveRate)
            {
                waveNumber++;
                if (waveNumber % 3 == 1)
                   StartWave1();
                else if (waveNumber % 3 == 2)
                    StartWave2();
                else if (waveNumber % 3 == 0)
                    StartWave3();

                currentCount = 0;
                waveTimer = 0;
            }

            return;
        }
        if (waveNumber > 0)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                currentCount++;
                spawnTimer = 0;

                GameObject enemy = Object.Instantiate(enemyPrefab, spawnPoints[Random.Range(0, 2)].transform.position, Quaternion.identity);
                CustomerScript enemyScript = enemy.GetComponent<CustomerScript>();

                GameObject currentMonument = monuments[Random.Range(0, 3)];
                Quaternion randomRot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
                Vector3 attackPosition = currentMonument.transform.position + 3 * (randomRot * Vector3.forward);
                //Vector3 attackPosition = currentMonument.transform.position + new Vector3(Random.value < 0.5f ? 3f : -3f, 0, Random.value < 0.5f ? 3f : -3f);

                enemy.GetComponent<CustomerScript>().SetupEnemy(agentSpeed, health, damage, attackSpeed, attackRadius, currentMonument, attackPosition, enemyType);


                
            }
        }

        foreach (GameObject monument in monuments)
        {
            if (monument.GetComponent<MonumentScript>().health <= 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;

                endText.SetActive(true);
            }
        }    
    }

    void StartWave1()
    {
        Debug.Log("Starting wave " + waveNumber);
        waveText.text = waveNumber.ToString();
        waveUI.GetComponent<RawImage>().color = new Color(0.961f, 0.314f, 0, 1);
        waveLength = 13;
        spawnRate = 6.0f;

        enemyType = 1;
        health = 2;
        damage = 2;
        attackSpeed = 2;
        attackRadius = 0.5f;
        agentSpeed = 2f;
    }

    void StartWave2()
    {
        Debug.Log("Starting wave " + waveNumber);
        waveText.text = waveNumber.ToString();
        waveUI.GetComponent<RawImage>().color = new Color(0.165f, 0.404f, 0.737f, 1);
        waveLength = 12;
        spawnRate = 4.0f;

        enemyType = 2;
        health = 1;
        damage = 1;
        attackSpeed = 1;
        attackRadius = 0.5f;
        agentSpeed = 5f;
    }

    void StartWave3()
    {
        Debug.Log("Starting wave " + waveNumber);
        waveText.text = waveNumber.ToString();
        waveUI.GetComponent<RawImage>().color = new Color(0.804f, 0.580f, 0.192f, 1);
        waveLength = 8;
        spawnRate = 8.0f;

        enemyType = 1;
        health = 8;
        damage = 4;
        attackSpeed = 5;
        attackRadius = 0.5f;
        agentSpeed = 2f;

    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

}
