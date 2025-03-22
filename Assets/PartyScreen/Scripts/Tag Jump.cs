using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagJump : MonoBehaviour
{
    public float jumpAmount;

    public float jumpSpeed;
    // Start is called before the first frame update
   public void Awake()
    {
       transform.LeanMoveLocal(new UnityEngine.Vector2(0, jumpAmount), jumpSpeed).setEaseOutQuart().setLoopPingPong(); // 1 is for animation duration
       // use ease out quart curve,set loop as ping pong
       // use the LeanTween library -> Lerps for us

    }
  
}
