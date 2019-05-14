using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : charactor
{
    protected int about_to_do = 0;

    protected bool move_lock = false;
    protected bool attack_lock = false;

    protected int envy_change_time = 2000;
    protected int envy_time = 0;
    protected GameObject img_obj_disappear;

    public override void Start2 ()
    {
        img_obj_disappear = Resources.Load("img_obj_disappear") as GameObject;
        Start3();
    }
    // Update is called once per frame
    public override void Update2 ()
    {
        envy_control ();
        against_wall_control();

        Update3();
        if(is_dead)
        {
            return;
        }

        think();
        if(is_stun)
        {
            return;
        }
        action();
    }

    public virtual void Start3() {}
    public virtual void Update3() {}

    public virtual void think () {}
    public virtual void action () {}

    private void envy_control ()
    {
        if (envy_time > 0)
        {
            envy_time--;
            return;
        }
        if (last_attacked != null)
        {
            target = last_attacked;
            envy_time = envy_change_time;
        }
    }
    private void against_wall_control()
    {

        if(against_wall)
        {
            set_speed((direction ? -1 : 1) * 0.1f, rb.velocity.y);
        }
        if(against_wall_2)
        {
            set_speed((direction ? 1 : -1) * 0.1f, rb.velocity.y);
        }
    }

    protected void hit_control(int hit1_code, int hit2_code)
    {
        if (is_stun)
        {
            if (is_anime_now_name ("hit1"))
            {
                set_anime_para (-1);
            }
            else if (is_anime_now_name ("hit2"))
            {
                if (is_hitted)
                {
                    set_anime_para (hit1_code);
                    is_hitted = false;
                }
            }
            else
            {
                set_anime_para (hit1_code);
            }
        }
        else
        {
            if (is_anime_now_name ("hit1"))
            {
                set_anime_para (-1);
            }
            else if (is_anime_now_name ("hit2"))
            {
                set_anime_para (hit2_code);
            }
        }
    }
    protected void approach (int walk_anime_id)
    {
        if (move_lock)
        {
            bool direction_miu = target.get_position2 ().x - get_position2 ().x > 0;
            if (direction_miu != direction)
            {
                turn ();
            }
            set_anime_para (walk_anime_id);
            if (against_wall)
            {
                set_speed (rb.velocity.x, jump_speed);
            }
        }
    }
    protected void attack (int attack_anime_id)
    {
        if (attack_lock)
        {
            set_anime_para (attack_anime_id);
        }
    }
    protected void die()
    {
        for(int i = 0; i < 10; i++)
        {
            ec.create_effect (10, direction, gameObject, new Vector2((direction ? -1 : 1) * (Random.value * 0.1f - 0.05f + 0.1f), Random.value * 0.1f - 0.2f), null, i * 2, new Vector2(-2 + i, 1));
        }
        GameObject shadow = Instantiate (img_obj_disappear) as GameObject;
        shadow.GetComponent<alpha_reduce> ().die_in_frame = 300;
        shadow.GetComponent<alpha_reduce> ().color = 1;
        shadow.GetComponent<alpha_reduce> ().start_rate = 1;
        shadow.GetComponent<SpriteRenderer> ().sprite = sprite.GetComponent<SpriteRenderer> ().sprite;
        shadow.transform.position = new Vector3(sprite.transform.position.x, sprite.transform.position.y, 0.1f);
        shadow.transform.localScale = new Vector3((direction ? 1 : -1), 1, 1);

        Destroy(gameObject);
    }
    protected void die_control(int die2_code)
    {
        if (is_anime_now_name ("die1"))
        {
            set_anime_para(-1);
        }
        else if (is_anime_now_name ("die2"))
        {
            if(once_in_animation())
            {
                for(int i = 0; i < 5; i++)
                {
                    ec.create_effect (10, direction, gameObject, new Vector2((direction ? -1 : 1) * (Random.value * 0.1f - 0.05f + 0.1f), Random.value * 0.1f - 0.2f), null, i * 2, new Vector2(-2 + i, 1));
                }
            }
            if(timer % 16 == 0)
            {
                ec.create_effect (11, direction, gameObject, new Vector2(0, 0), this, 0,  new Vector2(0, 0));
            }
            if(get_anime_normalized_time() > 0.5 && !in_air)
            {
                die();
            }
        }

        if(hp_now == 0 && !is_dead)
        {
            if(timer % 16 == 0)
            {
                ec.create_effect (11, direction, gameObject, new Vector2(0, 0), this, 0,  new Vector2(0, 0));
            }
        }
        if(hp_now == 0 && !is_dead && !in_air && !is_stun)
        {
            set_anime_para(die2_code);
            is_dead = true;
            set_speed(speed * (direction ? -1 : 1), jump_speed);

            create_coin(4);
            ec.create_effect (16, direction, new Vector2(transform.position.x, transform.position.y), 0);
        }
    }
    protected void create_coin(int coin_num)
    {
        for(int i = 0; i < coin_num; i++)
        {
            sc.create_spell(6, this);
        }
    }

}