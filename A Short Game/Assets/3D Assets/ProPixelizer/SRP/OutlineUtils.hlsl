// Copyright Elliot Bentine, 2018-
//
// Utility functions for outlining.
#ifndef OUTLINE_UTILS_INCLUDED
#define OUTLINE_UTILS_INCLUDED

TEXTURE2D(_ProPixelizerOutlines);
SAMPLER(sampler_ProPixelizerOutlines);

/// <summary>
/// Gets the value from the outline buffer.
/// </summary>
inline void GetOutline_float(float2 texel, out float IDOutline, out float EdgeOutline) {
	float4 outlineResult = SAMPLE_TEXTURE2D(_ProPixelizerOutlines, sampler_ProPixelizerOutlines, texel);
	IDOutline = outlineResult.r;
	EdgeOutline = outlineResult.b;
}

#endif