using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
public class EnemyC : MonoBehaviour
{
    [Header("Datos de todos los enemigos")]
    [SerializeField] public float damage = 0.5f;
    [SerializeField] float vida;
    [SerializeField] public float speedEnemy = 3f;
    [SerializeField] string name= "BadBoy";
    Rigidbody rigidbody;
    [Header("RaycastPatrullaje")]
    [SerializeField] Transform controladorSuelo;
    [SerializeField] private float distanceModifier = 0.9f;
    [SerializeField] private LayerMask myLayers;
    [Header("RaycastVision")]
    [Range(2, 10)] [SerializeField] private float distanceModifierVista = 2.5f;
    [SerializeField] Transform controladorVision;
    [SerializeField] Transform ObjetivoPlayer;
    [SerializeField] private LayerMask myLayerPlayer;

    public event Action<EnemyC> OnHitEnemy;
    public event Action<EnemyC> OnKillEnemy;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RangoVision();
    }
    public void Patrullaje()
    {
        RaycastHit hit;
        Ray raycast = new Ray(controladorSuelo.position, controladorSuelo.up*-1);
        Debug.DrawRay(raycast.origin, raycast.direction * distanceModifier,Color.red);

        rigidbody.velocity = new Vector2(speedEnemy, rigidbody.velocity.y);

        if (Physics.Raycast(raycast, out hit, distanceModifier, myLayers)==false)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            speedEnemy *= -1;
            
        }       

    }
    public void RangoVision()
    {
        RaycastHit hit;
        Ray raycast = new Ray(controladorVision.position, controladorVision.forward);
        Debug.DrawRay(raycast.origin, raycast.direction * distanceModifierVista, Color.green);
        if (Physics.Raycast(raycast,out hit, distanceModifierVista, myLayerPlayer))
        {
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(ObjetivoPlayer.position.x, ObjetivoPlayer.position.y, ObjetivoPlayer.position.z), speedEnemy * Time.deltaTime);
            rigidbody.position = Vector3.MoveTowards(transform.position, ObjetivoPlayer.position, 20 * Time.deltaTime);
        }
        else
        {
            Patrullaje();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnHitEnemy?.Invoke(this);
            MovimientoCa.Instance.MoverCamara(5,5,0.5f);
            collision.gameObject.transform.position= new Vector3(collision.gameObject.transform.position.x + 0.3f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z + 0.3f);
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnKillEnemy?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
