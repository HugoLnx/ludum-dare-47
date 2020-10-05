using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class expolsionscript : MonoBehaviour
{
    public float minForce;
    public float maxForce;
    public float minAngle;
    public float maxAngle;
    public float radius;
    public float gravity;

    private IEnumerator Start()
    {
        ExplodeAll();
        yield return new WaitForSeconds(0.25f);
        AudioPlayer.Sfx.Play("haha");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExplodeAll()
    {

        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                //    rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
                rb.gravityScale = gravity;
                Explode(rb, minAngle,maxAngle, minForce, maxForce);
            }
        }
    }

    public static void Explode(Rigidbody2D body, float minAngle, float maxAngle, float minForce, float maxForce)
    {
        var degrees = UnityEngine.Random.Range(minAngle, maxAngle);
        var force = UnityEngine.Random.Range(minForce, maxForce);
        var radians = degrees * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * force;
        body.AddForce(direction);
        
    }
}
