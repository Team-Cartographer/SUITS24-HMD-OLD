using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GrayscaleRenderer), PostProcessEvent.AfterStack, "Hidden/Custom/Greyscale")]
public sealed class Grayscale : PostProcessEffectSettings
{

}
public sealed class GrayscaleRenderer : PostProcessEffectRenderer<Grayscale>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Greyscale"));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}