// Copyright Elliot Bentine, 2018-
// Helper functions for screen parameters.
// Some of these parameters have not yet been exposed as Shader Graph nodes, so this file is needed.

#ifndef SCREEN_UTIL_INCLUDED
#define SCREEN_UTIL_INCLUDED

void GetScaledScreenParameters_float(out float4 Out)
{
	#ifdef SHADERGRAPH_PREVIEW
		Out = float4(0, 0, 0, 0);
	#else
		Out = floor(_ScaledScreenParams);
	#endif
}
#endif
