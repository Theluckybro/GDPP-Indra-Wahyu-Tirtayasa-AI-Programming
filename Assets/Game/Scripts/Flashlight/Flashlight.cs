using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private PlayerCharacter _owner;
    [SerializeField] private float _initialBatteryLevel = 100;
    [SerializeField] private float _batteryDrainRate = 1;
    private float _batteryLevel;
    public bool HasFlashlight => _owner.Inventory.CheckItem("Flashlight_001");
    public bool HasBattery => _batteryLevel > 0;
    private void Start()
    {
        _batteryLevel = _initialBatteryLevel;
        HUDManager.Instance.BatteryLevelUI.UpdateBatteryUI(_batteryLevel, _initialBatteryLevel);
    }
    public void UseFlashlight()
    {
        if (HasFlashlight == true && _light != null)
        {
            if (HasBattery == true)
            {
                _light.enabled = !_light.enabled;
            }
            else
            {
                _light.enabled = false;
            }
        }
    }
    private void Awake()
    {
        _batteryLevel = _initialBatteryLevel;
    }
    private void Update()
    {
        UpdateFlashlightRotation();
        UpdateBatteryLevel();
    }
    private void UpdateBatteryLevel()
    {
        if (_light != null && _light.enabled == true)
        {
            if (HasBattery == true)
            {
                _batteryLevel = _batteryLevel - _batteryDrainRate * Time.deltaTime;
            }
            else
            {
                _batteryLevel = 0;
                _light.enabled = false;
            }
            HUDManager.Instance.BatteryLevelUI.UpdateBatteryUI(_batteryLevel, _initialBatteryLevel);
        }
    }
    private void UpdateFlashlightRotation()
    {
        _light.transform.rotation = Camera.main.transform.rotation;
    }
    public void RefillBatteryLevel()
    {
        _batteryLevel = _initialBatteryLevel;
        HUDManager.Instance.BatteryLevelUI.UpdateBatteryUI(_batteryLevel, _initialBatteryLevel);
    }
}
