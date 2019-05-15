using UnityEngine;
using System.Collections;

public class ParticleSortingLayer : MonoBehaviour {

    public string sortingLayer = "Foreground";
    public int sortingOrder = -1;
	public bool isParticle = false;
   
    void Start ()
    {

		if(!isParticle) {
			Renderer rend = GetComponent<Renderer>();
			if (rend != null)
			{
				rend.sortingLayerName = sortingLayer;
				rend.sortingOrder = sortingOrder;
			}
		}
		else {
			// Set the sorting layer of the particle system.
			ParticleSystem system = GetComponent<ParticleSystem>();
			if (system != null)
			{
				system.GetComponent<Renderer>().sortingLayerName = sortingLayer;
				system.GetComponent<Renderer>().sortingOrder = sortingOrder;
			}
			else {
				ParticleRenderer rend = GetComponent<ParticleRenderer>();
				if (rend != null)
				{
					rend.sortingLayerName = sortingLayer;
					rend.sortingOrder = sortingOrder;
				}
			}
		}

        
    }
}
