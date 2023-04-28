using System;

public interface ICutScene
{
    public void ProcessCutScene(Action before = null, Action after = null);
}
