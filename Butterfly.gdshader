shader_type canvas_item;

uniform float flapSpeed = 4.0;
uniform float flapStrength = 0.2;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 uv = UV;
	
	float t = (sin(TIME * flapSpeed)) * flapStrength;
	uv.y = t * abs(uv.x - 0.5) + uv.y;

	COLOR = texture(TEXTURE, uv);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
