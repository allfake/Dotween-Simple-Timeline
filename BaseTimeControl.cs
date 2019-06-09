using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Timeline;

namespace BashiBashi
{
    public class BaseTimeControl : MonoBehaviour, ITimeControl
    {
        public Sequence m_Sequence = null;
	
        public void OnControlTimeStart ()
        {
        }

        public void OnControlTimeStop ()
        {
        }

        void OnValidate()
        {
            SetupAction();
        }

        public void SetTime (double _time)
        {
            if(m_Sequence == null)
            {
                return;
            }
            m_Sequence.Goto((float)_time);
        }

        protected void InitSequence()
        {
            if(m_Sequence != null)
            {
                m_Sequence.Goto(0);
                m_Sequence.Kill();
            }
            m_Sequence = DOTween.Sequence();
        }

        protected virtual void SetupAction()
        {
            
        }
    }
}
