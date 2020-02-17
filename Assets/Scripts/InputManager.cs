using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public Command() { }
    public virtual void Execute(GameObject obj) { }
}


public class InputManager : Command
{
    public void HandleInput() 
    {
        //if (Input.GetKeyDown())
    }
}

public class JumpCommand : Command
{
    public override void Execute(GameObject obj)
    {
        //obj.jump();
    }
}


