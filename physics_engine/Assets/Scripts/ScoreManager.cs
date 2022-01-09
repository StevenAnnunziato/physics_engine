using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private Text scoreText;
    private int currentScore;

    public void AddScore(int v)
    {
        currentScore += v;

        // update UI
        scoreText.text = currentScore.ToString();
    }
}
