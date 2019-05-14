using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy3 : enemy
{
    private int attack_cd_max;
    private int attack_cd_now;

    public override void Start3 ()
    {
        attack_cd_max = 90;
        attack_cd_now = 0;
    }
    // Update is called once per frame
    public override void Update3 ()
    {
        if(hp_now == 0)
        {
            floating_control (0.2f, 0.8f, 1.6f);
        }
        else
        {
            floating_control (0.4f, 0.5f, 1.0f);
        }

        if(attack_cd_now > 0)
        {
            attack_cd_now--;
        }

        die_control(2);
        if(is_dead)
        {
            return;
        }

        hit_control(0, 1);
        if(is_stun)
        {
            return;
        }

        move_lock = false;
        attack_lock = false;

        if (is_anime_now_name ("idle"))
        {
            move_lock = true;
            attack_lock = true;
            skill (-1);
        }
        else if (is_anime_now_name ("run"))
        {
            move_lock = true;
            attack_lock = true;
            if (get_anime_normalized_time () % 1 > 0.5)
            {
                move (direction);
            }else{
                move (direction,speed/2);
            }
            skill (-1);
        }
        else if (is_anime_now_name ("attack1"))
        {
            set_anime_para (-1);
            if (get_anime_normalized_time () > 0.857f)
            {
                skill (-1);
            }
            else if (get_anime_normalized_time () > 0.571f)
            {
                skill (0);
            }
            else if(get_anime_normalized_time () > 0.285f)
            {
                set_speed ((direction ? 1 : -1)*speed * 1.2f, rb.velocity.y);
            }
        }

    }

    public override void think ()
    {
        switch (about_to_do)
        {
        case 0:
            if (target != null)
            {
                about_to_do = 1;
            }
            else
            {
                about_to_do = 0;
            }
            break;
        case 1:
            if(target == null){
                about_to_do = 0;
            }
            break;
        }
    }

    public override void action ()
    {
        switch (about_to_do)
        {
        case 0:
            break;
        case 1:
            if (Mathf.Abs (target.get_position2 ().x - get_position2 ().x) > 0.4)
            {
                approach (3);
            }
            else
            {
                if(attack_cd_now == 0)
                {
                    attack (4);
                    attack_cd_now = attack_cd_max;
                }
            }
            break;
        }
    }
}