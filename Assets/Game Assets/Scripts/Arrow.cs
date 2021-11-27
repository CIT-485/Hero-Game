using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    bool fadeStart = false;
    bool contact = false;
    SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        sr.enabled = true;
        if (!contact)
        {
            Vector2 v = GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (fadeStart && sr.color.a > 0)
        {
            float differenceAlpha = Mathf.Abs(sr.color.a);
            if (differenceAlpha < 0.1f)
                differenceAlpha = 0.1f;
            float alpha = sr.color.a - Time.deltaTime * differenceAlpha * 1.5f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!contact)
            if (!collision.tag.Contains("Hitbox") && !(collision.tag == "Player") && !collision.tag.Contains("Draw"))
                StartCoroutine(GetStuck(collision.transform));
            else if (collision.tag == "Player")
                Destroy(gameObject);
    }
    IEnumerator GetStuck(Transform obj)
    {
        yield return new WaitForSeconds(0.02f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
        contact = true;
        fadeStart = true;
        yield return new WaitUntil(() => sr.color.a <= 0.01f);
        Destroy(gameObject);
    }
}