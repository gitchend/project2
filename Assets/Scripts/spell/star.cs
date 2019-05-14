using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : spell_script
{

    private int time_now;
    private int time_max;
    private bool is_front;
    private Animator animator;
    private bool is_dead;

    private Light light;
    private float intensity;

    void OnEnable ()
    {
        if (speller != null)
        {
            time_max = 50;
            time_now = 0;
            is_front = true;
            is_dead = false;
            animator = GetComponent<Animator> ();
            attack attack = gameObject.GetComponent<attack> ();
            attack.set_attacker (speller);
            intensity = 4;
            light = transform.Find("light").gameObject.GetComponent<Light>();

            Vector2 position_center = speller.get_position2();
            transform.position = new Vector3(position_center.x, position_center.y - 0.08f, (is_front ? -0.01f : 0.01f));
            gameObject.GetComponent<TrailRenderer> ().enabled = true;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Update ()
    {
        if(!is_dead)
        {
            time_now++;
            if(time_now == time_max)
            {
                time_now = 0;
            }
            float rate = time_now * 1.0f / time_max;

            float x = - Mathf.Sin(2 * Mathf.PI * rate) * 0.3f;
            float y = Mathf.Cos(2 * Mathf.PI * rate) * 0.04f;
            is_front = y < 0;

            //y -= Mathf.Sin(2 * Mathf.PI * rate) * 0.05f;
            Vector2 position_center = speller.get_position2();
            transform.position = new Vector3(position_center.x + x, position_center.y - 0.05f + y, (is_front ? -0.01f : 0.01f));
        }
        else
        {
        	if(is_anime_now_name("star2"))
            {
                light.intensity = intensity * 4f;
            }
            if(is_anime_now_name("blank"))
            {
                float time_left = (1 - get_anime_normalized_time()) * 4f;
                if(time_left > 0)
                {
                    light.intensity = (time_left) * intensity;
                }
                else
                {
                    Destroy (gameObject);
                }
            }
        }


    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        bear bear = collider.gameObject.GetComponent<bear> ();
        if (bear != null)
        {
            charactor bearer = bear.get_parent ();
            if (speller != bearer)
            {
                animator.SetBool(animator.parameters[0].nameHash, true);
                is_dead = true;
            }
        }

    }
    private bool is_anime_now_name(string anime_name)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.IsName("Base Layer." + anime_name);
    }
    protected float get_anime_normalized_time()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateinfo.normalizedTime;
    }
}