using System.Collections;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    public float popUpHeight = 1.5f;
    public float speed = 5f;
    public float popUpTime = 1.5f;

    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;
    private bool moleVisible = false;
    private bool hasBeenHit = false;

    void Start()
    {
        hiddenPosition = transform.position; // Mole starts hidden
        visiblePosition = transform.position + Vector3.up * popUpHeight;
        transform.position = hiddenPosition; // Ensure mole starts hidden
    }
    

    public void StartMoleRoutine()
    {
        if (!moleVisible) StartCoroutine(MoleRoutine());
    }

    IEnumerator MoleRoutine()
    {
        yield return MoveMole(visiblePosition);
        moleVisible = true;
        hasBeenHit = false;
        yield return new WaitForSeconds(popUpTime);
        yield return MoveMole(hiddenPosition);
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
        return moleVisible && transform.position == visiblePosition;
    }

    public void RegisterHit()
    {
        hasBeenHit = true;
    }

    public bool HasBeenHit()
    {
        return hasBeenHit;
    }

    public void ForceMoleDown()
    {
        if (moleVisible)
        {
            StartCoroutine(MoveMole(hiddenPosition));
            moleVisible = false;
        }
    }
}

