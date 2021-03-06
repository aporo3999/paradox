﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using SiliconStudio.Paradox.Effects.ShadowMaps;
using SiliconStudio.Paradox.Engine;
using SiliconStudio.Paradox.Graphics;

namespace SiliconStudio.Paradox.Effects.Processors
{
    /// <summary>
    /// This class inherits from <see cref="LightShadowProcessorWithBudget"/> and has a budget of two 2048 x 2048 shadow map textures: one for the variance shadow maps, one for the other types.
    /// </summary>
    public class LightShadowProcessorDefaultBudget : LightShadowProcessorWithBudget
    {
        public LightShadowProcessorDefaultBudget(GraphicsDevice device, bool manageShadows)
            : base(device, manageShadows)
        {
            // Fixed budget of textures
            AddShadowMapTexture(new ShadowMapTexture(GraphicsDevice, ShadowMapFilterType.Nearest, 2048), ShadowMapFilterType.Nearest);
            AddShadowMapTexture(new ShadowMapTexture(GraphicsDevice, ShadowMapFilterType.Variance, 2048), ShadowMapFilterType.Variance);
        }
    }
}
