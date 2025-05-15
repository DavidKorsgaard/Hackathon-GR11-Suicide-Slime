using UnityEngine;

public class RedFood : Food
{
    protected override void InitializeFood() //Enables individual nutritional values and colors for child class
    {
        nutritionalValue = 10; // Set nutritional value for RedFood
        foodColor = Color.red;
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        Color slimeColor = Color.red;
        slimeColor.a = 0.647f; // Keep the same alpha as in the original code
        slime.ChangeColor(slimeColor);
    }
}