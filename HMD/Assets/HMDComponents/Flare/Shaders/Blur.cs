using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(BlurRenderer), PostProcessEvent.AfterStack, "Hidden/Custom/Blur")]
public sealed class Blur : PostProcessEffectSettings
{

}
public sealed class BlurRenderer : PostProcessEffectRenderer<Blur>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Blur"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}