using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IState
{
    void Enter();
    IEnumerator Execute();
    void Exit();

    //out keyword
    //As a parameter modifier, which lets you pass an argument to a method by reference rather than by value.

    //In generic type parameter declarations for interfaces and delegates, which specifies that a type parameter is covariant.
    bool ValidateLinks(out IState nextState);


    //These methods make all the links from this state to others active or inactive.
    void EnableLinks();
    void DisableLinks();
}
