using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        private Rect bounds;
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void Start()
        {
            bounds = new Rect(
                RoomController.Instance.Left.position.x, RoomController.Instance.Bottom.position.y,
                RoomController.Instance.Width, RoomController.Instance.Height);
        }

        private Rect CamBoundsFromPosition(Vector2 position)
        {
            return new Rect(
                position.x - (cam.orthographicSize * cam.aspect),
                position.y - cam.orthographicSize,
                cam.orthographicSize * cam.aspect * 2,
                cam.orthographicSize * 2
            );
        }

        private void Update()
        {
            var characterPosition = PlayerController.Instance.gameObject.transform.position;
            var currentPosition = characterPosition;
            var camBounds = CamBoundsFromPosition(currentPosition);

            if (camBounds.xMin < bounds.xMin)
            {
                currentPosition += new Vector3(bounds.xMin - camBounds.xMin, 0, 0);
                camBounds = CamBoundsFromPosition(currentPosition);
            }

            if (camBounds.yMin < bounds.yMin)
            {
                currentPosition += new Vector3(0, bounds.yMin - camBounds.yMin, 0);
                camBounds = CamBoundsFromPosition(currentPosition);
            }

            if (camBounds.xMax > bounds.xMax)
            {
                currentPosition -= new Vector3(camBounds.xMax - bounds.xMax, 0, 0);
                camBounds = CamBoundsFromPosition(currentPosition);
            }

            if (camBounds.yMax > bounds.yMax)
            {
                currentPosition -= new Vector3(0, camBounds.yMax - bounds.yMax, 0);
            }

            transform.position = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
        }
    }
}