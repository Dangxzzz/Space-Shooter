using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Variables

    public Color HighLightLabelColor;
    public TextMeshProUGUI StartGameButtonLabel;
    private Color StartLabelColor;

    #endregion

    #region Unity lifecycle

    private void Awake()
    {
        StartLabelColor = StartGameButtonLabel.color;
    }

    #endregion

    #region IPointerEnterHandler

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartGameButtonLabel.color = HighLightLabelColor;
    }

    #endregion

    #region IPointerExitHandler

    public void OnPointerExit(PointerEventData eventData)
    {
        StartGameButtonLabel.color = StartLabelColor;
    }

    #endregion
}