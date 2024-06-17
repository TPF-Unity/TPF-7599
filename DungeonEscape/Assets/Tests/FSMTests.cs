using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FSMTests
{
    public class TrueCondition : Condition
    {
        public override bool ConditionIsMet(FSM fsm)
        {
            return true;
        }
    }

    public class FalseCondition : Condition
    {
        public override bool ConditionIsMet(FSM fsm)
        {
            return false;
        }
    }


    [UnityTest]
    public IEnumerator StateUpdatesWhenConditionIsTrue()
    {
        GameObject fsmObject = new GameObject();
        FSM fsm = fsmObject.AddComponent<FSM>();

        State state1 = ScriptableObject.CreateInstance<State>();
        State state2 = ScriptableObject.CreateInstance<State>();

        TrueCondition trueCondition = ScriptableObject.CreateInstance<TrueCondition>();

        Transition transition = Transition.Create(state2, trueCondition);
        state1.AddTransition(transition);

        fsm.CurrentState = state1;

        yield return null;

        Assert.AreEqual(state2, fsm.CurrentState);
    }

    [UnityTest]
    public IEnumerator StateDoesNotUpdateWhenConditionIsFalse()
    {
        GameObject fsmObject = new GameObject();
        FSM fsm = fsmObject.AddComponent<FSM>();

        State state1 = ScriptableObject.CreateInstance<State>();
        State state2 = ScriptableObject.CreateInstance<State>();

        FalseCondition falseCondition = ScriptableObject.CreateInstance<FalseCondition>();

        Transition transition = Transition.Create(state2, falseCondition);
        state1.AddTransition(transition);

        fsm.CurrentState = state1;

        yield return null;

        Assert.AreEqual(state1, fsm.CurrentState);
    }

    [Test]
    public void PlayerInRangeReturnsTrueWhenPlayerIsClose()
    {
        GameObject fsmObject = new GameObject();
        FSM fsm = fsmObject.AddComponent<FSM>();
        fsmObject.transform.position = Vector3.zero;

        GameObject playerObject = new GameObject();
        playerObject.transform.position = new Vector3(6f, 0f, 0f);
        playerObject.layer = LayerMask.NameToLayer("Player");
        SphereCollider collider = playerObject.AddComponent<SphereCollider>();
        collider.radius = 0.5f;

        PlayerInRangeCondition condition = PlayerInRangeCondition.Create(5f, LayerMask.GetMask("Player"), true);
        bool conditionMet = condition.ConditionIsMet(fsm);

        Assert.IsFalse(conditionMet, "Condition should not be met initially");

        fsmObject.transform.position = new Vector3(2f, 0f, 0f);
        conditionMet = condition.ConditionIsMet(fsm);

        Assert.IsTrue(conditionMet, "Condition should be met when player is within range");
    }
}