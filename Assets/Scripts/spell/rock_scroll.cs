using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock_scroll : spell_script
{
    private int max_num=6;
    private int wait_time_f=5;

    void OnEnable ()
    {
        if (speller != null)
        {
            bool direction = speller.get_direction();
            Vector2 parent_position = speller.get_position2 ();
            transform.position = new Vector3 (parent_position.x + (direction ? 1 : -1) * 0.5f, parent_position.y, transform.position.z);

            for(int i = 0; i <= max_num; i++)
            {
                int wait_time = i * wait_time_f;
                float position_x = transform.position.x + (direction ? 1 : -1) * 0.2f * i;
                float position_y = (i==max_num?transform.position.y +0.05f:transform.position.y +0.01f*i- Random.value * 0.1f);

                sc.create_spell (7, speller, new Vector2 (position_x, position_y), wait_time, 0);
            }
            Destroy(gameObject);
        }
    }

}