using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MoleController : MonoBehaviour
{
    public Transform[] holes;
    public float popUpHeight = 1.5f;
    public float popDownDistance = 1.5f;
    public float speed = 5f;
    public float minWait = 1f;
    public float maxWait = 3f;
    public float popUpTime = 1.5f;
    public float cursorDetectionRadius = 1.5f; // Radius within which the mole detects the cursor
    public float escapeChance = 0.5f; // 50% chance to escape when cursor is detected

    private Transform currentHole;
    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;
    private bool moleVisible = false;
    private bool hasBeenHit = false; // Prevent multiple scoring on a single mole

    public static MoleController Instance;
    private GameManagerW gm;
    private bool panel = true;

    void Awake()
    {
        gm = FindObjectOfType<GameManagerW>();
        Instance = this;
    }

    void Start()
    {
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
        int randomIndex = Random.Range(0, holes.Length);
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
        return moleVisible && currentHole == hole;
    }

    public void ForceMoleDown()
    {
        StopAllCoroutines();
        StartCoroutine(ForceMoveDown());
    }

    IEnumerator ForceMoveDown()
    {
        yield return MoveMole(hiddenPosition);
        moleVisible = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoleRoutine()); // Restart cycle
    }

    void DetectCursorProximity()
    {
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0; // Ensure it's in the correct plane

        float distance = Vector3.Distance(transform.position, cursorWorldPosition);
        
        if (distance < cursorDetectionRadius && Random.value < escapeChance) // 50% chance to escape
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
