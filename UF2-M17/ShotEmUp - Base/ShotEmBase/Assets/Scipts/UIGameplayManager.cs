using TMPro;
using UnityEngine;

public class UIGameplayManager : MonoBehaviour
{
    [Header("Respective texts from the UI")]
    [SerializeField]
    private TextMeshProUGUI m_RoundsText;
    [SerializeField]
    private TextMeshProUGUI m_PlayerHp;

    public void OnRoundsChange(int round)
    {
        m_RoundsText.text = "Ronda: " + round;
    }
    public void OnHpChange(int hp)
    {
        m_PlayerHp.text = "Vida: " + hp + "/50";
    }
}
