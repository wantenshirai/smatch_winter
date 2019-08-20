using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectV2Manager : MonoBehaviour
{
    static KinectV2Manager _instance;

    public static KinectV2Manager instance
    {
        get
        {
            if (_instance == null)
            {
                var previous = FindObjectOfType(typeof(KinectV2Manager));
                if (previous)
                {
                    Debug.LogWarning("Initialized twice. Don't use KinectV2Manager in the scene hierarchy.");
                    _instance = (KinectV2Manager)previous;
                }
                else
                {
                    var go = new GameObject("__KinectV2Manager");
                    _instance = go.AddComponent<KinectV2Manager>();
                    DontDestroyOnLoad(go);
                    go.hideFlags = HideFlags.HideInHierarchy;
                }
            }
            return _instance;
        }
    }

    public Windows.Kinect.Vector4 floorClipPlane
    {
        get;
        private set;
    }

    public Body[] GetData()
    {
        return bodies;
    }

    private KinectSensor sensor;
    private BodyFrameReader bodyFrameReader;
    private Body[] bodies;


    // Start is called before the first frame update
    void Start()
    {
        sensor = KinectSensor.GetDefault();

        if (sensor != null)
        {
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();

            if (!sensor.IsOpen)
            {
                sensor.Open();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bodyFrameReader != null)
        {
            using(var frame = bodyFrameReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Body[sensor.BodyFrameSource.BodyCount];
                    }

                    frame.GetAndRefreshBodyData(bodies);

                    floorClipPlane = frame.FloorClipPlane;
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (bodyFrameReader != null)
        {
            bodyFrameReader.Dispose();
            bodyFrameReader = null;
        }

        if (sensor != null)
        {
            if (sensor.IsOpen)
            {
                sensor.Close();
            }

            sensor = null;
        }
    }
}

public static class VectorExtensions
{
    public static Quaternion ToQuaternion(this Windows.Kinect.Vector4 vector, Quaternion comp)
    {
        return Quaternion.Inverse(comp) *
                 new Quaternion(-vector.X, -vector.Y, vector.Z, vector.W);
    }

    public static Windows.Kinect.Vector4 ToMirror(this Windows.Kinect.Vector4 vector)
    {
        return new Windows.Kinect.Vector4()
        {
            X = vector.X,
            Y = -vector.Y,
            Z = -vector.Z,
            W = vector.W
        };
    }
}
