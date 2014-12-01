using UnityEngine;
using System.Collections;

public class End : MonoBehaviour
{
    private MazeCell currentCell;

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
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
    }
}
