using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    enum Action
    {
        Idle,
        GoTowardsScrap,
        ReturnToBase
    }

    [SerializeField] private float movementSpeed = 1f;

    private bool carryingItem = false;
    private Action currentAction = Action.Idle;
}
