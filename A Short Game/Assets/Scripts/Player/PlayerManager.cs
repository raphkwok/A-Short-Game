using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public enum State { GameStart, Active, Cutscene }
    public static PlayerManager player;

    [Header("References")]
    public Animator pAnim;
    public GameObject fRod;
    private Animator anim;
    private PlayerMovement pm;
    private PlayerRaycast pr;
    private PlayerRotation prot;
    private CharacterController cc;
    private Movement m;


    public Transform playerRot;

    // Camera
    public CinemachineVirtualCamera animCamera;

    [Header("State")]
    public State state;
    public Vector2 input;
    public float modelRot;

    [Header("Model Rotation Settings")]
    public float rotationSpeed;


    //////////////// Unity Functions ///////////////
    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        pr = GetComponent<PlayerRaycast>();
        prot = GetComponent<PlayerRotation>();
        cc = GetComponent<CharacterController>();
        m = GetComponent<Movement>();
        anim = GetComponent<Animator>();

        player = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.GameStart;

        modelRot = playerRot.eulerAngles.y;
    }


    // Update is called once per frame
    void Update()
    {
        if (state == State.GameStart && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            NextState();
        }

        input = m.moveInput;
        playerRot.transform.position = transform.position;
    }

    //////////////// States ///////////////

    public void NextState()
    {
        string methodName = state.ToString() + "State";

        // Get method
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null)); // Call the next state
    }

    IEnumerator GameStartState()
    {
        state = State.Active;
        pAnim.Play("Getting Up");
        anim.Play("Getting Up");

        GameObject.Find("Title").GetComponent<Animator>().Play("Title Out");

        yield return new WaitForSeconds(0.5f);
        anim.enabled = false;
        yield return new WaitForSeconds(0.5f);
        pr.enabled = true;
        prot.enabled = true;
        m.enabled = true;
        cc.enabled = true;

        animCamera.Priority = 1;
        NextState();
    }

    IEnumerator ActiveState()
    {
        while (state == State.Active)
        {
            pAnim.SetBool("isMoving", input != Vector2.zero);

            if (input != Vector2.zero)
            {
                // float rot = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

                // playerRot.eulerAngles =
                //     new Vector3(playerRot.eulerAngles.x,
                //                 Mathf.Lerp(playerRot.eulerAngles.y, rot, rotationSpeed),
                //                 playerRot.eulerAngles.z);

                float directionAngle = 0;

                if (Mathf.Abs(input.x) > 0 && input.y == 0)
                {
                    directionAngle = 90 * Mathf.Sign(input.x);
                }
                else if (input.x == 0 && input.y == -1)
                {
                    directionAngle = 180;
                }
                else if (Mathf.Abs(input.x) > 0 && input.y > 0)
                {
                    directionAngle = 45f * Mathf.Sign(input.x);
                }
                else if (Mathf.Abs(input.x) > 0 && input.y < 0)
                {
                    directionAngle = 135f * Mathf.Sign(input.x);
                }

                modelRot = transform.rotation.eulerAngles.y + directionAngle;
                playerRot.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(playerRot.eulerAngles.y, modelRot, rotationSpeed), transform.eulerAngles.z);
                // playerRot.rotation = Quaternion.Lerp(playerRot.rotation, transform.rotation, rotationSpeed);
            }
            else
            {
            }

            yield return null;
        }


    }
}
