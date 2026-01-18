using TMPro;
using UnityEngine;

public class UIPlayerGold : MonoBehaviour, IPlayerGoldView
{
    [SerializeField] private TextMeshProUGUI _txtHasGold;

    public void UpdateGoldUI(long curGold)
    {
        _txtHasGold.text = $"{curGold:N0}";
    }
}
