using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpBar : MonoBehaviour, IPlayerHpView
{
    [SerializeField] private Slider _sliderHpBar;
    [SerializeField] private TextMeshProUGUI _txtPlayerHpValue;
    public void UpdateHpUI(float curHp, float maxHp)
    {
        _sliderHpBar.value = curHp / maxHp;
        _txtPlayerHpValue.text = $"{curHp} / {maxHp}";
    }
}
