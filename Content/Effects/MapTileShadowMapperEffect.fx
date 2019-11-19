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
};

Texture2D ShadowTexture;
sampler2D ShadowTextureSampler = sampler_state
{
    Texture = <ShadowTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D UnitMrfTexture;
sampler2D UnitMrfTextureSampler = sampler_state
{
    Texture = <UnitMrfTexture>;
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

//                 for each pixel, if shadow rendertarget has no value, just return the maptile palette for color
//                 if shadow rendertarget DOES have a value, convert the maptile palette value to the shadow value, via 
//                 lookup in the mrf texture

	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 shadowColor = tex2D(ShadowTextureSampler, input.TextureCoordinates);
	float unitMrfColor = tex2D(UnitMrfTextureSampler, input.TextureCoordinates);

	if(shadowColor.r == 0) {
		return color;
	}
	else {
		int numPaletteEntries = 256.0f;
		float paletteIndex = (color.r * 256.0f) / numPaletteEntries;
		float2 paletteCoordinates = float2(paletteIndex, 0.5f);

	    float4 paletteColor = tex2D(UnitMrfTextureSampler, paletteCoordinates);
	    return paletteColor;

	}




}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};