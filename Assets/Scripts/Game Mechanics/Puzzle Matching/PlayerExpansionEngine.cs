using UnityEngine;

public class PlayerExpansionEngine : ExpansionEngine
{
    public Movement playerMovement;

    protected override void HandleNewBlock(Transform blockTransform)
    {
        var physics = blockTransform.GetComponent<BlockPhysics>();

        if (physics == null) return;

        playerMovement.AddGroundCheck(physics.groundCheck);
        physics.engine = this;
    }

    protected override void HandleRemovedBlock(Transform blockTransform)
    {
        var physics = blockTransform.GetComponent<BlockPhysics>();

        if(physics == null) return;

        playerMovement.RemoveGroundCheck(physics.groundCheck);
    }
}
