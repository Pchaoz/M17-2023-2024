using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI m_TextShuricken;


    [SerializeField]
    private TMPro.TextMeshProUGUI m_TextPostura;

    [SerializeField]
    private CharacterController m_CharacterController;


    private void Awake()
    {
        m_CharacterController.OnActualitzarMunicio += ActualitzaMunicio;
        m_CharacterController.OnActualitzarPostura += ActualitzaPostura;
    }

    private void ActualitzaMunicio(int n_shuri)
    {
        m_TextShuricken.text = " Numero de Shuriken: " + n_shuri ;
    }

    private void ActualitzaPostura(string n_post)
    {
        m_TextPostura.text = n_post;
    }
    private void OnDestroy()
    {
        m_CharacterController.OnActualitzarMunicio -= ActualitzaMunicio;
        m_CharacterController.OnActualitzarPostura -= ActualitzaPostura;

    }
}



