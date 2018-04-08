using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_cam : MonoBehaviour 
{
	[Header("MOVE: WASD")]
	[Header("SPRINT: SHIFT")]
	[Header("ZOOM: CLICK 1")]
	[Space(15)]
	public bool invertAim = false; // Ivert our aiming system
	float mouseiXoY; // Store mouse position (input X output Y)
	float mouseiYoX; // Store mouse position (input Y output X)

	void Cam_() 
	{
		Vector3 mousePos = Input.mousePosition; // Create a Vector3 variable to store mouse position

		mouseiXoY += Input.GetAxis ("Mouse X"); // Store Y axis

		if (invertAim == true) // Invert aiming
		{
			mouseiYoX += Input.GetAxis ("Mouse Y"); // Store X axis
			mouseiYoX = Mathf.Clamp (mouseiYoX, -30, 30); // Clamping X rotation
		} 
		else if (invertAim == false)
		{
			mouseiYoX -= Input.GetAxis ("Mouse Y"); // Store X axis
			mouseiYoX = Mathf.Clamp (mouseiYoX, -30, 30); // Clamping X rotation
		}

		transform.eulerAngles = new Vector3 (mouseiYoX, mouseiXoY, 0);
	}

	void Zoom()
	{
		int nFov = 60; // Camera normal field of view
		int zFov = 30; // Camera zooming field of view
		int smooth = 5; // Lerp interpolate value (smooth)

		if (Input.GetMouseButton (1)) // Right click to zooming
		{
			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, zFov, (smooth * Time.fixedDeltaTime)); // Camera current FOV lerp in 
		} 
		else
		{
			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, nFov, (smooth * Time.fixedDeltaTime)); // Camera zoom FOV lerp out
		}
	}

	void LateUpdate()
	{
		Cam_ ();
		Zoom ();
	}

}
