using System;
using GameplayAssembly.HealthSystem;
using UnityEngine;

namespace Plugins.procedural_healthbar_shader.HealthBar.Components
{
    [ExecuteInEditMode]
    public class HealthBar : MonoBehaviour
    {
        private static readonly int FillColor = Shader.PropertyToID("_fillColor");

        enum ShapeType
        {
            Circle, Box, Rhombus
        };

        [SerializeField]  private HealthProvider _healthProvider;
        [SerializeField] ShapeType _shape;
        [SerializeField, Range(0,1)] float _healthNormalized;
        [SerializeField, Range(0,1)] float _lowHealthThreshold;

        [Header("Fill")]
        [SerializeField] Gradient _lowToHighHealthTransition;

        [Header("Wave")]
        [SerializeField, Range(0,0.1f)] float _fillWaveAmplitude;
        [SerializeField, Range(0,100f)] float _fillWaveFrequency;
        [SerializeField, Range(0, 1f)] float _fillWaveSpeed;

        [Header("Background")]
        [SerializeField] Color _backgroundColor;

        [Header("Border")]
        [SerializeField, Range(0, 0.15f)] float _borderWidth;
        [SerializeField] Color _borderColor;

        Material _matInstance;

        /// <summary>
        /// Health value between 0 and 1
        /// </summary>
        public float HealthNormalized => _healthProvider.healthNormalized;

        private float _oldHealthNormalized;
        private void Update()
        {
            if (Time.frameCount % 2 == 0 && !Mathf.Approximately(_oldHealthNormalized, _healthProvider.healthNormalized))
            {
                SetMaterialData();
                _oldHealthNormalized = HealthNormalized;
            }
        }

        void Start()
        {
            SetupUniqueMaterial();
            SetMaterialData();
        }

        void SetupUniqueMaterial()
        {
            if (_matInstance != null) return;

            Debug.Log("Setup Material", this.gameObject);
            _matInstance = new Material(Shader.Find("CustomShaders/HealthBar"));
            if (Application.isPlaying)
            {
                GetComponent<Renderer>().material = _matInstance;
            }
            else
            {
                GetComponent<Renderer>().sharedMaterial = _matInstance;
            }
        }

        void SetMaterialData()
        {
            if (_matInstance == null) return;

            if (Application.isPlaying)
                _matInstance.SetFloat("_healthNormalized", _healthProvider.healthNormalized);
            else
                _matInstance.SetFloat("_healthNormalized", _healthNormalized);

            SetKeyword();

            _matInstance.SetFloat("_lowLifeThreshold", _lowHealthThreshold);

            _matInstance.SetFloat("_waveAmp", _fillWaveAmplitude);
            _matInstance.SetFloat("_waveFreq", _fillWaveFrequency);
            _matInstance.SetFloat("_waveSpeed", _fillWaveSpeed);

            if (Application.isPlaying)
                _matInstance.SetColor(FillColor, _lowToHighHealthTransition.Evaluate(_healthProvider.healthNormalized));
            else
                _matInstance.SetColor(FillColor, _lowToHighHealthTransition.Evaluate(_healthNormalized));
           

            _matInstance.SetColor("_backgroundColor", _backgroundColor);
            _matInstance.SetFloat("_borderWidth", _borderWidth);
            _matInstance.SetColor("_borderColor", _borderColor);
        }

        void SetKeyword()
        {
            foreach (var kword in _matInstance.shaderKeywords)
            {
                _matInstance.DisableKeyword(kword);
            }
            if (_shape == ShapeType.Circle) _matInstance.EnableKeyword("_SHAPE_CIRCLE");
            else if (_shape == ShapeType.Box) _matInstance.EnableKeyword("_SHAPE_BOX");
            else if (_shape == ShapeType.Rhombus) _matInstance.EnableKeyword("_SHAPE_RHOMBUS");

            //Sync shader keywordEnum
            _matInstance.SetInt("_shape", (int)_shape);
        }

        void OnValidate()
        {
            SetMaterialData();
        }

        void OnDestroy()
        {
            if (_matInstance != null)
            {
                if (Application.isPlaying)
                    Destroy(_matInstance);
                else
                    DestroyImmediate(_matInstance);
            }
        }
    }
}
