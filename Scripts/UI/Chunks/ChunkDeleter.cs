using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDeleter : MonoBehaviour
{
    void Update()
    {
        if(transform.childCount > 15)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
