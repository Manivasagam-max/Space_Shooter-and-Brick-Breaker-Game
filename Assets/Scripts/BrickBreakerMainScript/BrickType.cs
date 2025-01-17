using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickType : MonoBehaviour
{
    // Start is called before the first frame update
    public int BrickId;
    void Start()
    {
                foreach (Transform child in transform)
        {
            BrickType brickType = child.gameObject.AddComponent<BrickType>();
            brickType.BrickId = BrickId;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
