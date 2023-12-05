using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Collider m_Col;

    private void Awake()
    {
        m_Col = GetComponent<Collider>();
    }
    
    public void NoCol()
    {
        m_Col.enabled = false;
        StartCoroutine(NoPortalCol());
    }

    IEnumerator NoPortalCol()
    {
        //COORUTINA QUE DESPUES DE DOS SEGUNDOS VUELVE A ACTIVAR EL COLIDER DE EL PORTAL
        yield return new WaitForSeconds(0.3f);
        m_Col.enabled = true;
    }
}
