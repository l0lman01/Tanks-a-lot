using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static int score;
    Text text1;

    private void Awake()
    {
        text1 = GetComponent<Text>();
        score = 0;
    }
    private void Update()
    {
        text1.text = Mathf.Abs(score/10) + " %";
    }
}
