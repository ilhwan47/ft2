using UnityEngine;

namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum State
        {
            Stop = 0,
            Walk = 1,
            Run = 2,
        }

        public float moveSpeed = 5f;            // The speed that the player will move at.
        public float runSpeed = 10f;            // The speed that the player will run at.
        public float accelerateTime = 2f;       // 가속 시간
        public float decelerateTime = 2f;       // 감속 시간
        public Animator anim;                      // Reference to the animator component.
        public JoystickHandler joystickHandler;

        private Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
        private float stopMagnitude;
        private float walkMagnitude;
        private Vector3 movement;                   // The vector to store the direction of the player's movement.
        private Vector3 turnDirection;
        private State state; // 상태
        private float speed; // 이동 속도
        private bool accelerate; // 가속 여부
        private bool decelerate; // 감속 여부
        private float currAccelerateTime; // 현재 가속 시간
        private float currDecelerateTime; // 현재 감속 시간

        void Awake ()
        {
            // Set up references.
            // anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();

            stopMagnitude = joystickHandler.stopMagnitude;
            walkMagnitude = joystickHandler.walkMagnitude;

            state = State.Stop;
            accelerate = false;
            decelerate = false;
        }


        void FixedUpdate ()
        {
            float magnitude = joystickHandler.InputDirection.magnitude;
            float h = joystickHandler.InputDirection.x;
            float v = joystickHandler.InputDirection.y;
            State stateOld = state;

            // 상태 설정
            if (magnitude > walkMagnitude)
            {
                state = State.Run;
            }
            else if (magnitude > stopMagnitude)
            {
                state = State.Walk;
            }
            else
            {
                state = State.Stop;
            }

            // 상태 변화 체크
            if (state != stateOld)
            {
                accelerate = false;
                decelerate = false;
                
                switch (state)
                {
                    case State.Stop:
                        speed = 0;
                        break;

                    case State.Walk:
                        if (stateOld == State.Run)
                        {
                            decelerate = true;
                            currDecelerateTime = 0;
                        }
                        else
                        {
                            speed = moveSpeed;
                        }
                        break;

                    case State.Run:
                        if (stateOld == State.Walk)
                        {
                            accelerate = true;
                            currAccelerateTime = 0;
                        }
                        else
                        {
                            speed = runSpeed;
                        }
                        break;
                }
            }
            
            // 상태별 처리
            switch (state)
            {
                case State.Stop:
                    Turning(h, v);
                    Animationg(0);
                    break;

                case State.Walk:
                    if (decelerate)
                    {
                        currDecelerateTime += Time.deltaTime;
                        speed -= (runSpeed - moveSpeed) * (currDecelerateTime / accelerateTime);
                        if (speed <= moveSpeed)
                        {
                            speed = moveSpeed;
                            decelerate = false;
                            currDecelerateTime = 0f;
                        }
                    }
                    Move(h, v, speed);
                    Turning(h, v);
                    Animationg(1);
                    break;

                case State.Run:
                    if (accelerate)
                    {
                        currAccelerateTime += Time.deltaTime;
                        speed += (runSpeed - moveSpeed) * (currAccelerateTime / decelerateTime);
                        if (speed >= runSpeed)
                        {
                            speed = runSpeed;
                            accelerate = false;
                            currAccelerateTime = 0f;
                        }
                    }
                    Move(h, v, speed);
                    Turning(h, v);
                    Animationg(2);
                    break;
            }
        }


        void Move (float h, float v, float speed)
        {
            //Debug.Log("====> state : " + state + ", speed : " + speed);

            // Set the movement vector based on the axis input.
            movement.Set (h, 0f, v);
            
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition (transform.position + movement);
        }


        void Turning (float h, float v)
        {
            turnDirection.Set(h, 0, v);
            
            if (turnDirection != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDirection) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }
        }


        void Animationg(int index)
        {
            anim.SetInteger("Walking", index); // 0:멈춤, 1:걷기, 2:달리기
        }
    }
}