using System.Collections;
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
            "Who was the first Filipino hero to resist Spanish colonization and fought against the Spaniards at the Battle of Mactan?",
            new string[] { "Jose Rizal", "Andres Bonifacio", "Lapu-Lapu", "Emilio Aguinaldo" },
            2
        ));

        questions.Add(new Question(
            "What is the Babaylan in Filipino culture?",
            new string[] { "A warrior leader", "A healer or spiritual guide", "A military commander", "A local chief" },
            1
        ));

        questions.Add(new Question(
            "Which ancient Filipino practice involves the use of herbal medicine and rituals for healing?",
            new string[] { "Albularyo", "Hilot", "Babaylan", "Katipunan" },
            0
        ));

        questions.Add(new Question(
            "The Sakim in the game is a corrupted force that distorts Filipino traditions. What historical event led to the distortion of Filipino culture and traditions?",
            new string[] { "The arrival of the Spanish colonizers", "The Japanese occupation during World War II", "The American colonial period", "The arrival of the Muslims in the Philippines" },
            0
        ));

        questions.Add(new Question(
            "In Filipino folklore, who is the Diwata?",
            new string[] { "A benevolent god", "A spirit of nature or guardian deity", "A hero who fought against invaders", "A local chieftain" },
            1
        ));

        questions.Add(new Question(
            "What is Lagundi commonly used for in Filipino traditional medicine?",
            new string[] { "To treat headaches", "To treat coughs, colds, and asthma", "To treat stomach ulcers", "To treat skin infections" },
            1
        ));

        questions.Add(new Question(
            "Which part of the Lagundi plant is used to make herbal tea?",
            new string[] { "Roots", "Leaves", "Flowers", "Stem" },
            1
        ));

        questions.Add(new Question(
            "What is Sambong used to treat in Filipino herbal medicine?",
            new string[] { "Kidney problems and urinary infections", "Coughs and colds", "Diabetes", "High blood pressure" },
            0
        ));

        questions.Add(new Question(
            "How do people typically use Sambong in traditional medicine?",
            new string[] { "By chewing the leaves", "By boiling the leaves to make tea", "By making an ointment", "By using it in cooking" },
            1
        ));

        questions.Add(new Question(
            "What is Niyog-niyogan commonly used for in Filipino medicine?",
            new string[] { "To treat headaches", "To treat digestive issues", "To get rid of intestinal worms", "To treat skin rashes" },
            2
        ));

        questions.Add(new Question(
            "What is the common method for using Niyog-niyogan to treat intestinal worms?",
            new string[] { "Boiling the leaves to make tea", "Chewing seeds", "Making an ointment", "Using the flowers in a bath" },
            1
        ));

        questions.Add(new Question(
            "What creature in Filipino folklore is said to walk backwards and suck the blood of its victims?",
            new string[] { "Aswang", "Sigbin", "Tikbalang", "Diwata" },
            1
        ));

        questions.Add(new Question(
            "What is a Tikbalang described as in Filipino mythology?",
            new string[] { "A human with wings", "A horse with a human body", "A snake with the head of a woman", "A giant eagle" },
            1
        ));

        questions.Add(new Question(
            "How is a Tikbalang said to confuse travelers?",
            new string[] { "By making loud noises", "By turning invisible", "By leading them in circles", "By changing the weather" },
            2
        ));

        questions.Add(new Question(
            "What does the Diwata represent in Filipino mythology?",
            new string[] { "A protector of communities", "A spirit of nature or goddess", "A warrior spirit", "A trickster god" },
            1
        ));

        questions.Add(new Question(
            "What happens if you harm the land protected by a Diwata?",
            new string[] { "The Diwata becomes a friend", "The Diwata can cause misfortune", "The Diwata will bless the community", "Nothing happens" },
            1
        ));

        questions.Add(new Question(
            "What is the purpose of the ceremonial drum in the ritual?",
            new string[] { "To summon rain", "To unify the community in times of hardship", "To call upon the Diwata", "To signal the beginning of a battle" },
            1
        ));

        questions.Add(new Question(
            "Which fish holds cultural significance as a symbol of Filipino agriculture and cuisine?",
            new string[] { "Bangus", "Apahap", "Bisugo", "Tilapia" },
            0
        ));

        questions.Add(new Question(
            "Which fish is often used in Filipino cooking for its tender meat and distinct flavor?",
            new string[] { "Bangus", "Apahap", "Bisugo", "Lapu-Lapu" },
            2
        ));

        questions.Add(new Question(
            "What is the common name for the fish called 'Apahap' in the Philippines?",
            new string[] { "Snapper", "Sea Bass", "Mackerel", "Barracuda" },
            1
        ));

        questions.Add(new Question(
            "Which fish is known for its round shape and mild flavor, commonly found in Filipino coastal areas?",
            new string[] { "Bangus", "Apahap", "Bisugo", "Tuna" },
            2
        ));

        questions.Add(new Question(
            "Which of the following herbs is used for making herbal tea for treating coughs in Filipino medicine?",
            new string[] { "Lagundi", "Sambong", "Niyog-niyogan", "Basil" },
            0
        ));

        questions.Add(new Question(
            "What mythological creature in Filipino folklore is believed to be controlled by witches?",
            new string[] { "Sigbin", "Tikbalang", "Diwata", "Aswang" },
            0
        ));

        questions.Add(new Question(
            "Which mythical creature is often described as playing tricks on travelers by confusing their sense of direction?",
            new string[] { "Sigbin", "Tikbalang", "Diwata", "Babaylan" },
            1
        ));


        ShuffleQuestions();

        if (questions.Count > 10)
        {
            questions = questions.GetRange(0, 10);
        }
    }

    private void ShuffleQuestions()
    {
        for (int i = 0; i < questions.Count; i++)
        {
            int randomIndex = Random.Range(0, questions.Count);
            Question temp = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
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

        Button wrongButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        StartCoroutine(ShowWrongAnswerFeedback(wrongButton));
    }

    private IEnumerator ShowWrongAnswerFeedback(Button wrongButton)
    {
        Color originalColor = wrongButton.image.color;
        wrongButton.image.color = Color.red;

        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }

        yield return new WaitForSeconds(1f);

        wrongButton.image.color = originalColor;

        foreach (var button in answerButtons)
        {
            button.interactable = true;
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