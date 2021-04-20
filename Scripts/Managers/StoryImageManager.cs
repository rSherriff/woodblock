using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryImageManager : MonoBehaviour
{
    public Texture GetImage(string image)
    {
        GameManager manager = FindObjectOfType<GameManager>();
        return Resources.Load<Texture>("Images/" + image);
    }
}
