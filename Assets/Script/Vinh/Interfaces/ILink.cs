using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILink
{
    bool Validate(out IState nextState);
    void Enable() { }
    void Disable() { }
}
