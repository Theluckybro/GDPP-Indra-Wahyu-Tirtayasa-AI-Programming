using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiObjects;
    [SerializeField] private Image _staminaFill;

    public void SetVisible(bool value)
    {
        _uiObjects?.SetActive(value);
    }
    public void SetStaminaFill(float value, float maxValue)
    {
        if (_staminaFill != null)
        {
            _staminaFill.fillAmount = value / maxValue;
        }
    }
}
