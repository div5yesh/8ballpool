using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Exp = UnityEngine.Experimental.UIElements;

namespace CPG
{
    public enum PlayerAction
    {
        SHOOT,
        ROTATE,
		MOVECUEBALL,
		PLACECUEBALL
    }

    public class PlayerControls : MonoBehaviour
    {

        public delegate void PlayerInputCallback(PlayerAction action, float deg);
        public event PlayerInputCallback OnPlayerInput;

		public delegate void FoulCallback(PlayerAction action, Vector3 deg);
		public event FoulCallback OnFoulInput;

        public bool isLocalPlayer = false;

		public bool bInputDown = false;

        // Use this for initialization
        void Start()
        {
        }

        public void SetupLocalPlayer()
        {
            isLocalPlayer = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

			RotateCue ();
            
			BallInHand ();
        }

		public void RotateCue()
		{
			float rotation = 0;

			if (Application.platform == RuntimePlatform.Android)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Moved)
				{
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

		public void BallInHand()
		{
			Vector3 dest;
			RaycastHit hit;

			if (Application.platform == RuntimePlatform.Android)
			{
				if (Input.touchCount == 1) {
					Touch touch = Input.GetTouch (0);

					if (touch.phase == TouchPhase.Began) {
						Ray ray = Camera.main.ScreenPointToRay (touch.position);
						if (Physics.Raycast (ray, out hit, 100)) {
							if (hit.collider.gameObject.tag == "CueBall") {
								bInputDown = true;
							}
						}
					}

					if (touch.phase == TouchPhase.Canceled) {
						bInputDown = false;
					}

					if (bInputDown) {
						dest = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, Camera.main.nearClipPlane));
						OnFoulInput (PlayerAction.MOVECUEBALL, dest);
					}
				} else {
					bInputDown = false;
				}
			}
			else
			{
				if (Input.GetMouseButtonDown (0)) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 100)) {
						if (hit.collider.gameObject.tag == "CueBall") {
							bInputDown = true;
						}
					}
				}

				if (Input.GetMouseButtonUp (0)) {
					bInputDown = false;
				}

				if (bInputDown) {
					
					dest = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y));
					OnFoulInput(PlayerAction.MOVECUEBALL, dest);
				}
			}
		}

        public void OnShoot()
        {
            if (!isLocalPlayer)
            {
                return;
            }

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
