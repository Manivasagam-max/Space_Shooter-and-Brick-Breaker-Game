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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // Space or Left Click
        {
            if (MoleController.Instance.IsMoleAtHole(holes[selectedHoleIndex]))
            {
                if (!MoleController.Instance.HasBeenHit()) // Ensures mole hasn't been hit before
                {
                    score++;
                    Debug.Log("Hit! Score: " + score);
                    ScoreText.SetText(score.ToString());

                    MoleController.Instance.RegisterHit(); // Marks mole as hit
                    MoleController.Instance.ForceMoleDown(); // Make mole disappear and reappear elsewhere
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
    //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // mousePosition.z = 0; // Ensure it's on the correct plane
    Vector3 position=mouse_pointer.position;

    Transform nearestHole = FindNearestHole(position);
    if (nearestHole != null)
    {
        selectedHoleIndex = System.Array.IndexOf(holes, nearestHole);
        UpdateSelectionIndicator();
    }
    }

    Transform FindNearestHole(Vector3 position)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform hole in holes)
        {
            float distance = Vector3.Distance(hole.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = hole;
            }
        }
        return nearest;
    }

    void UpdateSelectionIndicator()
    {
        if (selectionIndicator != null)
        {
            selectionIndicator.transform.position = holes[selectedHoleIndex].position;
        }
    }
}
