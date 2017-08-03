using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;            // The position that that camera will be following.
        public float smoothing = 10f;        // The speed with which the camera will be following.
		public float zoomOutSize = 32f;
		public float zoomInSize = 13f;
		public float zoomSpeed = 15f;

        Vector3 offset;                     // The initial offset from the target.

		private Camera fieldCamera;

        void Start ()
        {
            // Calculate the initial offset.
            offset = transform.position - target.position;
			fieldCamera = GetComponent<Camera>();
        }


        void FixedUpdate ()
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = target.position + offset;
			//bool walking = transform.position != target.position;

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		}

		public void zoomIn(bool walking)
		{
			if(walking){
				if(fieldCamera.fieldOfView < zoomOutSize){
					fieldCamera.fieldOfView = fieldCamera.fieldOfView + zoomSpeed * Time.deltaTime;
				}
			}
			else{
				if(fieldCamera.fieldOfView > zoomInSize){
					fieldCamera.fieldOfView = fieldCamera.fieldOfView - zoomSpeed * Time.deltaTime;
				}
			}
        }
    }
}