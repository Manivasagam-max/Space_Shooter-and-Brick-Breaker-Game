using System.IO;
using TMPro;
using UnityEngine;

public class HoleSelector : MonoBehaviour
{
    public Transform[] holes; // List of all holes
    public MoleController[] moles; // Corresponding mole controllers
    public GameObject selectionIndicator; // Visual indicator for the selected hole
    public TextMeshProUGUI scoreText; // UI Score text
    public Transform mouse_pointer; // Mouse pointer object in the game

    private int selectedHoleIndex = -1; // Index of the currently selected hole
    private int lastScoredHoleIndex = -1; // Track the last hole where the player scored
    public int score;
    private GameManagerW gm;
    public float selectionRadius = 1.0f; // Radius within which a hole gets selected

    private string filepath = Path.Combine(Application.dataPath, "Patient_Data", "Whack_Score.csv");
    private int Level;

    void Start()
    {
        gm = FindObjectOfType<GameManagerW>();
        if (File.Exists(filepath))
        {
            string[] lines = File.ReadAllLines(filepath);
            string[] values = lines[0].Split(',');
            if (values.Length >= 2)
            {
                int.TryParse(values[0], out score);
                int.TryParse(values[1], out Level);
            }
        }

        scoreText.text = score.ToString();
        if (File.Exists(filepath))
        {
            string data = $"{score},{Level}";
            File.WriteAllText(filepath, data);
        }

        if (holes.Length != moles.Length)
        {
            Debug.LogError("Mismatch: Number of holes and moles must be the same!");
        }
    }

    void Update()
    {
        if (gm != null && gm.isgameover) return;

        HandleMouseSelection();
    }

    void HandleMouseSelection()
    {
        Vector3 position = mouse_pointer.position;
        Transform closestHole = null;
        float closestDistance = selectionRadius;

        for (int i = 0; i < holes.Length; i++)
        {
            float distance = Vector3.Distance(position, holes[i].position);
            if (distance < closestDistance)
            {
                closestHole = holes[i];
                selectedHoleIndex = i;
                closestDistance = distance;
            }
        }

        if (closestHole != null)
        {
            // Move selection indicator to selected hole
            selectionIndicator.SetActive(true);
            selectionIndicator.transform.position = closestHole.position;

            // Check and score
            CheckAndScore();
        }
        else
        {
            // No hole is close, hide the selection indicator
            selectionIndicator.SetActive(false);
            selectedHoleIndex = -1;
        }
    }

    void CheckAndScore()
    {
        if (selectedHoleIndex == -1) return;

        MoleController selectedMole = moles[selectedHoleIndex];

        if (selectedMole != null && selectedMole.IsMoleAtHole(holes[selectedHoleIndex]))
        {
            if (!selectedMole.HasBeenHit())
            {
                // **Prevent scoring if the player hasn't moved to another hole**
                if (selectedHoleIndex != lastScoredHoleIndex)
                {
                    score++;
                    Debug.Log("Hit! Score: " + score);
                    scoreText.SetText(score.ToString());

                    selectedMole.RegisterHit();
                    selectedMole.ForceMoleDown();

                    lastScoredHoleIndex = selectedHoleIndex; // Update last scored hole
                }
                else
                {
                    Debug.Log("Player did not move to a new hole, ignoring score.");
                }
            }
            else
            {
                Debug.Log("Mole already hit, ignoring extra hit.");
            }
        }
    }
}
