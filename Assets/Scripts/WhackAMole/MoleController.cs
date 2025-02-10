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

    private Transform currentHole;
    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;
    private bool moleVisible = false;
    private bool isFullyUp = false; // NEW: Tracks if mole is fully popped up
    private bool hasBeenHit = false; // NEW: Tracks if mole has already been hit

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
    }

    IEnumerator MoleRoutine()
    {
        while (panel)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            PickNewHole();
            hasBeenHit = false; // Reset hit status for new mole pop-up

            yield return MoveMole(visiblePosition);
            moleVisible = true;
            isFullyUp = true; // NEW: Mark as fully popped up

            yield return new WaitForSeconds(popUpTime);

            yield return MoveMole(hiddenPosition);
            moleVisible = false;
            isFullyUp = false; // NEW: Mark as no longer popped up
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
        isFullyUp = false;
    }

    IEnumerator MoveMole(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;

        if (target == visiblePosition)
        {
            isFullyUp = true; // Ensure mole is marked as fully up
        }
    }

    public bool IsMoleAtHole(Transform hole)
    {
        return isFullyUp && currentHole == hole;
    }

    public bool HasBeenHit()
    {
        return hasBeenHit;
    }

    public void RegisterHit()
    {
        hasBeenHit = true;
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
        isFullyUp = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoleRoutine()); // Restart cycle
    }
}
