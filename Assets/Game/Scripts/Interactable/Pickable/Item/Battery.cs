using UnityEngine;

public class Battery : Item
{
    public override void Pickup(PlayerCharacter character)
    {
        base.Pickup(character);
        character.Flashlight.RefillBatteryLevel();
    }
}
