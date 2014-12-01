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
        transform.localPosition = cell.transform.localPosition;
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
    }
}
