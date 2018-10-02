using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformBase : MonoBehaviour {

    protected Vector2 velocity = Vector2.zero;

    public virtual Vector2 GetSpeed()
    {
        return velocity;
    }
}
