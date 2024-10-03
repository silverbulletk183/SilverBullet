using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class StateMachine: MonoBehaviour
{
    public IState CurrentState {  get; private set; }
    public virtual void SetCurrentState(IState state)
    {
        CurrentState = state;
        StartCoroutine(Play());
    }
    public void Run()
    {
        if (CurrentState != null)
        {

        }
    }

    Coroutine m_CurrentPlayCoroutine;
    bool m_PlayLock;


    IEnumerator Play()
    {
        if (!m_PlayLock)
        {
            m_PlayLock = true;

            CurrentState.Enter();

            //keep a ref to execute coroutine of the current state
            //to support stopping it later.
            m_CurrentPlayCoroutine = StartCoroutine(CurrentState.Execute());

            yield return m_CurrentPlayCoroutine;

            m_CurrentPlayCoroutine = null;
        }
    }

    void Skip()
    {
        if (CurrentState == null)
        {
            Debug.Log("nameof(CurrentState) is null!");
        }

        if (m_CurrentPlayCoroutine != null)
        {
            StopCoroutine(m_CurrentPlayCoroutine);

            CurrentState.Exit();
            m_CurrentPlayCoroutine = null;
            m_PlayLock = false;
        }
    }
}
