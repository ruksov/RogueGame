using System;
using System.Collections;
using Rogue.Input;
using UnityEngine;
using VContainer;

namespace Rogue.Hero.Components
{
  public class HeroRoll : MonoBehaviour
  {
    public float Speed;
    public float Distance;
    public float Cooldown;

    public HeroMove HeroMove;
    public Rigidbody2D Rigidbody;

    public Action IsRollingChanged;
    public bool IsRolling
    {
      get => m_isRolling;
      private set
      {
        if (value == m_isRolling)
          return;

        m_isRolling = value;
        IsRollingChanged?.Invoke();
      }
    }

    private InputService m_inputService;
    private float m_nextRollMinTime;
    private bool m_isRolling;
    private bool m_requestEndRoll;

    [Inject]
    private void Construct(InputService inputService)
    {
      m_inputService = inputService;
      m_inputService.RollPerformed += Roll;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (IsRolling)
        m_requestEndRoll = true;
    }

    private void Roll()
    {
      if (!CanStartRoll())
        return;

      StartCoroutine(RollCoroutine());
    }

    private IEnumerator RollCoroutine()
    {
      StartRoll();

      Vector2 rollDirection = m_inputService.MoveInput;
      Vector3 startRollPosition = transform.position;

      while (CanRolling(startRollPosition))
      {
        Rigidbody.velocity = rollDirection * Speed;

        yield return new WaitForFixedUpdate();
      }

      EndRoll();
    }

    private void StartRoll()
    {
      IsRolling = true;
      HeroMove.enabled = false;
      m_requestEndRoll = false;
    }

    private void EndRoll()
    {
      IsRolling = false;
      HeroMove.enabled = true;
      m_nextRollMinTime = Time.time + Cooldown;
    }

    private bool CanStartRoll()
    {
      return !IsRolling && m_nextRollMinTime < Time.time && CheckMoveDirection();
    }

    private bool CanRolling(Vector3 startRollPosition) => 
      !m_requestEndRoll && 
      Distance > Vector3.Distance(startRollPosition, transform.position);

    private bool CheckMoveDirection()
    {
      return m_inputService.MoveInput == Vector2.up ||
             m_inputService.MoveInput == Vector2.down ||
             m_inputService.MoveInput == Vector2.left ||
             m_inputService.MoveInput == Vector2.right;

    }
  }
}