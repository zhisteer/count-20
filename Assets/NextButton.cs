using UnityEngine;

public class NextButton : MonoBehaviour
{

    public GameObject gameManager;
    public GameManager gameManagerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.currentState == GameManager.GameState.Playing)
        {

        }
    }
}
