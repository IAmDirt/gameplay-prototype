using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStatus
{
    public class Controll
    {
        public static bool IsFirstPerson;

        //boat is tied up at the dock
        public bool docked;

        void FirstPerson()
        {
            IsFirstPerson = true;
        }
    }
}
