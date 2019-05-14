using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock1 : spell_script
{

    private Animator animator;
    private List<GameObject> skills = new List<GameObject>() ;
    private int skill_spelling = -1;
    void OnEnable ()
    {
        if (speller != null)
        {
            animator = GetComponent<Animator> ();
            transform.position = new Vector3 (direction2.x, direction2.y, transform.position.z);

            foreach (Transform child in transform.Find("skills"))
            {
                GameObject child_obj = child.gameObject;
                child.gameObject.SetActive(false);
                skills.Add(child_obj);
                attack attack = child_obj.GetComponent<attack>();
                if (attack != null)
                {
                    attack.set_attacker(speller);
                }
            }
        }
    }
    void Update ()
    {
        if(is_anime_now_name("rock1_1"))
        {
            if(get_anime_normalized_time() > 0.5)
            {
                skill(0);
            }
            else
            {
                skill(-1);
            }
        }
        if(is_anime_now_name("rock1_2"))
        {
            skill(-1);
        }
        if(is_anime_now_name("rock1_3"))
        {
            if(get_anime_normalized_time() > 0.2 && get_anime_normalized_time() < 0.8 )
            {
                skill(1);
            }
            else
            {
                skill(-1);
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
    private void skill(int index)
    {
        if (skill_spelling != index)
        {
            if (skill_spelling != -1)
            {
                skills[skill_spelling].SetActive(false);
            }
            if (index != -1)
            {
                skills[index].SetActive(true);
            }
            skill_spelling = index;
        }
    }
}