﻿using System.Collections;
using UnityEngine;

namespace Waffle.Revenant.States
{
    public class RandomMeleeState : PassiveState
    {
        public bool AttackDone = false;
        public bool Bounced = false;
        private Coroutine CurrentAttack;

        public RandomMeleeState(Revenant revenant) : base(revenant)
        {
        }

        public override void Begin()
        {
            Revenant.StartCoroutine(DoStuff());
        }

        public IEnumerator DoStuff()
        {
            JumpscareCanvas.Instance.FlashImage(Revenant);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));

            float mult = 10;
            Vector3 addedDirection = new();

            switch (Random.Range(0, 3))
            {
                case 0:
                    addedDirection = NewMovement.Instance.transform.forward * mult;
                    break;
                case 1:
                    addedDirection = -NewMovement.Instance.transform.forward * mult;
                    break;
                case 2:
                    addedDirection = -NewMovement.Instance.transform.right * mult;
                    break;
                case 3:
                    addedDirection = NewMovement.Instance.transform.right * mult;
                    break;
            }

            Revenant.transform.position = NewMovement.Instance.transform.position + addedDirection;
            Revenant.InstantLookAtPlayer();
            IEnumerator chosenAttack = null;

            switch (Random.Range(0, 3))
            {
                case 0:
                    chosenAttack = Revenant.Melee1();
                    break;

                case 1:
                    chosenAttack = Revenant.Melee2();
                    break;

                case 2:
                    chosenAttack = Revenant.Melee3();
                    break;
            }

            Revenant.LookAtPlayer = true;

            CurrentAttack = Revenant.StartCoroutine(chosenAttack);
            yield return CurrentAttack;

            End();
        }

        public override void End()
        {
            Revenant.StopCoroutine(CurrentAttack);
            base.End();
        }

        public override void Update()
        {
            if (AttackDone)
            {
                base.Update();
            }

            if (Physics.Raycast(Revenant.transform.position, Revenant.transform.forward, out RaycastHit hit, 2f, LayerMaskDefaults.Get(LMD.Environment)) && Revenant.ForwardBoost > 0)
            {
                Reflect(hit.normal);
            }
        }

        public void Reflect(Vector3 normal)
        {
            Debug.Log($"Reflecting, normal {normal}");
            Revenant.transform.forward = Vector3.Reflect(Revenant.transform.forward, normal);
            //Revenant.transform.rotation *= Quaternion.Euler(-Revenant.transform.up * -22.5f);
            Revenant.TargetRotation = Revenant.transform.rotation;
            ((RandomMeleeState)Revenant.RevState).Bounced = true;
        }
    }
}
