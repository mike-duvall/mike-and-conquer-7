#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    

};


Texture2D PaletteTexture;
sampler2D PaletteTextureSampler = sampler_state
{
    Texture = <PaletteTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D MapTileVisibilityTexture;
sampler2D MapTileVisibilityTextureSampler = sampler_state
{
    Texture = <MapTileVisibilityTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D Value13MrfTexture;
sampler2D Value13MrfTextureSampler = sampler_state
{
    Texture = <Value13MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D Value14MrfTexture;
sampler2D Value14MrfTextureSampler = sampler_state
{
    Texture = <Value14MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};

Texture2D Value15MrfTexture;
sampler2D Value15MrfTextureSampler = sampler_state
{
    Texture = <Value15MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};

Texture2D Value16MrfTexture;
sampler2D Value16MrfTextureSampler = sampler_state
{
    Texture = <Value16MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};




struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 mapTileVisibilityColor = tex2D(MapTileVisibilityTextureSampler, input.TextureCoordinates);	

	float sentinelR = 255.0f / 255.0f;
	float sentinelG = 254.0f / 255.0f;
	float sentinelB = 253.0f / 255.0f;

	float rValueFor12 = 12.0f / 255.0f;
	float rValueFor13 = 13.0f / 255.0f;
	float rValueFor14 = 14.0f / 255.0f;
	float rValueFor15 = 15.0f / 255.0f;
	float rValueFor16 = 16.0f / 255.0f;

	float rValueFor1 = 1.0f / 255.0f;
	float rValueFor2 = 2.0f / 255.0f;
	float rValueFor3 = 3.0f / 255.0f;


	if( (mapTileVisibilityColor.r == sentinelR) && (mapTileVisibilityColor.g == sentinelG) && (mapTileVisibilityColor.b == sentinelB) ) {

		// Pickup here code:
		// Need to pass in 13,14,15,16 mrf mapperTextures
		// Then, if mapTileVisibilityColor.r = 13, map using 13 mrfmapper, 
		// same for 14,15,16
		// for 12, map it to black,
		// else, do code below

		if(color.a) {
			int numPaletteEntries = 256.0f;
			float paletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);


		    float4 paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
		}

		return color;

	}
	else if((mapTileVisibilityColor.r == rValueFor1) && (mapTileVisibilityColor.g == rValueFor2) && (mapTileVisibilityColor.b == rValueFor3)) {
		// return float4 (0,0,0,1);

		// Pickup here, this code does the "opposite" in some way
		// need to somehow mark black ones as maybe 17 ? and then map them?
		// Consider putting sentinel values in partiallyVisibleMask texture for then it's black
		// and check for that here, and then do code below

			int numPaletteEntries = 256.0f;
			float paletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);
		    float4 paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
	}
	else if(mapTileVisibilityColor.r == rValueFor13) {
			int numPaletteEntries = 256.0f;
			float mrfPaletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 mrfPaletteCoordinates = float2(mrfPaletteIndex, 0.5f);
		    float4 mrfColor = tex2D(Value13MrfTextureSampler, mrfPaletteCoordinates);

		    float paletteIndex = (mrfColor.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);		    

		    float paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
	}
	else if(mapTileVisibilityColor.r == rValueFor14) {
			int numPaletteEntries = 256.0f;
			float mrfPaletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 mrfPaletteCoordinates = float2(mrfPaletteIndex, 0.5f);
		    float4 mrfColor = tex2D(Value14MrfTextureSampler, mrfPaletteCoordinates);

		    float paletteIndex = (mrfColor.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);		    

		    float paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
	}
	else if(mapTileVisibilityColor.r == rValueFor15) {
			int numPaletteEntries = 256.0f;
			float mrfPaletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 mrfPaletteCoordinates = float2(mrfPaletteIndex, 0.5f);
		    float4 mrfColor = tex2D(Value15MrfTextureSampler, mrfPaletteCoordinates);

		    float paletteIndex = (mrfColor.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);		    

		    float paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
	}
	else if(mapTileVisibilityColor.r == rValueFor16) {
			int numPaletteEntries = 256.0f;
			float mrfPaletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 mrfPaletteCoordinates = float2(mrfPaletteIndex, 0.5f);
		    float4 mrfColor = tex2D(Value16MrfTextureSampler, mrfPaletteCoordinates);

		    float paletteIndex = (mrfColor.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);		    

		    float paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
	}

	else {
		return float4(0,0,0,1);			
	}

}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};