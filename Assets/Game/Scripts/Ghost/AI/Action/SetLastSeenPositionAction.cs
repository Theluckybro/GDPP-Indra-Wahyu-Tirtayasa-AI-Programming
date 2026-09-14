using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetLastSeenPosition", story: "Set [LastSeenPosition] from [AI]", category: "Action", id: "616f719c6cedb8572581fb0df37cdb22")]
public partial class SetLastSeenPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> LastSeenPosition;
    [SerializeReference] public BlackboardVariable<GhostAIController> AI;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (AI.Value == null || AI.Value.SightPerception == null)
        {
            return Status.Failure;
        }
        if (AI.Value.SightPerception.CanSeePlayer)
        {
            LastSeenPosition.Value = AI.Value.Target.transform.position;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

