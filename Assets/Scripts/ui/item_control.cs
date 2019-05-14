using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item_control : MonoBehaviour
{
    public Sprite[] item_icon;
    private player target;
    private Dictionary<int, int> item_map = new Dictionary<int, int> ();
    private List<int> item_list = new List<int>();
    private int item_index;

    private Dictionary<int, string> item_name = new Dictionary<int, string> ();
    private Dictionary<int, string> item_rune_name = new Dictionary<int, string> ();

    private SpriteRenderer scroll1_renderer;
    private SpriteRenderer scroll2_renderer;
    private SpriteRenderer scroll3_renderer;

    private Text scroll_text;
    private Text rune_text;
    private Text item_index_text;

    private int shine_rate;
    private int shine_rate_max;
    private int shine_rate_max2;

    private bool shine_direction;
    private bool shine_switch;
    private bool shine_switch_cache;

    void Start ()
    {
        target = GameObject.Find ("hero").GetComponent<player> ();
        item_map = target.get_item_map ();
        item_list = target.get_item_list();

        item_index = -1;

        scroll1_renderer = transform.Find ("icon/icon_back").gameObject.GetComponent<SpriteRenderer>();
        scroll2_renderer = transform.Find ("icon/icon_mid").gameObject.GetComponent<SpriteRenderer>();
        scroll3_renderer = transform.Find ("icon/icon_top").gameObject.GetComponent<SpriteRenderer>();

        scroll_text = transform.Find ("item_now/canvas/text").gameObject.GetComponent<Text>();
        rune_text = transform.Find ("icon/rune/canvas/text").gameObject.GetComponent<Text>();
        item_index_text = transform.Find ("item_index/canvas/text").gameObject.GetComponent<Text>();

        shine_rate = 0;
        shine_rate_max = 200;
        shine_rate_max2 = 150;

        shine_direction = true;
        shine_switch = true;

        item_name[100] = "定身咒";
        item_name[101] = "火花令";
        item_name[102] = "濟生符";
        item_name[103] = "冲岩令";

        item_rune_name[100] = "DSZ";
        item_rune_name[101] = "HHLL";
        item_rune_name[102] = "JSF";
        item_rune_name[103] = "CYLL";

    }
    void Update ()
    {
        //deal_scroll();
        if (item_index != target.get_item_index_now ())
        {
            item_index = target.get_item_index_now ();
            draw_icon ();
        }
    }
    private void draw_icon ()
    {
        scroll_text.text = get_scroll_name(item_list[item_index]);
        rune_text.text = get_scroll_rune_name(item_list[item_index]);
        item_index_text.text =string.Format("{0:D2}", item_index+1);

    }
    private string get_scroll_name(int item_id)
    {
        if(item_name.ContainsKey(item_id))
        {
            return item_name[item_id];
        }
        else
        {
            return "???";
        }
    }
    private string get_scroll_rune_name(int item_id)
    {
        if(item_rune_name.ContainsKey(item_id))
        {
            return item_rune_name[item_id];
        }
        else
        {
            return "ABCD";
        }
    }
    private void deal_scroll()
    {
        if(shine_switch)
        {
            if(shine_direction)
            {
                shine_rate += 2;
                if(shine_rate == shine_rate_max)
                {
                    shine_direction = !shine_direction;
                }
            }
            else
            {
                shine_rate -= 2;
                if(shine_rate == 0)
                {
                    shine_direction = !shine_direction;
                }
            }
        }
        else
        {
            shine_rate = 0;
            shine_direction = true;
        }
        float rate = shine_rate * 1.0f / shine_rate_max2;
        if(rate > 1)
        {
            rate = 1f;
        }
        scroll2_renderer.color = new Color(1 - 0.1176f * (1 - rate), 1 - 0.3961f * (1 - rate), 1 - 0.7137f * (1 - rate));

    }

}