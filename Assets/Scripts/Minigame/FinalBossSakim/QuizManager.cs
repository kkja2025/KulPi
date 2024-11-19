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
    [SerializeField] private Transform spiritContainer; // Parent Transform for Spirit objects
    [SerializeField] private GameObject spiritPrefab; // Prefab for Spirit object

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

        InitializeQuestions(); // Load hardcoded questions
        InitializeSpirits();   // Setup spirit objects
        LoadQuestion();
    }

    private void InitializeQuestions()
    {
        questions.Add(new Question(
            "What is the capital of the Philippines?",
            new string[] { "Manila", "Cebu", "Davao", "Baguio" },
            0
        ));

        questions.Add(new Question(
            "What year did the Philippines gain independence from the United States?",
            new string[] { "1946", "1947", "1945", "1950" },
            0
        ));

        questions.Add(new Question(
            "Which Philippine island is the largest in terms of land area?",
            new string[] { "Luzon", "Mindanao", "Palawan", "Visayas" },
            0
        ));

        questions.Add(new Question(
            "What is the national bird of the Philippines?",
            new string[] { "Eagle", "Philippine Eagle", "Parrot", "Maya" },
            1
        ));

        questions.Add(new Question(
            "What is the national language of the Philippines?",
            new string[] { "English", "Filipino", "Tagalog", "Cebuano" },
            1
        ));

        questions.Add(new Question(
            "What is the largest ethnic group in the Philippines?",
            new string[] { "Ilocano", "Visayan", "Tagalog", "Kapampangan" },
            2
        ));

        questions.Add(new Question(
            "What is the smallest province in the Philippines by land area?",
            new string[] { "Batanes", "Tawi-Tawi", "Siquijor", "Guimaras" },
            0
        ));

        questions.Add(new Question(
            "Which Philippine festival is known for its colorful masks?",
            new string[] { "Sinulog", "Ati-Atihan", "MassKara", "Kadayawan" },
            2
        ));

        questions.Add(new Question(
            "What is the longest river in the Philippines?",
            new string[] { "Agusan River", "Cagayan River", "Pasig River", "Pampanga River" },
            1
        ));

        questions.Add(new Question(
            "Who is known as the National Hero of the Philippines?",
            new string[] { "Jose Rizal", "Andres Bonifacio", "Emilio Aguinaldo", "Antonio Luna" },
            0
        ));
    }

    private void InitializeSpirits()
    {
        foreach (Transform child in spiritContainer)
        {
            Destroy(child.gameObject); // Destroys each spirit object
        }

        // Clear the spirits list to ensure it starts empty
        spirits.Clear();
        // Create Spirit objects equal to the number of questions
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

        // Shuffle answers and assign to buttons...
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
        RemoveSpirit(); // Remove a spirit when a question is answered correctly
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

        RemoveSpirit(); // Remove a spirit for a wrong answer
        currentQuestionIndex++;
        LoadQuestion();
    }

    private void RemoveSpirit()
    {
        if (spirits.Count > 0)
        {
            Destroy(spirits[0]); // Destroy the first Spirit object
            spirits.RemoveAt(0); // Remove it from the list
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over! You lost all your health.");
        // Handle Game Over logic here
    }

    private void EndQuiz()
    {
        Debug.Log("Quiz Complete!");
        // Handle end-of-quiz logic
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