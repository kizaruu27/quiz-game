using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] private QuestionScriptableObject question;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject[] answerButtons;
    
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite correctSprite;
    
    void Start()
    {
        // DisplayQuestion();
        GetNextQuestion();
    }

    public void OnAnswerSelected(int index)
    {
        Image buttonImage = answerButtons[index].GetComponent<Image>();
        
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctSprite;
        }
        else
        {
            questionText.text = "Sorry, the correct answer is: \n" + question.GetAnswer(question.GetCorrectAnswerIndex());
            buttonImage = answerButtons[question.GetCorrectAnswerIndex()].GetComponent<Image>();
            buttonImage.sprite = correctSprite;
        }
        
        SetButtonState(false);
    }

    void DisplayQuestion()
    {
        //set question
        questionText.text = question.GetQuestion();
        
        // set button
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttons = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttons.text = question.GetAnswer(i);
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
        SetButtonState(true);
        DisplayQuestion();
        SetDefaultButtonSprites();
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
