using TMPro;
using UnityEngine;

public class HoleSelector : MonoBehaviour
{
    public Transform[] holes;
    public GameObject selectionIndicator;
    private int selectedHoleIndex = 0;
    public int score = 0;
    public TextMeshProUGUI ScoreText;
    private GameManagerW gm;
    public Transform mouse_pointer;

    void Start()
    {
        gm = FindObjectOfType<GameManagerW>();
        if (holes.Length > 0)
        {
            UpdateSelectionIndicator();
        }
    }

    void Update()
    {
        if (gm != null && gm.isgameover)
        {
            return;
        }

        HandleMouseSelection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (MoleController.Instance.IsMoleAtHole(holes[selectedHoleIndex]))
            {
                if (!MoleController.Instance.HasBeenHit())
                {
                    score++;
                    Debug.Log("Hit! Score: " + score);
                    ScoreText.SetText(score.ToString());

                    MoleController.Instance.RegisterHit();
                    MoleController.Instance.ForceMoleDown();
                }
                else
                {
                    Debug.Log("Mole already hit, ignoring extra hit.");
                }
            }
            else
            {
                Debug.Log("Miss!");
            }
        }
    }

    void HandleMouseSelection()
    {
        Vector3 position = mouse_pointer.position;
        float selectionRadius = 1.0f;

        Transform closestHole = null;
        float closestDistance = selectionRadius;

        foreach (Transform hole in holes)
        {
            float distance = Vector3.Distance(position, hole.position);
            if (distance < closestDistance)
            {
                closestHole = hole;
                closestDistance = distance;
            }
        }

        if (closestHole != null && selectedHoleIndex != System.Array.IndexOf(holes, closestHole))
        {
            selectedHoleIndex = System.Array.IndexOf(holes, closestHole);
            UpdateSelectionIndicator();
        }
    }

    void UpdateSelectionIndicator()
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.transform.position = holes[selectedHoleIndex].position;
        }
    }
}
