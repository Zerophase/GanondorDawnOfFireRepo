using UnityEngine;

public static class Helper
{
    public struct ClipPlane
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;
    }

    public static ClipPlane ClipPaneAtNear(Vector3 pos)
    {
        var clipPlane = new ClipPlane();

        if (Camera.main == null)
            return clipPlane;

        var transform = Camera.main.transform;
        var halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
        var aspect = Camera.main.aspect;
        var distance = Camera.main.nearClipPlane;
        var height = distance * Mathf.Tan(halfFOV);
        var width = height * aspect;

        clipPlane.LowerRight = pos + transform.right * width;
        clipPlane.LowerRight -= transform.up * height;
        clipPlane.LowerRight += transform.forward * distance;

        clipPlane.LowerLeft = pos - transform.right * width;
        clipPlane.LowerLeft -= transform.up * height;
        clipPlane.LowerLeft += transform.forward * distance;

        clipPlane.UpperRight = pos + transform.right * width;
        clipPlane.UpperRight += transform.up * height;
        clipPlane.UpperRight += transform.forward * distance;

        clipPlane.UpperLeft = pos - transform.right * width;
        clipPlane.UpperLeft += transform.up * height;
        clipPlane.UpperLeft += transform.forward * distance;

        return clipPlane;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle < -360)
                angle += 360;
            else if (angle > 360)
                angle -= 360;
        } while (angle < - 360 || angle > 360);
        //Then clamp and returns
        return Mathf.Clamp(angle, min, max);
    }
}
