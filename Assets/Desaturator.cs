using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game
{
    [Serializable]
    [PostProcess(typeof(DesaturatorRenderer), PostProcessEvent.BeforeStack, "Custom/Desaturator")]
    public class Desaturator : PostProcessEffectSettings
    {
        [Range(0, 5)]
        public FloatParameter OverSaturation = new FloatParameter();
    }

    public sealed class DesaturatorRenderer : PostProcessEffectRenderer<Desaturator>
    {
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Custom/Desaturator"));
            sheet.properties.SetFloat("_OverSaturation", settings.OverSaturation);
            sheet.properties.SetFloatArray("_SaturationPoints", DesaturatorRuntime.Instance == null ? new float[360] : DesaturatorRuntime.Instance.SaturationArray);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
