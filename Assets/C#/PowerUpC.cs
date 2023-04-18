using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PowerUpC : MonoBehaviour
{
    public event Action<PowerUpC> onColleted;

    private void OnTriggerEnter(Collider other)
    {
        //Cuando el player choca con el powerUp se activa el event y sus sucribers
        if (other.gameObject.tag == "Player")
        {
            onColleted?.Invoke(this);//para q se invoque requiere que se le pase un objeto de tipo "PowerUpC" 
            gameObject.SetActive(false);
        }
    }
}
