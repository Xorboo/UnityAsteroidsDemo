using System;


public class KeyboardUI : CanvasUIBase
{
    public override MovementData GetMovementData()
    {
        throw new Exception("Keyboard UI can only give hints");
    }

    public override bool IsShooting()
    {
        throw new Exception("Keyboard UI can only give hints");
    }
}
