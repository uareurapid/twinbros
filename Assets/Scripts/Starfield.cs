using UnityEngine;
using UnityEngine.Assertions;

//http://guidohenkel.com/2018/05/endless_starfield_unity/
public class Starfield : MonoBehaviour
{
	public int MaxStars = 100;
	public float StarSize = 0.1f;
	public float StarSizeRange = 0.5f;
	public float FieldWidth = 20f;
	public float FieldHeight = 25f;
	public bool Colorize = false;

	ParticleSystem Particles;
	ParticleSystem.Particle[] Stars;

	public float ParallaxFactor = 0f;

	private float xOffset = 0;
	private float yOffset = 0;

	void Awake()
	{
		Stars = new ParticleSystem.Particle[MaxStars];
		Particles = GetComponent<ParticleSystem>();

		Assert.IsNotNull(Particles, "Particle system missing from object!");

		xOffset = FieldWidth * 0.5f;                                                                                                        // Offset the coordinates to distribute the spread
		yOffset = FieldHeight * 0.5f;                                                                                                       // around the object's center

		for (int i = 0; i < MaxStars; i++)
		{
			float randSize = Random.Range(StarSizeRange, StarSizeRange + 0.16f) / 2f;                       // Randomize star size within parameters
			float scaledColor = (true == Colorize) ? randSize - StarSizeRange : 1f;         // If coloration is desired, color based on size

			Stars[i].position = GetRandomInRectangle(FieldWidth, FieldHeight) + transform.position;
			Stars[i].startSize = (StarSize * randSize) / 2f;
			Stars[i].startColor = new Color(1f, scaledColor, scaledColor, 1f);
		}
		Particles.SetParticles(Stars, Stars.Length); // Write data to the particle system

		//GetComponent<ParticleSortingLayer>().enabled = true;
	}


	// GetRandomInRectangle
	//----------------------------------------------------------
	// Get a random value within a certain rectangle area
	//
	Vector3 GetRandomInRectangle(float width, float height)
	{
		float x = Random.Range(0, width);
		float y = Random.Range(0, height);
		return new Vector3(x - xOffset, y - yOffset, 0);
	}


	void Update() {

		for ( int i=0; i < MaxStars; i++ )
		{
			Vector3 pos = Stars[ i ].position + transform.position;

			if ( pos.x < ( Camera.main.gameObject.transform.position.x - xOffset ) )
			{
				pos.x += FieldWidth;
			}
			else if ( pos.x > ( Camera.main.gameObject.transform.position.x + xOffset ) )
			{
				pos.x -= FieldWidth;
			}

			if ( pos.y < ( Camera.main.gameObject.transform.position.y - yOffset ) )
			{
				pos.y += FieldHeight;
			}
			else if ( pos.y > ( Camera.main.gameObject.transform.position.y + yOffset ) )
			{
				pos.y -= FieldHeight;
			}

			Stars[ i ].position = pos - transform.position;
		}
		Particles.SetParticles( Stars, Stars.Length );

		Vector3 newPos = Camera.main.gameObject.transform.position * ParallaxFactor;					// Calculate the position of the object
		newPos.z = 0;						// Force Z-axis to zero, since we're in 2D
		transform.position = newPos;
	}
}


