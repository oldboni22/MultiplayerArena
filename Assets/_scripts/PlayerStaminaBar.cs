using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


namespace BonBon
{
    public class PlayerStaminaBar : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Slider _slider;
        private float _valueTarget;
                
        public void SetMax(float value)
        {
            _slider.maxValue = value;
        }
        public void UpdateTarget(float value)
        {
            _valueTarget = value;
        }
        void Update()
        {
            HandleSlider();
        }

        private void HandleSlider()
        {
            if (_valueTarget != _slider.value)
            {
                _slider.value = Mathf.Lerp(_slider.value, _valueTarget,_speed * Time.deltaTime);
            }
        }
    }
}