using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Experiment_VibrationQuest : MonoBehaviour
{
    public float vibrationDuration = 5f; 
    public float vibrationIntensity = 1.0f;
    //[SerializeField] InputActionReference leftHapticAction;
    //[SerializeField] InputActionReference rightHapticAction;

     void Update()
    {
       

        // Check for input to trigger vibration
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            VibrateControllerXRInspectorValue();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Coroutine_AnimCurve());
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Coroutine_Scale());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Coroutine_Sin());
        }
    }

    public uint m_channel=0;
    public float m_amplitude=1;
    public float m_duration=1;

    public AnimationCurve m_curve;

    public float m_frameTime = 0.1f;

    public IEnumerator Coroutine_Scale()
    {

        float start = Time.time;
        float now = Time.time;
        float percent = 0;
        while (now - start < m_duration)
        {
            now = Time.time;
            percent = (now - start) / m_duration;
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_frameTime);
            VibrateControllerXR(percent, m_frameTime);
        }

    }
    public float m_sinFrequence=5;
    public IEnumerator Coroutine_Sin()
    {

        float start = Time.time;
        float now = Time.time;
        float percent = 0;
        while (now - start < m_duration)
        {
            now = Time.time;
            percent = (now - start) / m_duration;
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_frameTime);
            VibrateControllerXR(Mathf.Sin(percent/ m_sinFrequence), m_frameTime);
        }

    }
    public IEnumerator Coroutine_AnimCurve()
    {

        float start = Time.time;
        float now = Time.time;
        float percent = 0;
        while (now - start < m_duration)
        {
            now = Time.time;
            percent = (now - start) / m_duration;
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_frameTime);
            VibrateControllerXR(m_curve.Evaluate(percent), m_frameTime) ;
        }

    }
    private void VibrateControllerXRInspectorValue()
    {
        VibrateControllerXR(m_amplitude, m_duration);
    }
        private void VibrateControllerXR(float amplitude, float duration)
    {
        // Get the list of input devices
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevices(devices);
        m_devicesName = devices.Select(k => k.name).ToArray();

        foreach (var device in devices)
        {
            // Check if the device is a controller
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
            {
                // Check for vendor-specific extensions or functions to trigger vibration
                device.TryGetHapticCapabilities(out HapticCapabilities capabilities);
                device.SendHapticImpulse(m_channel, amplitude, duration);
            }
        }
    }

    public string[] m_devicesName;
    //private void VibrateControllerXRBuffer()
    //{
    //    List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
    //    InputDevices.GetDevices(devices);

    //    foreach (var device in devices)
    //    {
    //        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
    //        {
    //            device.TryGetHapticCapabilities(out HapticCapabilities capabilities);
    //            //uint max = capabilities.bufferMaxSize;
    //            uint hz = capabilities.bufferFrequencyHz;
    //            m_supportsBuffer = capabilities.supportsBuffer;
    //            m_buffer = GeneratePulseBuffer(m_amplitude, m_duration, hz);
    //            device.SendHapticBuffer(m_channel, m_buffer);
    //        }
    //    }
    //}
    //protected virtual byte[] GeneratePulseBuffer(float intensity, float duration,uint bufferFrequencyHz)
    //{
    //    int clipCount = (int)(bufferFrequencyHz * duration);
    //    byte[] clip = new byte[clipCount];
    //    for (int index = 0; index < clipCount; index++)
    //    {
    //        //clip[index] = (byte)(byte.MaxValue * intensity * m_curve.Evaluate(index / (float)clipCount));
    //        clip[index] = (byte)(byte.MaxValue * intensity * UnityEngine.Random.value);
    //    }
    //    return clip;

    //}
}

