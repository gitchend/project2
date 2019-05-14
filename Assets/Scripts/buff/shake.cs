using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shake : buff
{
    private GameObject obj = null;
    private bool is_hit_effect = false;
    private SpriteRenderer renderer;
    private SpriteRenderer renderer2;
    private GameObject renderer2_obj;

    public override void init2 ()
    {
        obj = buff_to.get_sprite_obj ();
    }
    public override void update ()
    {
        obj.transform.localPosition = new Vector2 (((int) (Mathf.Sin (time_now * 1.2f) * 3) / 64.0f), ((int) (Mathf.Sin (time_now * 1.2f) * 1) / 64.0f));

        if(is_hit_effect && renderer2 != null)
        {
            renderer2.sprite = get_white_sprite(renderer.sprite);
        }
    }
    public override void start ()
    {
        renderer = obj.GetComponent<SpriteRenderer>();
        if(renderer != null)
        {
            is_hit_effect = true;
            GameObject img_obj = Resources.Load("img_obj") as GameObject;
            renderer2_obj = buff_controller.Instantiate (img_obj) as GameObject;
            renderer2_obj.transform.parent = obj.transform;
            renderer2_obj.transform.localPosition = new Vector3(0, 0, 0);

            renderer2 = renderer2_obj.GetComponent<SpriteRenderer>();
            renderer.color = new Color(1, 1, 1, 0);
        }
    }
    public override void end ()
    {
        obj.transform.localPosition = new Vector3 (0, 0, 0);
        renderer.color = new Color(1, 1, 1, 1);
        buff_controller.Destroy(renderer2_obj);
    }

    private Sprite get_white_sprite(Sprite sprite)
    {
        if(sprite == null)
        {
            return null;
        }
        int sprite_x_min = (int)sprite.rect.xMin;
        int sprite_y_min = (int)sprite.rect.yMin;
        int sprite_width = (int)sprite.rect.width;
        int sprite_height = (int)sprite.rect.height;
        Texture2D texture2D = new Texture2D(sprite_width, sprite_height, sprite.texture.format, false);
        texture2D.SetPixels(sprite.texture.GetPixels(sprite_x_min, sprite_y_min, sprite_width, sprite_height));
        texture2D.filterMode = FilterMode.Point;

        int[,] location = new int[4, 2] {{0, -1}, {0, 1}, {-1, 0}, {1, 0}};
        for(int i = 0; i < sprite_width; i++)
        {
            for(int j = 0; j < sprite_height; j++)
            {
                Color color_set = sprite.texture.GetPixel(i + sprite_x_min, j + sprite_y_min);
                if(color_set.a == 0)
                {
                    continue;
                }
                texture2D.SetPixel(i, j, new Color(1, 1, 1, 1));
            }
        }
        texture2D.Apply();
        Rect rect = new Rect (0, 0, sprite_width, sprite_height);
        Sprite new_sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f), 64.0f);
        new_sprite.name = sprite.name;
        return new_sprite;
    }
}