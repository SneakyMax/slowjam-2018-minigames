using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        public float ShakeAmount = 1;

        private Rect bounds;
        private Camera cam;

        private bool shaking;
        private bool shakeLeft;

        private void Awake()
        {
            Instance = this;
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

            if (shaking)
            {
                var shake = (shakeLeft ? Vector3.left : Vector3.right) * ShakeAmount;
                shakeLeft = !shakeLeft;
                currentPosition += shake;
            }

            transform.position = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
        }

        public void ShakeOn()
        {
            shaking = true;
        }

        public void ShakeOff()
        {
            shaking = false;
        }
    }
}