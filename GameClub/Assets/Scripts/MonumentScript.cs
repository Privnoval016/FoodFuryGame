using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonumentScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public GameObject healthBar;
    public Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(Color.red, 0.0f);
        colors[1] = new GradientColorKey(Color.yellow, 0.5f);
        colors[2] = new GradientColorKey(Color.green, 1.0f);

  

        gradient.SetKeys(colors, gradient.alphaKeys);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<Slider>().value = health / maxHealth;
        healthBar.GetComponent<Slider>().fillRect.gameObject.GetComponent<Image>().color = gradient.Evaluate(healthBar.GetComponent<Slider>().value);
    }
}
