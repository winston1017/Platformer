using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class JoystickControl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image bgImg;
    private Image joystickImg;
    public Vector2 inputVector;


    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform
                       , ped.position
                       , ped.pressEventCamera
                       , out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);

            inputVector = new Vector2(pos.x * 2 + 1, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystickImg.rectTransform.anchoredPosition =
             new Vector2(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3)
              , inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));

            //Debug.Log(inputVector.x);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }


    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;
        joystickImg.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            Debug.Log("Horizontal IF");
            return inputVector.x;
        }
            
        else
        {
            Debug.Log("Horizontal E");
            return Input.GetAxis("Horizontal");
        }
    }
}