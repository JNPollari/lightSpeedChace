using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulateScripr : MonoBehaviour
{
    public GameObject particle;

    private SpriteRenderer sr;
    private Transform trf;
    private float width = 25;
    private float alpha = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        trf = GetComponent<Transform>();
        InvokeRepeating("Incoming", 0.05f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Draws alarm area for incoming particle, shoots particle upon alarm animation is finished, and destroys this' gameobject
    /// </summary>
    private void Incoming()
    {
        trf.localScale = new Vector3(300, width);
        sr.color = new Color(1, 0.7f, 1, alpha);
        width = width - 1;
        alpha = alpha + 0.011f;
        if (width < 0)
        {
            Instantiate(particle, new Vector2(trf.position.x + 100, trf.position.y), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
