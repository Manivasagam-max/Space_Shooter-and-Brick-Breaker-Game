using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    public Transform[] holes;
    public float popUpHeight = 1.5f;
    public float popDownDistance = 1.5f;
    public float speed = 5f;
    public float minWait = 1f;
    public float maxWait = 3f;
    public float popUpTime = 1.5f;
    public float cursorDetectionRadius = 1.5f; // Detection radius
    [Range(0f, 1f)] 
    public float cursorDetectionChance = 0.5f; // 50% chance to detect cursor

    private Transform currentHole;
    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;
    private bool moleVisible = false;
    private bool hasBeenHit = false; // Prevent multiple scoring on a single mole
    private bool canDetectCursor = false; // Randomly decide if the mole can detect the cursor

    public static MoleController Instance;
    private GameManagerW gm;
    private bool panel = true;
    private int lastHoleIndex = -1;
    private List<int> recentHoles = new List<int>(); // Store recent holes

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gm = FindObjectOfType<GameManagerW>();
        StartCoroutine(MoleRoutine());
    }

    void Update()
    {
        if (gm != null && gm.isgameover)
        {
            panel = false;
        }

        if (moleVisible)
        {
            DetectCursorProximity();
        }
    }

    IEnumerator MoleRoutine()
    {
        while (panel)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            PickNewHole();

            // Decide if the mole will detect the cursor this time
            canDetectCursor = Random.value < cursorDetectionChance;

            yield return MoveMole(visiblePosition);
            moleVisible = true;
            hasBeenHit = false; // Reset hit detection
            yield return new WaitForSeconds(popUpTime);
            yield return MoveMole(hiddenPosition);
            moleVisible = false;
        }
    }

    void PickNewHole()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, holes.Length);
        }
        while (recentHoles.Contains(randomIndex) && holes.Length > 2); // Avoid recent holes

        lastHoleIndex = randomIndex;
        recentHoles.Add(randomIndex);

        if (recentHoles.Count > 2)
        {
            recentHoles.RemoveAt(0);
        }

        currentHole = holes[randomIndex];
        hiddenPosition = currentHole.position + Vector3.down * popDownDistance;
        visiblePosition = currentHole.position + Vector3.up * popUpHeight;
        transform.position = hiddenPosition;
        moleVisible = false;
    }

    IEnumerator MoveMole(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    public bool IsMoleAtHole(Transform hole)
    {
        return moleVisible && currentHole == hole && transform.position == visiblePosition;
    }

    public void ForceMoleDown()
    {
        StopAllCoroutines();
        StartCoroutine(EscapeToNewHole());
    }

    IEnumerator EscapeToNewHole()
    {
        // Move the mole down
        yield return MoveMole(hiddenPosition);
        moleVisible = false;

        // Pick a new hole before popping up again
        PickNewHole();
        yield return new WaitForSeconds(1f); 

        // Immediately pop up from the new hole
        yield return MoveMole(visiblePosition);
        moleVisible = true;
        hasBeenHit = false; // Reset hit detection
    }
//     IEnumerator EscapeToNewHole()
// {
//     // Instantly hide the mole (skip Lerp animation)
//     transform.position = hiddenPosition;
//     moleVisible = false;

//     // Pick a new hole before popping up again
//     PickNewHole();

//     // Instantly place the mole at the new hole's hidden position
//     transform.position = hiddenPosition;

//     // Wait a small delay to make it feel natural (optional)
//     yield return new WaitForSeconds(0.2f);

//     // Move the mole up normally
//     yield return MoveMole(visiblePosition);
//     moleVisible = true;
//     hasBeenHit = false; // Reset hit detection
// }


    void DetectCursorProximity()
    {
        if (!canDetectCursor) return; // Skip detection if not enabled for this pop-up

        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0;
        float distance = Vector3.Distance(transform.position, cursorWorldPosition);

        if (distance < cursorDetectionRadius)
        {
            Debug.Log("Cursor detected! Mole escaping...");
            ForceMoleDown();
        }
    }

    public void RegisterHit()
    {
        hasBeenHit = true;
    }

    public bool HasBeenHit()
    {
        return hasBeenHit;
    }
}
