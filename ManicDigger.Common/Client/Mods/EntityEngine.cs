
public class EntityEngine : EntityScript
{
    internal float constGravity;
    internal float constWaterGravityMultiplier;
    internal bool constEnableAcceleration;
    internal float constJump;
    
    internal bool isEntityonground;

    internal float movedz;
    internal float jumpacceleration;
    internal Acceleration acceleration;
    internal float jumpstartacceleration;
    internal float jumpstartaccelerationhalf;
    internal float movespeednow;
    internal Vector3Ref curspeed;

    public EntityEngine()
    {
        constGravity = 0.3f;
        constWaterGravityMultiplier = 3;
        constEnableAcceleration = true;
        constJump = 2.1f;
        isEntityonground = false;

        movedz = 0;
        jumpacceleration = 0;
        acceleration = new Acceleration();
        jumpstartacceleration = 0;
        jumpstartaccelerationhalf = 0;
        movespeednow = 0;
        curspeed = new Vector3Ref();

    }



}

