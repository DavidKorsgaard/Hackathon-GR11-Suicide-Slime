using UnityEngine;

public class Food : MonoBehaviour
{
    public virtual void ChangeSlimeColor(Slime slime) { }
}

public class RedFood : Food
{
    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.red);
    }
}

public class BlueFood : Food
{
    public override void ChangeSlimeColor(Slime slime)
    {
        slime.ChangeColor(Color.blue);
    }
}

// Add more food classes as needed
