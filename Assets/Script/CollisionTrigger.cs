using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    private BoxCollider2D playerCollider;

    [SerializeField]
    private BoxCollider2D platFromCollider;

    [SerializeField]
    private BoxCollider2D platFromTriger;
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.Find("Ninja").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platFromCollider, platFromTriger, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="Ninja")
        {
            Physics2D.IgnoreCollision(platFromCollider, playerCollider, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ninja")
        {
            Physics2D.IgnoreCollision(platFromCollider, playerCollider, false);
        }
    }
 
}
