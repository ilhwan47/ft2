/**
 * http://www.theappguruz.com/blog/beginners-guide-learn-to-make-simple-virtual-joystick-in-unity
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public float stopMagnitude = 0.1f;
    public float walkMagnitude = 0.8f;

    [HideInInspector]
    public Vector3 InputDirection;

    private Image container;
    private Image joystick;

    void Awake()
    {
        container = transform.GetChild(0).GetComponent<Image>();
        joystick = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        InputDirection = Vector3.zero;

        container.enabled = false;
        joystick.enabled = false;
    }


    public void OnPointerUp(PointerEventData ped)
    {
        InputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
        container.enabled = false;
        joystick.enabled = false;
    }


    public void OnPointerDown(PointerEventData ped)
    {
        container.transform.position = ped.position;
        container.enabled = true;
        joystick.enabled = true;

        OnDrag(ped);
    }


    public void OnDrag(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;
        Vector2 positionTemp = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(container.rectTransform, ped.position, ped.pressEventCamera, out position);

        //If anchor is center
        positionTemp = position- joystick.rectTransform.anchoredPosition;
        position += container.rectTransform.sizeDelta * 0.5f;
        position.x = position.x / container.rectTransform.sizeDelta.x;
        position.y = position.y / container.rectTransform.sizeDelta.y;

        //float x = (container.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        //float y = (container.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        float x = position.x * 2 - 1;
        float y = position.y * 2 - 1;

        InputDirection = new Vector3(x, y, 0);
        //InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;
        if (InputDirection.magnitude > 1)
        {
            container.rectTransform.anchoredPosition += positionTemp;
            InputDirection = InputDirection.normalized;
        }
        //To define the area in which joystick can move around
        joystick.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (container.rectTransform.sizeDelta.x / 2)
            , InputDirection.y * (container.rectTransform.sizeDelta.y / 2));
    }
}
