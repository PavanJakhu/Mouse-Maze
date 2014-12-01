using UnityEngine;
using System.Collections;

public class Cheese : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAroundLocal(new Vector3(0.0f, 1.0f, 0.0f), 1 * Time.deltaTime);
    }

    public void SetLocation(MazeCell cell)
    {
        transform.localPosition = cell.transform.localPosition;
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
    }
}
