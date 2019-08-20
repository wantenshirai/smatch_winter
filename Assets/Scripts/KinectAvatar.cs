using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;

public class KinectAvatar : MonoBehaviour
{
    [SerializeField]
    public Body body = null;

    [SerializeField] GameObject Ref;
    [SerializeField] GameObject Head;
    [SerializeField] GameObject Neck;
    [SerializeField] GameObject LeftUpLeg;
    [SerializeField] GameObject LeftLeg;
    [SerializeField] GameObject RightUpLeg;
    [SerializeField] GameObject RightLeg;
    [SerializeField] GameObject Spine1;
    [SerializeField] GameObject LeftArm;
    [SerializeField] GameObject LeftForeArm;
    [SerializeField] GameObject LeftHand;
    [SerializeField] GameObject RightArm;
    [SerializeField] GameObject RightForeArm;
    [SerializeField] GameObject RightHand;

    public float x;
    public float y;
    public float z;
    public float alpha;

    // Update is called once per frame
    void Update()
    {

        var bodies = KinectV2Manager.instance.GetData();

        if (bodies != null)
        {
            Body body = bodies.FirstOrDefault(x => x.IsTracked==true);

            if (body != null)
            {
                Quaternion floorComp = Quaternion.FromToRotation(new Vector3(-KinectV2Manager.instance.floorClipPlane.X, KinectV2Manager.instance.floorClipPlane.Y, KinectV2Manager.instance.floorClipPlane.Z), Vector3.up);

                var joints = body.JointOrientations;

                var Head2 = joints[JointType.Head].Orientation.ToQuaternion(floorComp);
                var Neck2 = joints[JointType.Neck].Orientation.ToQuaternion(floorComp);
                var SpineBase = joints[JointType.SpineBase].Orientation.ToQuaternion(floorComp);
                var SpineMid = joints[JointType.SpineMid].Orientation.ToQuaternion(floorComp);
                var SpineShoulder = joints[JointType.SpineShoulder].Orientation.ToQuaternion(floorComp);
                var ShoulderLeft = joints[JointType.ShoulderLeft].Orientation.ToQuaternion(floorComp);
                var ShoulderRight = joints[JointType.ShoulderRight].Orientation.ToQuaternion(floorComp);
                var ElbowLeft = joints[JointType.ElbowLeft].Orientation.ToQuaternion(floorComp);
                var WristLeft = joints[JointType.WristLeft].Orientation.ToQuaternion(floorComp);
                var HandLeft = joints[JointType.HandLeft].Orientation.ToQuaternion(floorComp);
                var ElbowRight = joints[JointType.ElbowRight].Orientation.ToQuaternion(floorComp);
                var WristRight = joints[JointType.WristRight].Orientation.ToQuaternion(floorComp);
                var HandRight = joints[JointType.HandRight].Orientation.ToQuaternion(floorComp);
                var KneeLeft = joints[JointType.KneeLeft].Orientation.ToQuaternion(floorComp);
                var AnkleLeft = joints[JointType.AnkleLeft].Orientation.ToQuaternion(floorComp);
                var KneeRight = joints[JointType.KneeRight].Orientation.ToQuaternion(floorComp);
                var AnkleRight = joints[JointType.AnkleRight].Orientation.ToQuaternion(floorComp);


                // calc joint rotation
                var q = transform.rotation;
                this.transform.rotation = Quaternion.identity;

                var bodyComp = Quaternion.AngleAxis(180, Vector3.up);
                var armComp = Quaternion.AngleAxis(0.0f, Vector3.right) * Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(0.0f, Vector3.forward);
                var leftLegComp = Quaternion.AngleAxis(0.0f, Vector3.right) * Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(0.0f, Vector3.forward);
                var rightLegComp = Quaternion.AngleAxis(0.0f, Vector3.right) * Quaternion.AngleAxis(-90.0f, Vector3.up) * Quaternion.AngleAxis(0.0f, Vector3.forward);

                /*
                if (Head != null)
                    Head.transform.rotation = Quaternion.Lerp(Head.transform.rotation, Head2 * bodyComp, alpha);

                if (Neck != null)
                    Neck.transform.rotation = Quaternion.Lerp(Neck.transform.rotation, Neck2 * bodyComp, alpha);
                */

                if (Spine1!=null)
                    Spine1.transform.rotation = Quaternion.Lerp(Spine1.transform.rotation, SpineMid * bodyComp, alpha);

                if (RightArm != null)
                    RightArm.transform.rotation = Quaternion.Lerp(RightArm.transform.rotation, ElbowRight, alpha);

                if (RightForeArm != null)
                    RightForeArm.transform.rotation = Quaternion.Lerp(RightForeArm.transform.rotation, WristRight, alpha);
                 
                if (RightHand != null)
                    RightHand.transform.rotation = Quaternion.Lerp(RightHand.transform.rotation, HandRight * armComp, alpha);

                if (LeftArm != null)
                    LeftArm.transform.rotation = Quaternion.Lerp(LeftArm.transform.rotation, ElbowLeft, alpha);

                if (LeftForeArm != null)
                    LeftForeArm.transform.rotation = Quaternion.Lerp(LeftForeArm.transform.rotation, WristLeft, alpha);

                if (LeftHand != null)
                    LeftHand.transform.rotation = Quaternion.Lerp(LeftHand.transform.rotation, HandLeft * armComp, alpha);

                if (RightUpLeg != null)
                    RightUpLeg.transform.rotation = Quaternion.Lerp(RightUpLeg.transform.rotation, KneeRight, alpha);

                if (RightLeg != null)
                    RightLeg.transform.rotation = Quaternion.Lerp(RightLeg.transform.rotation, AnkleRight * rightLegComp, alpha);

                if (LeftUpLeg != null)
                    LeftUpLeg.transform.rotation = Quaternion.Lerp(LeftUpLeg.transform.rotation, KneeLeft, alpha);

                if (LeftLeg != null)
                    LeftLeg.transform.rotation = Quaternion.Lerp(LeftLeg.transform.rotation, AnkleLeft * leftLegComp, alpha);

                // set model rotation
                this.transform.rotation = q;

                // set model position
                var position = body.Joints[JointType.SpineMid].Position;
                Ref.transform.position = new Vector3(-position.X, position.Y, -position.Z) * z;
            }
        }
    }

}