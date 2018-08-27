using UnityEngine;
using PlayerStatus;

    public class BoatControll : MonoBehaviour
    {
      
        //the boat's current power
        public float propellerPower;

        //boats acceleration
        public float acceleration;

        //the boat's maximum engine power
        public float maxPower;

        //the boat's maximum reverse power
        public float maxReversePower;

        public Transform PropellerTransform;

        private Rigidbody boatRB;

        private float PropellerRotation;

        private void Start()
        {
            boatRB = GetComponent<Rigidbody>();
        }
        void Update()
        {
            UserInput();
        }
        void UserInput()
        {
            //canot drive boat if in first person
            if (Controll.IsFirstPerson == false)
            {
                //Drive backwards
                if (Input.GetKey(KeyCode.S))
                {
                    if (currentSpeed() > -50f && propellerPower > -maxReversePower)
                    {
                        propellerPower -= acceleration * Time.deltaTime;
                    }
                }
                //Drive Forward
                else if (Input.GetKey(KeyCode.W))
                {
                    if (currentSpeed() < 50f && propellerPower < maxPower)
                    {
                        propellerPower += acceleration * Time.deltaTime * 2;
                    }
                }
                else
                {
                    propellerPower = 0f;
                }

                //AddForce in Current Direction
                Vector3 forceToAdd = PropellerTransform.forward * propellerPower;

                boatRB.AddForceAtPosition(forceToAdd, PropellerTransform.position);

                //Steer left
                if (Input.GetKey(KeyCode.A))
                {
                    PropellerRotation = PropellerTransform.localEulerAngles.y + 2f;

                    if (PropellerRotation > 10f && PropellerRotation < 270f)
                    {
                        PropellerRotation = 10f;
                    }

                    Vector3 newRotation = new Vector3(0f, PropellerRotation, 0f);

                    PropellerTransform.localEulerAngles = newRotation;
                }
                //Steer right
                else if (Input.GetKey(KeyCode.D))
                {
                    PropellerRotation = PropellerTransform.localEulerAngles.y - 2f;

                    if (PropellerRotation < 350f && PropellerRotation > 90f)
                    {
                        PropellerRotation = 350f;
                    }

                    Vector3 newRotation = new Vector3(0f, PropellerRotation, 0f);

                    PropellerTransform.localEulerAngles = newRotation;
                }
                else
                {
                    // return propellar to original rotation(so it goes forward)
                    PropellerTransform.localEulerAngles = new Vector3(0f, 0f, 0f); ;
                }
            }
        }
        private Vector3 lastPosition;

        private float currentSpeed()
        {
            float CalculateSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

            lastPosition = transform.position;

            return CalculateSpeed;
        }
    }
