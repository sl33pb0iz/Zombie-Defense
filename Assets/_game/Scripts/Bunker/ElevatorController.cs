using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public bool canMove;
    public float speed;
    public Transform[] points;
    public int startPoint; 


    private int index;
    bool reverse;

    private void Start()
    {
        transform.position = points[startPoint].position;
        index = startPoint;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, points[index].position)<= 0.01f)
        {
            canMove = false;
            if(index == points.Length - 1)
            {
                reverse = true;
                index--;
                return;
            }
            else if(index == 0)
            {
                reverse = true;
                index++;
                return; 
            }
            if (reverse)
            {
                index--;
            }
            else index++;
        }
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[index].position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canMove = true; 
        }
    }
}
