using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timerValue;
    [SerializeField] private float timeToComplete = 30f;
    [SerializeField] private float timeToShowAnswer = 10f;

    public bool isAnsweringQuestion = false;
    public bool loadNextQuestion;
    
    private float fillFraction;
    
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToComplete;
            }
            else
            {
                timerValue = timeToShowAnswer;
                isAnsweringQuestion = false;
            }
        }
        else
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToShowAnswer;
            }
            else
            {
                timerValue = timeToComplete;
                isAnsweringQuestion = true;
                loadNextQuestion = true;
            }
        }
        
        Debug.Log(isAnsweringQuestion + " : " + timerValue + " : " + fillFraction);
    }

    void CancelTimer()
    {
        timerValue = 0;
    }
}
