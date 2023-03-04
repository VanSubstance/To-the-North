using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourControllByKey : MonoBehaviour, IControllByKey
{
    public abstract void ControllByKey(int purpose);
}
