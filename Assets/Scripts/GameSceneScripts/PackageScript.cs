using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageScript : MonoBehaviour
{

    public Rigidbody2D packageRB;
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

        if (packageRB.velocity == Vector2.zero)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z);
        }
        if (transform.localScale.x < 0)
        {
            Destroy(gameObject);
        }
    }
}
