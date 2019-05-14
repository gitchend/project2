using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star_scroll : spell_script
{
    private int max_num=5;
    private int wait_time_f=10;

    void OnEnable ()
    {
        if (speller != null)
        {
            for(int i = 0; i < max_num; i++)
            {
                int wait_time = i * wait_time_f;
                sc.create_spell (8, speller, new Vector2 (0, 0), wait_time, 0);
            }
            Destroy(gameObject);
        }
    }

}