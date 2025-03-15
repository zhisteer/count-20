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
            Debug.Log("Correct! The answer is: " + correctAnswer);

            // Make the button background green
            if (buttonBackground != null)
                buttonBackground.color = Color.green;

            // Play a congratulations effect (particle system)
            if (congratulationsEffect != null)
                congratulationsEffect.Play();

            gameManager.EndTurn(true, currentValue);
        }
        else
        {
            Debug.Log("Incorrect. You chose: " + currentValue + ", correct is: " + correctAnswer);
            // You could change color to red or handle other UI feedback here
            gameManager.EndTurn(false, currentValue);
        }
    }
}
