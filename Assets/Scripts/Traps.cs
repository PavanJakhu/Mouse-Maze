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
        Debug.Log("Setting Location");
        transform.position = cell.transform.localPosition;
        Debug.Log(transform.position);
        transform.position += new Vector3(0.0f, 0.1f, 0.0f);
    }
}
