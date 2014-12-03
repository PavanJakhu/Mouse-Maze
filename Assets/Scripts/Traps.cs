using UnityEngine;
using System.Collections;

public class Traps : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetLocation(MazeCell cell)
    {
        transform.position = cell.transform.localPosition;
        transform.position += new Vector3(0.0f, 0.1f, 0.0f);
    }
}
