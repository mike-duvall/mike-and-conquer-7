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

	if( (mapTileVisibilityColor.r == sentinelR) && (mapTileVisibilityColor.g == sentinelG) && (mapTileVisibilityColor.b == sentinelB) ) {
	// if( mapTileVisibilityColor.a == 0.0f ) {		
		if(color.a) {
			int numPaletteEntries = 256.0f;
			float paletteIndex = (color.r * 256.0f) / numPaletteEntries;
			float2 paletteCoordinates = float2(paletteIndex, 0.5f);

		    float4 paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
		    return paletteColor;
		}

		return color;

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