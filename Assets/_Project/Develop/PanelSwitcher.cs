using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public bool Switch;
    public CanvasGroup From;
    public CanvasGroup To;
    private CanvasGroup _last;

    [SerializeField] private float _speed;
    public void SwitchPanels(CanvasGroup from, CanvasGroup to)
    {
        from.alpha -= _speed * Time.fixedDeltaTime;
        to.alpha += _speed * Time.fixedDeltaTime;

        if(from.alpha <= 0.05f)
        {
            from.alpha = 0f;
            to.alpha = 1f;
            Switch = false;

            _last = To;
            From = _last;
        }

        if (to.blocksRaycasts == true) return;
        else
        {
            from.interactable = false;
            from.blocksRaycasts = false;

            to.interactable = true;
            to.blocksRaycasts = true;
        }
    }
    public void StartSwitching(CanvasGroup to)
    {
        To = to;
        Switch = true;
    }
    private void FixedUpdate()
    {
        if (Switch) SwitchPanels(From, To);   
    }
    public void Welcome()
    {
        Switch = true;
    }
}
