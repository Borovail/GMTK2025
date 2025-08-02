using UnityEngine;

public class ShakerTutorial : ITutorialStep
{
    private Shaker _shaker;
    public ShakerTutorial(Shaker shaker)
    {
        _shaker = shaker;
    }

    public void StartStep()
    {
        PlayerUI.Instance.Text(true, "Click on Shaker");
    }

    public void EndStep()
    {
        PlayerUI.Instance.Text(false);
    }

    public bool IsStepCompleted()
    {
        return _shaker.Shaked;
    }
}