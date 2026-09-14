using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetCanSeeTarget", story: "Set [CanSeeTarget] from [AI]", category: "Action", id: "d6a6ac80911221684799e6e4787b96ac")]
public partial class SetCanSeeTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> CanSeeTarget;
    [SerializeReference] public BlackboardVariable<GhostAIController> AI;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (AI.Value == null && AI.Value.SightPerception == null)
        {
            return Status.Failure;
        }
        CanSeeTarget.Value = AI.Value.SightPerception.CanSeePlayer;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

