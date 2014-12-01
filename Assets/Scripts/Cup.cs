using UnityEngine;
using System.Collections;

public class Cup : MonoBehaviour
{
    [SerializeField]
    Transform cup;

    [SerializeField]
    Transform startCup;

    [SerializeField]
    Transform endCup;

    [SerializeField]
    float cupSpeed;

    Vector3 direction;
    Transform destination;

    void Start()
    {
        Transform start = Instantiate(startCup) as Transform;
        start.position = transform.localPosition + new Vector3(0.0f, 1.0f, 0.0f);
        Transform end = Instantiate(endCup) as Transform;
        end.position = transform.localPosition - new Vector3(0.0f, 0.8f, 0.0f);
        startCup = GameObject.Find("Start Position(Clone)").transform;
        endCup = GameObject.Find("End Position(Clone)").transform;
        SetDestination(startCup);
    }

    void FixedUpdate()
    {
        cup.rigidbody.MovePosition(cup.position + direction * cupSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(cup.position, destination.position) < cupSpeed * Time.fixedDeltaTime)
        {
            SetDestination(destination == startCup ? endCup : startCup);
        }
    }

    void SetDestination(Transform dest)
    {
        destination = dest;
        direction = (destination.position - cup.position).normalized;
    }

    public void SetLocation(MazeCell cell)
    {
        transform.localPosition = cell.transform.localPosition;
        transform.localPosition += new Vector3(0f, 1.0f, 0f);
    }
}
