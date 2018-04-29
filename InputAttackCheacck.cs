using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputAttack
{
    public class InputAttackCheacck : MonoBehaviour
    {
        public static bool attack;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator Attack()
        {
            attack = true;
            yield return new WaitForSeconds(0.01f);
            attack = false;
        }
    }
}