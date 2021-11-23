using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolutionaryDungeon
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        private readonly float panSpeed = 0.3f;
        private Vector3 initialPosition;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            initialPosition = cam.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cam.transform.position = initialPosition;
            }

            UpdateCameraMovement();
        }

        /// <summary>
        /// Pans and zooms the camera.
        /// </summary>
        private void UpdateCameraMovement()
        {
            // Update screen movement pan using keys
            float horizontalMovement = Input.GetAxisRaw("Horizontal") * panSpeed;
            Vector3 newPos = cam.transform.position;
            newPos.x += horizontalMovement;
            cam.transform.position = newPos;
        }

    }
}