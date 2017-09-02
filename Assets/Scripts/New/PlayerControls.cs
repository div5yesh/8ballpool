using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CPG
{
    public enum PlayerAction
    {
        SHOOT,
        ROTATE
    }

    public class PlayerControls : MonoBehaviour
    {

        public delegate void PlayerInputCallback(PlayerAction action, float deg);
        public event PlayerInputCallback OnPlayerInput;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

            float rotation = 0;

            if (Application.platform == RuntimePlatform.Android)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = touch.deltaPosition.ToString();
                    rotation = touch.deltaPosition.x * Time.deltaTime * 15;
                    OnPlayerInput(PlayerAction.ROTATE, rotation);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
                {
                    rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
                    OnPlayerInput(PlayerAction.ROTATE, rotation);
                }
            }
        }

        public void OnShoot()
        {
            float power = GetComponentInChildren<Slider>().value;
            OnPlayerInput(PlayerAction.SHOOT, power);
        }

        public void EnableControls()
        {
            gameObject.SetActive(true);
        }

        public void DisableControls()
        {
            gameObject.SetActive(false);
        }
    } 
}
