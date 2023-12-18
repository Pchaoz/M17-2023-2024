using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Target;
    public GameObject Target => m_Target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Target = other.gameObject; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Target = null;
        }
    }
}
