using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ButtonJump : MonoBehaviour
{
    public void Awake()
    {
       transform.LeanMoveLocal(new UnityEngine.Vector2(0, 10), 1).setEaseOutQuart().setLoopPingPong(); // 1 is for animation duration
       // use ease out quart curve,set loop as ping pong
       // use the LeanTween library -> Lerps for us

    }
}
