using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
#if UNITY_ANDROID
using UnityEngine.XR.OpenXR.Features.Meta;
using System.Linq;
using Unity.Collections;
#endif

namespace KadenZombie8.BIMOS.Rig.Tempor
{
    public class AutoPhysicsRate : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(SetFixedTimeStep());
        }

        private static IEnumerator SetFixedTimeStep()
        {
            var refreshRate = 0f;
            while (refreshRate == 0f)
            {
                try
                {
                    var display = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();

#if UNITY_ANDROID && !UNITY_EDITOR
                    display.TryGetSupportedDisplayRefreshRates(Allocator.Temp, out var refreshRates);
                    if (display.TryRequestDisplayRefreshRate(refreshRates.Max()))
                        refreshRate = refreshRates.Max();
#else
                    display.TryGetDisplayRefreshRate(out refreshRate);
#endif
                }
                catch { }
                yield return null;
            }
            Time.fixedDeltaTime = 1f / refreshRate;
        }
    }
}
