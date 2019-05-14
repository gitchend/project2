using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bear : MonoBehaviour
{
    private charactor bearer;
    private player player;
    private GameObject parent;
    private Rigidbody2D rb;
    private effect_controller ec;
    private audio_controller ac;
    private buff_controller bc;

    void Start ()
    {
        parent = transform.parent.gameObject;

        bearer = parent.GetComponent<charactor> ();
        player = GameObject.Find ("hero").GetComponent<player> ();

        rb = parent.GetComponent<Rigidbody2D> ();
        ec = GameObject.Find ("effect_controller").GetComponent<effect_controller> ();
        ac = GameObject.Find ("audio_controller").GetComponent<audio_controller> ();
        bc = GameObject.Find ("buff_controller").GetComponent<buff_controller> ();
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        attack attack = collider.gameObject.GetComponent<attack> ();
        charactor attacker = attack.get_attacker ();
        if (attacker == bearer)
        {
            return;
        }
        if (!attack.is_valid ())
        {
            return;
        }
        if(!bearer.get_is_attackable())
        {
            return;
        }
        //flag only one hit attack
        attack.attacked ();


        float position_x = bearer.transform.position.x - attacker.transform.position.x;
        bearer.set_direction (position_x < 0);

        Vector2 derection = transform.position - attacker.transform.position;
        rb.velocity = new Vector2 ((attack.beatback_damage * (derection.x > 0 ? 1 : -1)), rb.velocity.y);
        //floating
        if (attack.is_ground_floating || bearer.get_in_air ())
        {
            rb.velocity = new Vector2 (rb.velocity.x, attack.floating_damage);
        }

        attacker.add_speed(attack.self_beatback_damage * (position_x < 0 ? 1 : -1), attack.self_floating_damage);

        bearer.set_hp (bearer.hp_now - attack.damage);
        bc.create_buff (1, attacker, bearer, attack.stun, attack.frame_extract);

        //face to attacker
        bearer.set_last_attacked (attacker);
        attacker.set_last_attack(bearer);

        attacker.hit_message (attack);
        bearer.hitted_message(attack);
        //frame_extract
        bc.create_buff(3, bearer, bearer, Mathf.Max(attack.frame_extract,1), 0);
        if (attack.is_melee)
        {
            bc.create_buff(3, attacker, attacker, attack.frame_extract, 0);
        }

        //effect
        ec.create_effect (1, false, parent, new Vector2(0, 0), bearer, attack.frame_extract);
        if(attack.damage > 8)
        {
            ec.create_effect (2, (Random.value > 0.5f), parent, new Vector2(0, 0), bearer, 0);
        }
        else
        {
            ec.create_effect (14, (Random.value > 0.5f), parent, new Vector2(0, 0), bearer, 0);
        }

        //ec.create_effect (5, bearer.get_direction(), parent, new Vector2(0,0), bearer, 0);


        if(attack.attack_kind == 0)
        {
            ec.create_effect (10, !bearer.get_direction(), parent, new Vector2((bearer.get_direction() ? -1 : 1) * (Random.value * 0.1f - 0.05f + 0.1f), Random.value * 0.1f - 0.12f), bearer, 0, attack.attack_direction);
        }
        if(attacker.is_player)
        {
            if(attack.frame_extract > 5)
            {
                if(Mathf.Abs(attack.attack_direction.x) >= Mathf.Abs(attack.attack_direction.y))
                {
                    player.set_camera_shaking_code(attack.frame_extract * 10 + 1);
                }
                else
                {
                    if(attack.attack_direction.y > 0)
                    {

                        player.set_camera_shaking_code(attack.frame_extract * 10 + 2);
                    }
                    else
                    {
                        player.set_camera_shaking_code(attack.frame_extract * 10 + 3);

                    }
                }
            }
        }

        //stun
        bc.create_buff(100, bearer, bearer, 10, 0);

        //audio
        ac.create_audio ((Random.value > 0.5f ? 1 : 2), attack.frame_extract);
    }

    void OnTriggerExit2D (Collider2D collider) { }
    void OnTriggerStay2D (Collider2D collider) { }
    public charactor get_parent ()
    {
        return bearer;
    }
}