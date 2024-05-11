using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ThresholdRenderer), PostProcessEvent.AfterStack, "Hidden/Custom/Threshold")]
public sealed class Threshold : PostProcessEffectSettings
{

}
public sealed class ThresholdRenderer : PostProcessEffectRenderer<Threshold>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Threshold"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}