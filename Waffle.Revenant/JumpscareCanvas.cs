﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Waffle.Revenant
{
    public class JumpscareCanvas : MonoBehaviour
    {
        public static JumpscareCanvas Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(RevenantAssets.Instance.TemplateJumpscareCanvas).GetComponent<JumpscareCanvas>();
                }

                return _instance;
            }
        }

        private static JumpscareCanvas _instance;

        public Image Image;
        public AudioSource GlitchSound;
        public Vector2 PitchRange;
        private Coroutine _lastFlash;

        public void FlashImage(Revenant rev)
        {
            if (_lastFlash != null)
            {
                StopCoroutine(_lastFlash);
            }

            _lastFlash = StartCoroutine(FlashImageRoutine(rev));
        }

        private IEnumerator FlashImageRoutine(Revenant rev)
        {
            float alpha = Mathf.Clamp((1 - Vector3.Distance(rev.transform.position, NewMovement.Instance.transform.position) / 100), 0.1f, 1);

            Image.color = new Color(1, 1, 1, alpha);
            Image.gameObject.SetActive(true);
            Image.sprite = rev.GetJumpscare();

            GlitchSound.pitch = UnityEngine.Random.Range(PitchRange.x, PitchRange.y);
            GlitchSound.Play();

            /*
            while (Image.color.a != 0)
            {
                Image.color = new Color(1, 1, 1, Mathf.MoveTowards(Image.color.a, 0, Time.deltaTime * 5));
                yield return null;
            }
            */

            yield return new WaitForSeconds(0.2f);

            int rand = UnityEngine.Random.Range(0, 3);

            if (rand == 1 && !rev.Enraged)
            {
                StartCoroutine(FlashImageRoutine(rev));
            }

            yield return new WaitForSeconds(0.05f);

            Image.gameObject.SetActive(false);
            GlitchSound.Stop();
        }
    }
}
