using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CoinC : MonoBehaviour
{
    //Valor de la moneda
    public int coinValue = 10;
    //Evento para cuando la moneda es recolectada
    public event Action<CoinC> onColleted;

    
    private void OnTriggerEnter(Collider other)
    {
        //Cuando el player choca con la moneda se activa el event y sus sucribers
        if(other.gameObject.tag == "Player")
        {
            onColleted?.Invoke(this);//para q se invoque requiere que se le pase un objeto de tipo "CoinC" 
            gameObject.SetActive(false);
        }        
    }
}
