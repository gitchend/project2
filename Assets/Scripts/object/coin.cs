using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public int coin_num = 1;
    private int lock_time = 30;

    void Start()
    {
        transform.parent.GetComponent<Rigidbody2D> ().velocity = new Vector2((Random.value * 2 - 1) * 1, 3);
    }
    void Update(){
    	if(lock_time>0){
    		lock_time--;
    	}
    }
    void OnTriggerStay2D (Collider2D collider)
    {
        if(lock_time == 0)
        {
            player player = collider.gameObject.transform.parent.GetComponent<player> ();
            player.add_coin(coin_num);
            player.get_ec().create_effect (15, false, new Vector2(transform.position.x, transform.position.y), 0);

            Destroy (transform.parent.gameObject);

        }
    }
}