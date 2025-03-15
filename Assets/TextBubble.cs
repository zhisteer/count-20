using TMPro;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    public TextMeshProUGUI friendText;
    public GameObject gameManager;
    private GameManager gameManagerScript;
    public GameObject vegetableSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
