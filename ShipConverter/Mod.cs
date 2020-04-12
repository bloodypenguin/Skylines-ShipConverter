using System;
using ColossalFramework.UI;
using ICities;
using UnityEngine;
using FerryConverter.OptionsFramework;
using FerryConverter.OptionsFramework.Extensions;

namespace FerryConverter
{
    public class Mod : IUserMod
    {
        public string Name => "Ship Converter";
        public string Description => "Converts regular ships into Mass Transit ferries and patches ship/boat buildings shaders for them to properly float on water";

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddOptionsGroup<Options>();
            try
            {
                OptionsWrapper<Config.Config>.Ensure();
            }
            catch (Exception e)
            {
                var display = new GameObject().AddComponent<ErrorMessageDisplay>();
                display.e = e;
            }
        }


        private class ErrorMessageDisplay : MonoBehaviour
        {
            public Exception e;

            public void Update()
            {
                var exceptionPanel = UIView.library?.ShowModal<ExceptionPanel>("ExceptionPanel");
                if (exceptionPanel == null)
                {
                    return;
                }
                exceptionPanel.SetMessage(
                "Malformed XML config",
                "There was an error reading Ferry Converter XML config:\n" + e.Message + "\n\nFalling back to default config...",
                true);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
