using UnityEngine;

public class BlueFood : Food
{
    protected override void InitializeFood()
    {
        nutritionalValue = 20; // Set nutritional value for BlueFood
        foodColor = Color.blue;
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        Color slimeColor = Color.blue;
        slimeColor.a = 0.647f;
        slime.ChangeColor(slimeColor);
    }
}