﻿// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Paradox Shader Mixin Code Generator.
// To generate it yourself, please install SiliconStudio.Paradox.VisualStudio.Package .vsix
// and re-save the associated .pdxfx.
// </auto-generated>

using System;
using SiliconStudio.Core;
using SiliconStudio.Paradox.Effects;
using SiliconStudio.Paradox.Graphics;
using SiliconStudio.Paradox.Shaders;
using SiliconStudio.Core.Mathematics;
using Buffer = SiliconStudio.Paradox.Graphics.Buffer;

using SiliconStudio.Paradox.Effects.Data;
using SiliconStudio.Paradox.Shaders.Compiler;
namespace SiliconStudio.Paradox.Effects
{
    internal static partial class ShaderMixins
    {
        internal partial class BasicEffect  : IShaderMixinBuilder
        {
            public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
            {
                context.Mixin(mixin, "ShaderBase");
                context.Mixin(mixin, "TransformationWAndVP");
                context.Mixin(mixin, "PositionVSStream");
                var hasNormals = context.GetParam(MaterialParameters.NormalMap) != null;
                if (hasNormals)
                {
                    context.Mixin(mixin, "NormalMapTexture");

                    {
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                        context.PushComposition(mixin, "normalMap", __subMixin);
                        context.Mixin(__subMixin, context.GetParam(MaterialParameters.NormalMap));
                        context.PopComposition();
                    }
                }
                else
                {
                    context.Mixin(mixin, "NormalVSStream");
                }
                context.Mixin(mixin, "BRDFDiffuseBase");
                context.Mixin(mixin, "BRDFSpecularBase");
                context.Mixin(mixin, "LightMultiDirectionalShadingPerPixel", 2);
                context.Mixin(mixin, "TransparentShading");
                context.Mixin(mixin, "DiscardTransparent");
                if (context.GetParam(MaterialParameters.AlbedoDiffuse) != null)
                {

                    {
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                        context.PushComposition(mixin, "DiffuseLighting", __subMixin);
                        context.Mixin(__subMixin, "ComputeBRDFDiffuseLambert");
                        context.PopComposition();
                    }

                    {
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                        context.PushComposition(mixin, "albedoDiffuse", __subMixin);
                        context.Mixin(__subMixin, context.GetParam(MaterialParameters.AlbedoDiffuse));
                        context.PopComposition();
                    }
                }
                if (context.GetParam(MaterialParameters.AlbedoSpecular) != null)
                {

                    {
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                        context.PushComposition(mixin, "SpecularLighting", __subMixin);
                        context.Mixin(__subMixin, "ComputeBRDFColorSpecularBlinnPhong");
                        context.PopComposition();
                    }

                    {
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                        context.PushComposition(mixin, "albedoSpecular", __subMixin);
                        context.Mixin(__subMixin, context.GetParam(MaterialParameters.AlbedoSpecular));
                        context.PopComposition();
                    }
                    if (context.GetParam(MaterialParameters.SpecularPowerMap) != null)
                    {
                        context.Mixin(mixin, "SpecularPower");

                        {
                            var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                            context.PushComposition(mixin, "SpecularPowerMap", __subMixin);
                            context.Mixin(__subMixin, context.GetParam(MaterialParameters.SpecularPowerMap));
                            context.PopComposition();
                        }
                    }
                    if (context.GetParam(MaterialParameters.SpecularIntensityMap) != null)
                    {

                        {
                            var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };
                            context.PushComposition(mixin, "SpecularIntensityMap", __subMixin);
                            context.Mixin(__subMixin, context.GetParam(MaterialParameters.SpecularIntensityMap));
                            context.PopComposition();
                        }
                    }
                }
                if (context.GetParam(MaterialParameters.HasSkinningPosition))
                {
                    if (context.GetParam(MaterialParameters.SkinningBones) > context.GetParam(MaterialParameters.SkinningMaxBones))
                    {
                        context.SetParam(MaterialParameters.SkinningMaxBones, context.GetParam(MaterialParameters.SkinningBones));
                    }
                    mixin.Mixin.AddMacro("SkinningMaxBones", context.GetParam(MaterialParameters.SkinningMaxBones));
                    context.Mixin(mixin, "TransformationSkinning");
                    if (context.GetParam(MaterialParameters.HasSkinningNormal))
                    {
                        if (hasNormals)
                            context.Mixin(mixin, "TangentToViewSkinning");
                        else
                            context.Mixin(mixin, "NormalVSSkinning");
                        context.Mixin(mixin, "NormalSkinning");
                    }
                    if (context.GetParam(MaterialParameters.HasSkinningTangent))
                        context.Mixin(mixin, "TangentSkinning");
                }
            }

            [ModuleInitializer]
            internal static void __Initialize__()

            {
                ShaderMixinManager.Register("BasicEffect", new BasicEffect());
            }
        }
    }
}
