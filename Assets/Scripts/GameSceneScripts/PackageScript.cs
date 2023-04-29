using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageScript : MonoBehaviour
{

    public Rigidbody2D packageRB;
    public BoxCollider2D packageCollider;
    private Vector2 velocity;

    public void SetStartForce(Vector2 startVelocity)
    {
        velocity = startVelocity;
        packageRB.AddForce(new Vector3(velocity.x, velocity.y, 0), ForceMode2D.Impulse);
    }

    void Update()
    {
        DeleteWhenLanded();
    }

    private void DeleteWhenLanded()
    {

        if (packageRB.velocity.magnitude <= 0.1f)
        {
            packageCollider.isTrigger = true;
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z);
        }
        if (transform.localScale.x < 0)
        {
            Destroy(gameObject);
        }
    }
}
