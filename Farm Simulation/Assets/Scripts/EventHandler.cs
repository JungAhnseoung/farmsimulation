public delegate void ActionDelegate(float xInput, float yInput, bool isIdle, bool isWalking, bool isRunning, bool isHolding,
    bool idleRight, bool idleLeft, bool idleUp, bool idelDown,
    bool isToolRight, bool isToolLeft, bool isToolUp, bool isToolDown,
    bool isPullRight, bool isPullLeft, bool isPullUp, bool isPullDown,
    bool isWaterRight, bool isWaterLeft, bool isWaterUp, bool isWaterDown,
    bool isReapRight, bool isReapLeft, bool isReapUp, bool isReapDown);

public static class EventHandler
{
    public static event ActionDelegate ActionEvent;

    public static void CallActionEvent(float xInput, float yInput, bool isIdle, bool isWalking, bool isRunning, bool isHolding,
    bool idleRight, bool idleLeft, bool idleUp, bool idleDown,
    bool isToolRight, bool isToolLeft, bool isToolUp, bool isToolDown,
    bool isPullRight, bool isPullLeft, bool isPullUp, bool isPullDown,
    bool isWaterRight, bool isWaterLeft, bool isWaterUp, bool isWaterDown,
    bool isReapRight, bool isReapLeft, bool isReapUp, bool isReapDown)
    {
        if (ActionEvent != null)
        {
            ActionEvent(xInput, yInput, isIdle, isWalking, isRunning, isHolding,
            idleRight, idleLeft, idleUp, idleDown,
            isToolRight, isToolLeft, isToolUp, isToolDown,
            isPullRight, isPullLeft, isPullUp, isPullDown,
            isWaterRight, isWaterLeft, isWaterUp, isWaterDown,
            isReapRight, isReapLeft, isReapUp, isReapDown);
        }
    }
}