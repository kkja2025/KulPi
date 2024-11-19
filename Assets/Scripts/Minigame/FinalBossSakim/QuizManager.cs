using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform spiritContainer; 
    [SerializeField] private GameObject spiritPrefab;

    [Header("Quiz Settings")]
    private List<Question> questions = new List<Question>();
    private List<GameObject> spirits = new List<GameObject>();

    private int maxHealth = 4;
    private int currentQuestionIndex;
    private int playerHealth;

    private void Start()
    {
        playerHealth = maxHealth;
        currentQuestionIndex = 0;

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        InitializeQuestions();
        InitializeSpirits();   
        LoadQuestion();
    }

    private void InitializeQuestions()
    {
        questions.Add(new Question(
            "What is the capital of the Philippines?",
            new string[] { "Manila", "Cebu", "Davao", "Baguio" },
            0
        ));
    }

    private void InitializeSpirits()
    {
        foreach (Transform child in spiritContainer)
        {
            Destroy(child.gameObject); 
        }
        spirits.Clear();
        for (int i = 0; i < questions.Count; i++)
        {
            GameObject spirit = Instantiate(spiritPrefab, spiritContainer);
            spirits.Add(spirit);
        }
    }

    private void LoadQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            EndQuiz();
            return;
        }

        Question currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.QuestionText;

        List<int> shuffledIndices = new List<int>();
        for (int i = 0; i < currentQuestion.Answers.Length; i++)
        {
            shuffledIndices.Add(i);
        }
        for (int i = 0; i < shuffledIndices.Count; i++)
        {
            int randomIndex = Random.Range(0, shuffledIndices.Count);
            int temp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[randomIndex];
            shuffledIndices[randomIndex] = temp;
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answerIndex = shuffledIndices[i];
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Answers[answerIndex];
            answerButtons[i].onClick.RemoveAllListeners();

            if (answerIndex == currentQuestion.CorrectAnswerIndex)
            {
                answerButtons[i].onClick.AddListener(CorrectAnswer);
            }
            else
            {
                answerButtons[i].onClick.AddListener(WrongAnswer);
            }
        }
    }

    private void CorrectAnswer()
    {
        RemoveSpirit();
        currentQuestionIndex++;
        LoadQuestion();
    }

    private void WrongAnswer()
    {
        playerHealth--;
        healthBar.value = playerHealth;

        if (playerHealth <= 0)
        {
            GameOver();
            return;
        }

        RemoveSpirit();
        currentQuestionIndex++;
        LoadQuestion();
    }

    private void RemoveSpirit()
    {
        if (spirits.Count > 0)
        {
            Destroy(spirits[0]); 
            spirits.RemoveAt(0);
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("gameover").Open();
    }

    private void EndQuiz()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("quiz").Close();
        PanelManager.GetSingleton("combat").Open();
    }
}
[System.Serializable]
public class Question
{
    public string QuestionText { get; private set; }
    public string[] Answers { get; private set; }
    public int CorrectAnswerIndex { get; private set; }

    public Question(string question, string[] answers, int correctAnswerIndex)
    {
        QuestionText = question;
        Answers = answers;
        CorrectAnswerIndex = correctAnswerIndex;
    }
}