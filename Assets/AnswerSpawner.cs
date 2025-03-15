using UnityEngine;
using System.Collections.Generic;

public class AnswerSpawner : MonoBehaviour
{
    public GameObject vegetableSpawner;
  
    public int spawnedAmount;

    public Canvas canvas;

    public Transform spawnArea; 
    

    public int columns = 3;         

    public GameObject answerButtonPrefab;

    public GameManager gameManager;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
    }

    private void Update()
    {
        if (spawnedAmount > 0)
        {
            int[] answers = GenerateAnswerChoices(spawnedAmount);

            SpawnAnswerButtons(answers);

            spawnedAmount = 0;
        }
    }

    private int[] GenerateAnswerChoices(int correct)
    {
        int[] answers = new int[3];
        answers[0] = correct;

        List<int> usedValues = new List<int>();
        usedValues.Add(correct);

        for (int i = 1; i < 3; i++)
        {
            int candidateValue = correct;
            bool foundUnique = false;
            int attempts = 0;

            while (!foundUnique && attempts < 20)
            {
                int offset = Random.Range(1, 3); 
                if (Random.value < 0.5f)
                    offset = -offset;

                candidateValue = correct + offset;

                if (!usedValues.Contains(candidateValue) && candidateValue > 0)
                {
                    usedValues.Add(candidateValue);
                    foundUnique = true;
                }
                attempts++;
            }

            if (!foundUnique)
            {
                candidateValue = correct + i;
                usedValues.Add(candidateValue);
            }

            answers[i] = candidateValue;
        }

        return answers;
    }

    public void SpawnAnswerButtons(int[] answers)
    {
        ShuffleArray(answers);

        for (int i = 0; i < columns; i++)
        {

            GameObject btnObj = Instantiate(answerButtonPrefab, spawnArea);
            AnswerButton answerBtn = btnObj.GetComponent<AnswerButton>();
            answerBtn.gameManager = gameManager;

            AnswerButton btnScript = btnObj.GetComponent<AnswerButton>();
            if (btnScript != null)
            {
                btnScript.SetValues(answers[i], spawnedAmount);
            }

            spawnedButtons.Add(btnObj);
        }
    }

    public void ClearAnswers()
    {
        foreach(GameObject btn in spawnedButtons)
        {
            if (btn != null)
            {
                Destroy(btn);
            }
        }
        spawnedButtons.Clear();
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public void ReceiveSpawnCount(int count)
    {
        spawnedAmount = count;
    }
}
