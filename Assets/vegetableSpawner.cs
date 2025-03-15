using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class vegetableSpawner : MonoBehaviour
{
    public GameObject[] vegetables;
    public GameObject chosenVegetable;

    public TextMeshProUGUI friendText;

    public BoxCollider2D spawnArea;

    public int columns = 12;
    public int rows = 2;

    private float xMin, xMax, yMin, yMax;

    private List<GameObject> spawnedVegetables = new List<GameObject>();
    public int spawnedAmount;

    public void SpawnVegetables()
    {
        StartCoroutine(SpawnVeggiesRoutine());
    }

    private IEnumerator SpawnVeggiesRoutine()
    {
        Bounds b = spawnArea.bounds;
        xMin = b.min.x;
        xMax = b.max.x;
        yMin = b.min.y;
        yMax = b.max.y;

        float cellWidth = (xMax - xMin) / columns;
        float cellHeight = (yMax - yMin) / rows;

        List<Vector2> allPositions = new List<Vector2>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (col >= 5 && col <= 6)
                {
                    continue;
                }

                float xCenter = xMin + (col + 0.5f) * cellWidth;
                float yCenter = yMin + (row + 0.5f) * cellHeight;

                allPositions.Add(new Vector2(xCenter, yCenter));
            }
        }

        int toSpawn = Random.Range(10, 21);

        chosenVegetable = vegetables[Random.Range(0, vegetables.Length)];
        UpdateFriendText(chosenVegetable);

        spawnedAmount = 0;
        for (int i = 0; i < toSpawn; i++)
        {
            Vector2 spawnPos = allPositions[i];

            // Instantiate at 0 scale
            GameObject newVeg = Instantiate(chosenVegetable, spawnPos, Quaternion.identity);
            newVeg.transform.localScale = Vector3.zero;

            spawnedVegetables.Add(newVeg);
            spawnedAmount++;

            // Animate
            StartCoroutine(AnimateZoomIn(newVeg.transform));

            yield return new WaitForSeconds(0.15f);
        }

        AnswerSpawner answerSpawner = FindFirstObjectByType<AnswerSpawner>();
        if (answerSpawner != null)
        {
            answerSpawner.ReceiveSpawnCount(spawnedAmount);
        }
    }

    private IEnumerator AnimateZoomIn(Transform target)
    {
        float durationUp = 0.2f;
        float time = 0f;
        while (time < durationUp)
        {
            time += Time.deltaTime;
            float t = time / durationUp;
            float scale = Mathf.Lerp(0f, 1.2f, t);
            target.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        float durationDown = 0.1f;
        time = 0f;
        while (time < durationDown)
        {
            time += Time.deltaTime;
            float t = time / durationDown;
            float scale = Mathf.Lerp(1.2f, 0.9f, t);
            target.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

    private void UpdateFriendText(GameObject chosenVeg)
    {
        string vegName = chosenVeg.name.Replace("(Clone)", "").Trim();

        string message = "";
        switch (vegName)
        {
            case "Carrot":
                message = "Хэдэн лууван байна тоолоорой!";
                break;
            case "Potato":
                message = "Хэдэн төмс байна тоолоорой!";
                break;
            case "Paprika":
                message = "Хэдэн чинжүү байна тоолоорой!";
                break;
            case "Cabbage":
                message = "Хэдэн байцаа байна тоолоорой!";
                break;
            case "Pumpkin":
                message = "Хэдэн хулуу байна тоолоорой!";
                break;
        }

        if (friendText != null)
        {
            friendText.text = message;
        }
    }

    public void ClearSpawnedVegetables()
    {
        foreach (GameObject veg in spawnedVegetables)
        {
            if (veg != null) Destroy(veg);
        }
        spawnedVegetables.Clear();
        spawnedAmount = 0;
    }
}
