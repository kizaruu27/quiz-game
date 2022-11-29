using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    QuestionScriptableObject currentQuestion;
    [SerializeField] private List<QuestionScriptableObject> question = new List<QuestionScriptableObject>();
    [SerializeField] private TextMeshProUGUI questionText;
    
    [Header("Answers")]
    [SerializeField] private GameObject[] answerButtons;
    private bool isAnswerEarly = true;
    
    [Header("Button Colors")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite correctSprite;

    [Header("Timer")]
    [SerializeField] private Image timerImage;
    Timer _timer;

    [Header("Scoring")] 
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreKeeper _scoreKeeper;

    [Header("Progress Bar")] 
    [SerializeField] private Slider progressBar;
    public bool isCompleted;

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    void Start()
    {
        progressBar.maxValue = question.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        timerImage.fillAmount = _timer.fillFraction;

        if (_timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isCompleted = true;
                return;
            }
            
            isAnswerEarly = false;
            GetNextQuestion();
            _timer.loadNextQuestion = false;
        }
        else if (!isAnswerEarly && !_timer.isAnsweringQuestion)
        {
            int index = currentQuestion.GetCorrectAnswerIndex() + 1;
            if (index >= answerButtons.Length)
            {
                index = 0;
            }
            
            DisplayAnswers(index);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        isAnswerEarly = true;
        DisplayAnswers(index);
        SetButtonState(false);
        _timer.CancelTimer();
        scoreText.text = "Score: " + _scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswers(int index)
    {
        Image buttonImage = answerButtons[index].GetComponent<Image>();
        
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctSprite;
            _scoreKeeper.IncrementCorrectAnswer();
        }
        else
        {
            questionText.text = "Sorry, the correct answer is: \n" + currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswerIndex());
            buttonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
            buttonImage.sprite = correctSprite;
        }

    }

    void DisplayQuestion()
    {
        //set question
        questionText.text = currentQuestion.GetQuestion();
        
        // set button
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttons = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttons.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Button>().interactable = state;
        }
    }

    void GetNextQuestion()
    {
        if (question.Count > 0)
        {
            GetRandomQuestion();
            SetButtonState(true);
            DisplayQuestion();
            SetDefaultButtonSprites();
            _scoreKeeper.IncrementQuestionSeen();

            progressBar.value++;
        }
    }

    private void GetRandomQuestion()
    {
        int index = Random.Range(0, question.Count);
        currentQuestion = question[index];

        if (question.Contains((currentQuestion)))
        {
             question.Remove(currentQuestion);
        }
    }

    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image imageButton = imageButton = answerButtons[i].GetComponent<Image>();
            imageButton.sprite = defaultSprite;
        }
    }
}
