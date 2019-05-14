using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class against_wall : MonoBehaviour
{

    public int direction = 0;
    public bool statu = false;
    private charactor parent;
    // Use this for initialization
    void Start ()
    {
        parent = transform.parent.gameObject.GetComponent<charactor> ();
    }
    // Update is called once per frame
    void Update () { }
    void OnTriggerExit2D (Collider2D collision)
    {
        switch(direction)
        {
        case 0:
            parent.set_in_air(true);
            statu = true;
            break;
        case 1:
            parent.set_against_wall (false);
            statu = false;
            break;
        case 2:
            parent.set_against_wall_2 (false);
            statu = false;
            break;
        case 3:
            parent.set_against_ceiling(false);
            statu = false;
            break;
        case 4:
            parent.set_against_monster(false);
            parent.set_jump_target(collision.gameObject.GetComponent<charactor>());
            statu = false;
            break;
        }
    }
    void OnTriggerStay2D (Collider2D collision)
    {
        switch(direction)
        {
        case 0:
            parent.set_in_air(false);
            statu = false;
            break;
        case 1:
            parent.set_against_wall (true);
            statu = true;
            break;
        case 2:
            parent.set_against_wall_2 (true);
            statu = true;
            break;
        case 3:
            parent.set_against_ceiling(true);
            statu = true;
            break;
        case 4:
            parent.set_against_monster(true);
            statu = true;
            break;
        }
    }
}