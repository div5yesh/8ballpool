using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{
    public float magnitude = 2f;
    public float timer = 30f;

    float speed = 40; // 70 to 5

    GameObject cueBall;
    float startTime;
    bool shooting = false;
    Vector3 initialPosition;

    Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

    LineRenderer shootline;

    // Use this for initialization
    void Start()
    {
        cueBall = GameObject.FindWithTag("CueBall");
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer && !GetComponent<NetworkPlayer>().isTurn)
        {
            return;
        }

        float rotation = 0;
        float power = 0;
        float force = 0;

        if (Application.platform == RuntimePlatform.Android)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved )
            {
                GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = touch.deltaPosition.ToString();
                rotation = touch.deltaPosition.x * Time.deltaTime * 20;
                CmdMoveCue(rotation);
            }
        }
        else
        {

            rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            power = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
            force = Vector3.Distance(transform.position, cueBall.transform.position);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            shooting = false;
            CmdMoveCue(rotation);

            //shootline.SetPosition(0, cueBall.transform.position);
            //shootline.SetPosition(1, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            shooting = false;
        }

        if (force <= 1.25)
        {
            speed = force * 20;
            transform.Translate(Math.Abs(power), 0, 0);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            startTime = Time.time;
            shooting = true;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            initialPosition = transform.position;
        }

            RecoilCue(force);
        //if (cueBall.GetComponent<Rigidbody>().IsSleeping())
        {
            //SpawnManager.Instance.isPlayerTurn = SpawnManager.Instance.isPlayerTurn == 0 ? 1 : 0;
        }
    }

    [Command]
    public void CmdMoveCue(float pos)
    {
        transform.RotateAround(cueBall.transform.position, ROTATION_AXIS, pos);
    }

    void RecoilCue(float force)
    {
        if (shooting)
        {
            float fracJourney = force / 2.50f;
            Vector3 newPosition = Vector3.Lerp(transform.position, initialPosition, fracJourney);
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (shooting)
        {
            if (collider.gameObject.tag == "CueBall")
            {
				CmdShoot ();
				shooting = false;
				GetComponent<NetworkPlayer> ().RpcTurnEnd ();
            }
        }
    }

	[Command]
	void CmdShoot(){
        float sliderValue = GameObject.FindGameObjectWithTag("PlayerControls").GetComponentInChildren<Slider>().value;
		cueBall.GetComponent<Rigidbody>().AddForce((cueBall.transform.position - transform.position).normalized * sliderValue, ForceMode.Impulse);
		cueBall.GetComponent<Rigidbody>().AddTorque(Vector3.zero);
        NetworkManager.Instance.Shooting = true;
	}
}