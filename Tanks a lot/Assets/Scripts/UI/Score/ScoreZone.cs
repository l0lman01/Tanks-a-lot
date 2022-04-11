using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreZone : MonoBehaviour
{
    public int score;
    public Slider slider;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            if (ScoreManager.score <= 1000)
                ScoreManager.score += score;
        }
        if (collision.CompareTag("Ennemy"))
        {
            if (ScoreManager.score >= -1000)
                ScoreManager.score -= score;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ennemy")
        {
            if (ScoreManager.score >= -1000)
                ScoreManager.score -= score;
        }
        else if (collision.tag == "Tank")
        {
            if (ScoreManager.score <= 1000)
                ScoreManager.score += score;
        }
    }

    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        slider.minValue = -1000;
        slider.maxValue = 1000;
        slider.value = ScoreManager.score;
    }
}
