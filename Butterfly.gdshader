shader_type canvas_item;

uniform float flapSpeed = 4.0;
uniform float flapStrength = 0.2;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 uv = UV;
	float t = sin(TIME * flapSpeed);
	
	float k = t * flapStrength;
	uv.x = -k * abs(uv.y - 0.5) + uv.x;
	
	float y = uv.y * 2.0 - 1.0;
	y *= (1.0 - (min(-t, 0.4)) * 3.5 + 1.5) * 1.0;
	uv.y = y * 0.5 + 0.5;

	COLOR = texture(TEXTURE, uv);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
