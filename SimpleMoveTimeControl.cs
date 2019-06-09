using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using DG.Tweening;

namespace BashiBashi
{
    public class SimpleMoveTimeControl : BaseTimeControl, ITimeControl
    {
        public Vector3 ToMovePosision;
        public Transform[] ToMoveTransform = null;
        public Transform LookAtTransform;
        public bool LookAhead;
        [Range(0.1f, 10.0f)]
        public float ToMoveDuration = 1.0f;
        public DG.Tweening.Ease EasingType = Ease.Linear;

        private Vector3 startPosition = Vector3.zero;

        private List<Vector3> m_Path = new List<Vector3>();

        protected override void SetupAction()
        {
            startPosition = transform.position;
            InitSequence();
        }

        void ITimeControl.OnControlTimeStart()
        {
            if (ToMoveTransform != null && ToMoveTransform.Length != 0)
            {
                m_Path.Clear();
                
                for(int i = 0; i < ToMoveTransform.Length; i++)
                {
                    m_Path.Add(ToMoveTransform[i].position);
                }

                if (LookAhead)
                {
                    m_Sequence = DOTween.Sequence()
                        .Append(gameObject.transform.DOPath(m_Path.ToArray(), ToMoveDuration).SetLookAt(0.01f));
                }
                else
                {
                    m_Sequence = DOTween.Sequence()
                        .Append(gameObject.transform.DOPath(m_Path.ToArray(), ToMoveDuration).SetLookAt(LookAtTransform));
                }

                m_Sequence.SetRecyclable();
                m_Sequence.Play();
                
            }
            else 
            {
                m_Sequence = DOTween.Sequence()
                    .Append(transform.DOLocalMove(ToMovePosision, ToMoveDuration).SetEase(EasingType))
                    .SetRecyclable()
                    .Play();
            }
        }

        void ITimeControl.OnControlTimeStop()
        {
            if (transform != null)
            {
                transform.position = startPosition;
                m_Sequence.Kill();
            }
            
            if (Application.isPlaying == false)
            {
                m_Sequence.Complete(true);
            }
        }

        void ITimeControl.SetTime(double time)
        {
            if(Application.isPlaying == false)
            {
                m_Sequence.Goto((float)time);
            }
        }
    }
}
