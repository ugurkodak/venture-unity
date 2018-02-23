using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    CityModel city;
    public Camera cityCamera;


    void Start()
    {
        city = new CityModel(4, 4);

        for (int x = 0; x < city.Width; x++)
        {
            for (int z = 0; z < city.Height; z++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tile.transform.position = new Vector3(x, 0, z);
                tile.transform.Rotate(new Vector3(90, 0, 0));
				
				//REMOVE IT!!!
				// if(x == 0 && z == 0)
				// {
				// 	Mesh mesh = tile.GetComponent<MeshFilter>().mesh;
				// 	Vector3[] vertices = mesh.vertices;
				// 	Debug.Log(mesh.vertices[0]);
				// 	vertices[0] -= Vector3.forward /2;
				// 	mesh.vertices = vertices;
				// 	mesh.RecalculateBounds();				
				// 	Debug.Log(mesh.vertices[0]);
				// }
            }
        }
		
		SetCameraToDefaultPosition();
    }

    void Update()
    {

    }

	//TODO: Properly calculate to center camera.
	public void SetCameraToDefaultPosition()
	{
		float cameraSize, xOffset, yOffset,zOffset;
		if (city.Width > city.Height)
		{
			cameraSize = (float)(city.Width * 0.4f);
			xOffset = cameraSize * (city.Width - city.Height) * 0.01f;
			zOffset = -cameraSize * (city.Width - city.Height) * 0.01f;
			yOffset = 0;
		}
		else 
		{
			cameraSize = (float)(city.Height * 0.4f);
			xOffset = cameraSize * (city.Width - city.Height) * 0.01f;
			zOffset = -cameraSize * (city.Width - city.Height) * 0.01f;
			yOffset = 0;
		}
        cityCamera.orthographicSize = cameraSize;
		cityCamera.transform.rotation = Quaternion.Euler(30, 45, 0);
		cityCamera.transform.position = new Vector3(cameraSize * 0.1f + xOffset, cameraSize * 0.8f + yOffset, cameraSize * 0.1f + zOffset);
	}
}
