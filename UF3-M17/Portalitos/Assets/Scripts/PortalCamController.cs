using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PlayerCam;
    [SerializeField]
    private GameObject m_MyCam;
    [SerializeField]
    private GameObject m_PortalCam;

    // Update is called once per frame
    void Update()
    {
        Quaternion distance = Quaternion.Inverse(m_MyCam.transform.rotation) * m_PlayerCam.transform.rotation;
        m_PortalCam.transform.localEulerAngles = new Vector3(distance.eulerAngles.x, distance.eulerAngles.y + 180 + distance.eulerAngles.z);

        Vector3 distancia = m_MyCam.transform.InverseTransformPoint(m_PlayerCam.transform.position);
        m_PortalCam.transform.localPosition = new Vector3(distancia.x, -distancia.y, distancia.z);
    }
}
