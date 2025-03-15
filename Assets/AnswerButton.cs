using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public int currentValue;         
    public int correctAnswer; 
    public TextMeshProUGUI buttonLabel;
    public GameManager gameManager;

    public Image buttonBackground;

    public ParticleSystem congratulationsEffect;

    public void SetValues(int value, int correct)
    {
        currentValue = value;
        correctAnswer = correct;

        if (buttonLabel != null)
            buttonLabel.text = value.ToString();
    }

    public void OnButtonClicked()
    {
        if (currentValue == correctAnswer)
        {

            gameManager.EndTurn(true, currentValue);
        }
        else
        {
            gameManager.EndTurn(false, currentValue);
        }
    }
}
