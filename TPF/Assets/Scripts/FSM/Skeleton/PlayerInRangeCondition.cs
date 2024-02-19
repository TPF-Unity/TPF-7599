using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerInRangeCondition : Condition
{
    private float range;
    private LayerMask whatIsPlayer;
    private bool returnTrueIfInRange = true;

    public static PlayerInRangeCondition Create(float range, LayerMask whatIsPlayer, bool returnTrueIfInRange = true)
    {
        PlayerInRangeCondition condition = CreateInstance<PlayerInRangeCondition>();
        condition.range = range;
        condition.whatIsPlayer = whatIsPlayer;
        condition.returnTrueIfInRange = returnTrueIfInRange;
        return condition;
    }

    public override bool ConditionIsMet(FSM fsm)
    {
        bool playerInRange = Physics.CheckSphere(fsm.transform.position, range, whatIsPlayer);
        return returnTrueIfInRange ? playerInRange : !playerInRange;
    }
}
