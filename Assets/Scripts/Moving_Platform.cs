using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    public float speed;
    public int startPos;
    public Transform[] points;

    private int i; 

   
    void Start()
    {
        transform.position = points[startPos].position;  
    }

    
    void Update()
    {
       if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
       {
           i++;
           if (i == points.Length)
           {
              i = 0;
           }
       }
       transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.position.y < collision.transform.position.y - 1f)
            collision.transform.SetParent(transform);
        else if (collision.gameObject.GetComponent<PlayerController>().isMoving)
        {
            Debug.Log("Has no parent");
            collision.transform.SetParent(null);
        }
            //collision.transform.SetParent(null);   
    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
