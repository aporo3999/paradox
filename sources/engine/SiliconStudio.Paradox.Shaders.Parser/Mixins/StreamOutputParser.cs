// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
#if PARADOX_EFFECT_COMPILER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using SiliconStudio.Shaders.Ast;
using SiliconStudio.Shaders.Ast.Hlsl;
using SiliconStudio.Paradox.Graphics;

namespace SiliconStudio.Paradox.Shaders.Parser.Mixins
{
    internal static class StreamOutputParser
    {
        private static Regex streamOutputRegex = new Regex(@"(([0-9]*)\s*:\s*)?(\w+)(.(\w+))?");
        private static readonly string[] masks = new[] { "xyzw", "rgba", "stuv" };

        public static void Parse(IList<ShaderStreamOutputDeclarationEntry> entries, out int[] strides, AttributeDeclaration streamOutputAttribute, IList<Variable> fields)
        {
            var streamStrings = streamOutputAttribute.Parameters
                .TakeWhile(x => x.Value is string)
                .Select(x => x.Value as string)
                .ToArray();

            Parse(entries, out strides, streamStrings, fields);
        }
        
        /// <summary>
        /// Parse stream output declarations.
        /// Format is "[slot :] semantic[index][.mask] ; ...".
        /// </summary>
        /// <param name="entries">The parsed entries.</param>
        /// <param name="strides">The output strides.</param>
        /// <param name="streams">The output declarations to parse.</param>
        public static void Parse(IList<ShaderStreamOutputDeclarationEntry> entries, out int[] strides, string[] streams, IList<Variable> fields)
        {
            strides = new int[4];

            var fieldsBySemantic = fields.ToDictionary(x => Semantic.Parse(x.Qualifiers.OfType<Semantic>().Single().Name));

            for (int streamIndex = 0; streamIndex < streams.Length; ++streamIndex)
            {
                // Parse multiple declarations separated by semicolon
                var stream = streams[streamIndex];
                foreach (var streamOutput in stream.Split(';'))
                {
                    // Parse a single declaration: "[slot :] semantic[index][.mask]"
                    var match = streamOutputRegex.Match(streamOutput);
                    if (!match.Success)
                        throw new InvalidOperationException("Could not parse stream output.");

                    var streamOutputDecl = new ShaderStreamOutputDeclarationEntry();

                    // Split semantic into (name, index)
                    var semantic = Semantic.Parse(match.Groups[3].Value);

                    streamOutputDecl.SemanticName = semantic.Key;
                    streamOutputDecl.SemanticIndex = semantic.Value;
                    //if (streamOutputDecl.SemanticName == "$SKIP")
                    //    streamOutputDecl.SemanticName = null;

                    var matchingField = fieldsBySemantic[semantic];
                    var matchingFieldType = matchingField.Type.TypeInference.TargetType ?? matchingField.Type;

                    if (matchingFieldType is VectorType)
                        streamOutputDecl.ComponentCount = (byte)((VectorType)matchingFieldType).Dimension;
                    else if (matchingFieldType is ScalarType)
                        streamOutputDecl.ComponentCount = 1;
                    else
                        throw new InvalidOperationException(string.Format("Could not recognize type of stream output for {0}.", matchingField));
                        
                    var mask = match.Groups[5].Value;
                    ParseMask(mask, ref streamOutputDecl.StartComponent, ref streamOutputDecl.ComponentCount);

                    byte.TryParse(match.Groups[2].Value, out streamOutputDecl.OutputSlot);

                    streamOutputDecl.Stream = streamIndex;

                    strides[streamOutputDecl.OutputSlot] += streamOutputDecl.ComponentCount * sizeof(float);
                    entries.Add(streamOutputDecl);
                }
            }
        }

        private static bool ParseMask(string mask, ref byte startComponent, ref byte componentCount)
        {
            if (mask == string.Empty)
            {
                return false;
            }

            foreach (var maskRef in masks)
            {
                var index = maskRef.IndexOf(mask);
                if (index != -1)
                {
                    componentCount = (byte)mask.Length;
                    startComponent = (byte)index;
                    return true;
                }
            }

            throw new InvalidOperationException("Could not parse stream output mask.");
        }
    }
}
#endif