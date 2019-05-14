﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class camera_controller : MonoBehaviour
{
    public int kind_of_map = 0 ;
    private int reletive_rate;
    private GameObject target;
    private player player;

    private GameObject ui_group1;
    private GameObject ui_group2;
    private GameObject ui_group3;
    private GameObject ui_group3_renderer_1;
    private GameObject ui_group3_renderer_2;
    private GameObject loading;

    private List<background_layer> layers = new List<background_layer>();
    private GameObject layer1;
    private GameObject layer2;
    private GameObject layer3;
    private GameObject layer4;

    private GameObject background_mask;
    private int screen_width;
    private int screen_height;
    private float img_x;
    private float img_y;

    private GameObject image_obj;
    private GameObject image_obj_light;

    private Vector3 camera_positon_to;
    private Vector3 camera_positon_now;
    private Vector2 camera_positon_offset;
    private Vector3 camera_velocity = Vector3.zero;
    private Vector2 camera_shake_offset = Vector2.zero;
    private int camera_shake_delay = 0;

    private float map_size_x;
    private float map_size_y;

    void Start()
    {
        player = GameObject.Find("hero").GetComponent<player>();
        target = player.transform.Find("sprite").gameObject;
        ui_group1 = GameObject.Find("ui_group1");
        ui_group2 = GameObject.Find("ui_group2");
        ui_group3 = GameObject.Find("ui_group3");
        loading = GameObject.Find("loading");

        ui_group3_renderer_1 = ui_group3.transform.Find("ui_group3_1").gameObject;
        ui_group3_renderer_2 = ui_group3.transform.Find("ui_group3_2").gameObject;

        layer1 = GameObject.Find("layer1");
        layer2 = GameObject.Find("layer2");
        layer3 = GameObject.Find("layer3");
        layer4 = GameObject.Find("layer4");
        if (layer1 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer1;
            background_layer.set = layer1.GetComponent<background_set>();
            background_layer.speed_rate = 1.0f;
            background_layer.is_light = false;
            background_layer.pixel_basic = target;
            background_layer.move_per_pixel = 10;
            layers.Add(background_layer);
        }
        if (layer2 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer2;
            background_layer.set = layer2.GetComponent<background_set>();
            background_layer.speed_rate = 0.953125f;
            background_layer.is_light = false;
            background_layer.pixel_basic = layer1;
            background_layer.move_per_pixel = 8;
            layers.Add(background_layer);
        }
        if (layer3 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer3;
            background_layer.set = layer3.GetComponent<background_set>();
            background_layer.speed_rate = 0.5f;
            background_layer.is_light = true;
            background_layer.pixel_basic = layer2;
            background_layer.move_per_pixel = 5;
            layers.Add(background_layer);

            background_layer.move_per_pixel = 5;
        }
        if (layer4 != null)
        {
            background_layer background_layer = new background_layer();
            background_layer.layer_obj = layer4;
            background_layer.set = layer4.GetComponent<background_set>();
            background_layer.speed_rate = 0.296875f;
            background_layer.is_light = true;
            background_layer.pixel_basic = layer3;
            layers.Add(background_layer);

            background_layer.move_per_pixel = 3;
        }

        background_mask = GameObject.Find("background_mask");

        image_obj = Resources.Load("img_obj") as GameObject;
        image_obj_light = Resources.Load("img_obj_light") as GameObject;
        refresh_screen_size();

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);


        switch(kind_of_map)
        {
        case 1:
            map_size_x = 4.5f;
            map_size_y = 0f;
            break;
        case 2:
            map_size_x = 31;
            map_size_y = 15;
            break;
        case 3:
            map_size_x = 15;
            map_size_y = 23;
            break;
        }
        map_size_x -= (int)(screen_width * 1.0f / reletive_rate / 2) / 64.0f ;
        map_size_y -= (int)(screen_height * 1.0f / reletive_rate / 2) / 64.0f ;

    }
    // Update is called once per frame
    void LateUpdate()
    {
        focous_target();
        adjust_ui();
        adjust_background();
        adjust_mask();
    }
    private void focous_target()
    {
        bool is_player_run = player.is_anime_now_name("run1") || player.is_anime_now_name("run2");
        Vector2 camera_target_offset = new Vector2((player.get_direction() ? 1 : -1) * 0.5f, 0.5f);
        camera_positon_to = new Vector3(target.transform.position.x + camera_target_offset.x, target.transform.position.y + camera_target_offset.y, transform.position.z);

        // if((camera_positon_to - transform.position).magnitude > 0.1f)
        // {
        //     camera_positon_now = Vector3.SmoothDamp(camera_positon_now, camera_positon_to, ref camera_velocity, 0.2f);
        //     transform.position = new Vector3(pixel_fix(camera_positon_now.x), pixel_fix(camera_positon_now.y), transform.position.z);

        // }
        // else
        // {
        //     transform.position  = camera_positon_to;
        // }
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 0.25f, transform.position.z);
        if(kind_of_map != 0)
        {
            float new_trans_x = transform.position.x;
            float new_trans_y = transform.position.y;
            if(map_size_x < 0)
            {
                new_trans_x = 0;
            }
            else
            {
                if(new_trans_x < -map_size_x)
                {
                    new_trans_x = -map_size_x;
                }
                if(new_trans_x > map_size_x)
                {
                    new_trans_x = map_size_x;
                }
            }
            if(map_size_y < 0)
            {
                new_trans_y = 0;
            }
            else
            {
                if(new_trans_y < -map_size_y)
                {
                    new_trans_y = -map_size_y;
                }
                if(new_trans_y > map_size_y)
                {
                    new_trans_y = map_size_y;
                }
            }
            transform.position = new Vector3(new_trans_x, new_trans_y, transform.position.z);
        }

        int camera_shaking_code = player.get_camera_shaking_code();
        if(camera_shaking_code != 0)
        {
            player.set_camera_shaking_code(0);
            camera_shake_delay = camera_shaking_code / 10;
            int camera_shaking_mode = camera_shaking_code % 10;
            switch(camera_shaking_mode)
            {
            case 1:
                camera_shake_offset = new Vector2((player.get_direction() ? -1 : 1) * (0.02f * camera_shake_delay - 0.1f), 0);
                break;
            case 2:
                camera_shake_offset = new Vector2((player.get_direction() ? -1 : 1) * 0.01f, (0.02f * camera_shake_delay - 0.1f) * 0.5f);
                break;
            case 3:
                camera_shake_offset = new Vector2((player.get_direction() ? -1 : 1) * 0.01f, -(0.02f * camera_shake_delay - 0.1f) * 0.5f);
                break;
            }
        }
        else
        {
            if(camera_shake_delay > 0)
            {
                camera_shake_delay--;
            }
            else
            {
                if(camera_shake_offset.magnitude > 0.1f)
                {
                    camera_shake_offset =  new Vector2(camera_shake_offset.x * 0.7f, camera_shake_offset.y * 0.7f);
                }
                else
                {
                    camera_shake_offset = Vector2.zero;
                }
            }
        }
        transform.position = new Vector3(transform.position.x + camera_shake_offset.x, transform.position.y + camera_shake_offset.y, transform.position.z);

    }
    private void adjust_ui()
    {

        float ui_group1_x = transform.position.x - (int)(screen_width * 1.0f / reletive_rate * 0.47f) / 64.0f;
        float ui_group1_y = transform.position.y + (int)(screen_height * 1.0f / reletive_rate * 0.44f)  / 64.0f;

        float ui_group2_x = transform.position.x - (int)(screen_width * 1.0f / reletive_rate * 0.42f ) / 64.0f;
        float ui_group2_y = transform.position.y - (int)(screen_height * 1.0f / reletive_rate * 0.4f) / 64.0f;

        ui_group1.transform.position = new Vector3(ui_group1_x, ui_group1_y, ui_group1.transform.position.z);
        ui_group2.transform.position = new Vector3(ui_group2_x, ui_group2_y, ui_group2.transform.position.z);
        ui_group3.transform.position = new Vector3(transform.position.x, transform.position.y, ui_group3.transform.position.z);

        loading.transform.position = new Vector3(transform.position.x, transform.position.y, loading.transform.position.z);

    }
    private void adjust_background()
    {
        foreach (background_layer layer in layers)
        {
            move_img(layer);
            remove_invisible_img(layer);
            add_new_img(layer);
        }
        //adjust_pixel();

    }
    private void move_img(background_layer layer)
    {
        Vector2 position_miu = get_position_miu(layer.speed_rate, layer.set);
        layer.layer_obj.transform.position = new Vector3(position_miu.x + layer.set.position_x, position_miu.y + layer.set.position_y, layer.layer_obj.transform.position.z);
        // foreach (string key in layer.img_map.Keys)
        // {
        //     string[] index = key.Split('_');
        //     int index_x = int.Parse(index[0]);
        //     int index_y = int.Parse(index[1]);
        //     layer.img_map[key].transform.parent.position = new Vector3(index_x * img_x + position_miu.x, index_y * img_y + position_miu.y, layer.img_map[key].transform.parent.position.z);
        //     adjust_pixel(layer.img_map[key]);
        // }
    }
    private void remove_invisible_img(background_layer layer)
    {
        for (int i = layer.img_map.Count - 1; i >= 0; i--)
        {
            var element = layer.img_map.ElementAt(i);
            if (!is_img_visible(element.Value))
            {
                layer.img_map.Remove(element.Key);
                Destroy(element.Value);
            }
        }
    }
    private void add_new_img(background_layer layer)
    {
        Vector2 position_miu = get_position_miu(layer.speed_rate, layer.set);

        float visible_x_left = transform.position.x - (screen_width / 64.0f + img_x) / 2;
        float visible_x_right = visible_x_left + (screen_width / 64.0f + img_x);
        float visible_y_down = transform.position.y - (screen_height / 64.0f + img_y) / 2;
        float visible_y_up = visible_y_down + (screen_height / 64.0f + img_y);

        int x_low = (int)((visible_x_left - position_miu.x) / img_x);
        int x_high = (int)((visible_x_right - position_miu.x) / img_x);
        int y_low = (int)((visible_y_down - position_miu.y) / img_y);
        int y_high = (int)((visible_y_up - position_miu.y) / img_y);

        for (int i = x_low; i <= x_high; i++)
        {
            for (int j = y_low; j <= y_high; j++)
            {
                string key = i + "_" + j;
                if (!layer.img_map.ContainsKey(key))
                {
                    GameObject obj = Instantiate((layer.is_light ? image_obj_light : image_obj)) as GameObject;
                    obj.transform.parent = layer.layer_obj.transform;
                    obj.transform.localPosition = new Vector3(i * img_x, j * img_y, 0);
                    if (j == 0)
                    {
                        if (layer.set.imgs[0] != null)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = layer.set.imgs[0];
                        }
                    }
                    else if (j > 0)
                    {
                        if (layer.set.imgs[1] != null)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = layer.set.imgs[1];
                        }
                    }
                    else
                    {
                        if (layer.set.imgs[2] != null)
                        {
                            obj.GetComponent<SpriteRenderer>().sprite = layer.set.imgs[2];
                        }
                    }
                    layer.img_map[key] = obj;
                }
            }
        }
    }
    private void adjust_mask()
    {
        background_mask.transform.position = new Vector3(transform.position.x, transform.position.y, background_mask.transform.position.z);
        background_mask.transform.localScale = new Vector2(screen_width * 1.0f / reletive_rate, screen_height * 1.0f / reletive_rate);
    }
    private Vector2 get_position_miu(float speed_rate, background_set img_set)
    {
        Vector2 position_miu = transform.position * speed_rate;
        position_miu = new Vector2(position_miu.x + img_set.position_x, position_miu.y + img_set.position_y);
        return position_miu;
    }
    private bool is_img_visible(GameObject obj)
    {
        return obj.GetComponent<SpriteRenderer>().isVisible;
    }
    protected void adjust_pixel()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            float target_x;
            float target_y;

            layers[i].layer_position_unpixeled = new Vector2(layers[i].layer_obj.transform.position.x, layers[i].layer_obj.transform.position.y);
            if(i == 0)
            {
                target_x = layers[i].pixel_basic.transform.position.x;
                target_y = layers[i].pixel_basic.transform.position.y;
            }
            else
            {
                target_x = layers[i - 1].layer_position_unpixeled.x;
                target_y = layers[i - 1].layer_position_unpixeled.y;
            }
            float position_miu_x = layers[i].layer_obj.transform.position.x - target_x;
            float position_miu_y = layers[i].layer_obj.transform.position.y - target_y;
            //layers[i].layer_obj.transform.position = new Vector3(layers[i].pixel_basic.transform.position.x + pixel_fix(position_miu_x), layers[i].pixel_basic.transform.position.y + pixel_fix(position_miu_y), layers[i].layer_obj.transform.position.z);
            layers[i].layer_obj.transform.position = new Vector3(pixel_fix(layers[i].layer_obj.transform.position.x),  pixel_fix(layers[i].layer_obj.transform.position.y), layers[i].layer_obj.transform.position.z);
        }

    }
    private float pixel_fix(float float_num)
    {
        return (int)(float_num * 64) / 64.0f;
    }
    private Vector2 pixel_fix(Vector2 vector2)
    {
        return new Vector2(pixel_fix(vector2.x), pixel_fix(vector2.y));
    }
    private float pixel_fix(float float_num, int move_per_pixel)
    {
        int pix = (int)(float_num * 64);
        pix = pix - (pix + move_per_pixel) % move_per_pixel;
        return pix / 64.0f;
    }
    private Vector2 pixel_fix(Vector2 vector2, int move_per_pixel)
    {
        return new Vector2(pixel_fix(vector2.x, move_per_pixel), pixel_fix(vector2.y, move_per_pixel));
    }
    private Vector3 pixel_fix(Vector3 vector3)
    {
        return new Vector3(pixel_fix(vector3.x), pixel_fix(vector3.y), pixel_fix(vector3.z));
    }
    public void refresh_screen_size()
    {
        screen_width = Screen.width;
        screen_height = Screen.height;
        int width_rate = (int)(screen_width / 256.0f);
        int height_rate = (int)(screen_height / 144.0f);
        reletive_rate = (width_rate < height_rate ? width_rate : height_rate);
        if(reletive_rate > 1)
        {
            reletive_rate -= 1;
        }
        img_x = 512 / 64.0f;
        img_y = 288 / 64.0f;

        float camera_size = screen_height * 1.0f / reletive_rate / 2 / 64;
        gameObject.GetComponent<Camera>().orthographicSize = camera_size ;

        ui_group3_renderer_1.transform.localScale = new Vector2(screen_width * 1.0f / reletive_rate, screen_height * 1.0f / reletive_rate * 0.75f);
        ui_group3_renderer_2.transform.localScale = new Vector2(screen_width * 1.0f / reletive_rate, screen_height * 1.0f / reletive_rate);
        ui_group3_renderer_1.transform.localPosition = new Vector3(0,  - screen_height * 0.125f / reletive_rate / 64.0f, ui_group3_renderer_1.transform.localPosition.z);
        ui_group3_renderer_2.transform.localPosition = new Vector3(0, 0, ui_group3_renderer_2.transform.localPosition.z);

    }
}