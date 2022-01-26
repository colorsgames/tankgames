using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Com.COLORSGAMES.TANKGAMES
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public Vehicles player;

        [SerializeField]
        private Transform touchPoint;

        public float radius;
        public float returnSpeed;

        Vector2 inputVector;
        Vector2 startPosition;

        bool activate;

        int joyTouchIndex;


        public void OnDrag(PointerEventData eventData)
        {
            if (activate)
            {
                Vector2 touchPos = eventData.position;

                inputVector = touchPos - startPosition;

                if (inputVector.magnitude < radius)
                {
                    touchPoint.position = Vector3.Lerp(touchPoint.position, touchPos, returnSpeed * Time.deltaTime);

                    inputVector /= radius;
                    player.SteerAngle = inputVector.x; //�������� �������� �������� ������
                                                       //�������� �������� ��� ���� ���������
                    if (inputVector.y > 0)
                    {
                        player.MotorForce = inputVector.magnitude;
                    }
                    else
                    {
                        player.MotorForce = -inputVector.magnitude;
                    }
                }
                else if (activate)
                {
                    Vector2 state = Vector2.ClampMagnitude(inputVector, radius);

                    touchPoint.position = Vector3.Lerp(touchPoint.position, state + startPosition, returnSpeed * Time.deltaTime);//�������� �� ���������� � ����������� �������

                    state /= radius;
                    player.SteerAngle = state.normalized.x;

                    if (inputVector.y > 0)
                    {
                        player.MotorForce = state.magnitude;
                    }
                    else
                    {
                        player.MotorForce = -state.magnitude;
                    }
                }
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            activate = true;
            joyTouchIndex = Input.touchCount - 1;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joyTouchIndex--;

            player.MotorForce = 0;
            player.SteerAngle = 0;

            activate = false;
        }

        private void Start()
        {
            startPosition = transform.position;
            player = GameObject.FindObjectOfType<Vehicles>();
        }

        private void Update()
        {
            if (!activate)
            {
                touchPoint.position = Vector3.Lerp(touchPoint.position, startPosition, returnSpeed * Time.deltaTime);
            }
        }
    }
}