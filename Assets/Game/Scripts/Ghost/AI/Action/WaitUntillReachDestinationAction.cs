using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait Untill Reach Destination", story: "[AI] wait untill reach destination", category: "Action", id: "d44baf64d2e97ef46c4d46daf3d006cd")]
public partial class WaitUntillReachDestinationAction : Action
{
    [SerializeReference] public BlackboardVariable<GhostAIController> AI;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (AI.Value == null)
        {
            return Status.Failure;
        }
        NavMeshAgent agent = AI.Value.NavMeshAgent;
        if (agent == null)
        {
            return Status.Failure;
        }
        if (agent.pathPending == true)
        {
            return Status.Running;
        }
        if (agent.remainingDistance > agent.stoppingDistance + 0.5)
        {
            return Status.Running;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

