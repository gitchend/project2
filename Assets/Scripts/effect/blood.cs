using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blood : effect_script
{
    void Start ()
    {
        GameObject pixel_res = Resources.Load ("pixel") as GameObject;

        for (int i = 0; i <= 3; i++)
        {
            if(transform.parent != null && transform.parent.gameObject != null)
            {
                GameObject pixel = Instantiate (pixel_res) as GameObject;
                particle particle = pixel.GetComponent<particle> ();

                int size = (int)(Random.value * 2) + 1;
                pixel.transform.localScale = new Vector2(size, size);

                int life_time = (int) (100 * Random.value + 100);
                int wait_time = (int) (3 * Random.value);

                particle.init (life_time, wait_time, new Color(1, 1, 1));
                particle.set_speed (new Vector2 (Random.value * 1 * transform.localScale.x, Random.value * 3));
                particle.set_target (transform.parent.gameObject);
                particle.set_is_tigger(true);
            }
        }
        Destroy (transform.gameObject);
    }
}