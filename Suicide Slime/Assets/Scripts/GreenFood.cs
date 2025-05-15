using UnityEngine;

public class GreenFood : Food
{
    protected override void InitializeFood() //Enables individual nutritional values and colors for child class
    {
        nutritionalValue = 15; // Set nutritional value for GreenFood
        foodColor = Color.green;
    }

    public override void ChangeSlimeColor(Slime slime)
    {
        Color slimeColor = Color.green;
        slimeColor.a = 0.647f;
        slime.ChangeColor(slimeColor);
    }
}