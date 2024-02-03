using UnityEngine;

// This script should be attached to a GameObject that has a SpriteRenderer component with the city view image set as the sprite.
public class TitleScreenBackground : MonoBehaviour
{
    public float panSpeed = 0.5f; // Speed at which the camera pans
    public float zoomSpeed = 0.5f; // Speed at which the camera zooms
    public Vector2 panLimit; // Limits to camera panning
    public float minZoom, maxZoom; // Zoom limits
    private Vector3 startPosition;
    private float startZoom;

    void Start()
    {
        startPosition = transform.position; // Store the starting position
        startZoom = Camera.main.orthographicSize; // Store the starting zoom level
    }

    void Update()
    {
        // Calculate the new position and zoom based on time, speed, and limits
        float newX = Mathf.PingPong(Time.time * panSpeed, panLimit.x * 2) - panLimit.x;
        float newZoom = Mathf.PingPong(Time.time * zoomSpeed, maxZoom - minZoom) + minZoom;

        // Apply the new position and zoom to the camera
        transform.position = new Vector3(startPosition.x + newX, startPosition.y, startPosition.z);
        Camera.main.orthographicSize = newZoom;
    }
}
