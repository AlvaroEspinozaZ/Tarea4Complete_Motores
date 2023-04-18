using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovimientoCa : MonoBehaviour
{
    public static MovimientoCa Instance;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    float tiempoMovimiento;
    float tiempoMovimientoTotal;
    float intensidadInicial;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

   public void MoverCamara(float intensidad, float frecuencia, float tiempo)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensidad;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frecuencia;
        intensidadInicial = intensidad;
        tiempoMovimientoTotal = tiempo;
        tiempoMovimiento = tiempo;
    }

    private void Update()
    {
        if(tiempoMovimiento > 0)
        {
            tiempoMovimiento -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensidadInicial, 0, 1 - (tiempoMovimiento / tiempoMovimientoTotal));
        }
        
    }
}
