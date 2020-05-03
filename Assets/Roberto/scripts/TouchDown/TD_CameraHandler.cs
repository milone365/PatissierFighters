using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

namespace td
{
    //カメラhandlerに基づいて作れた
    public class TD_CameraHandler : MonoBehaviour
    {

        Camera cam;
        Camera Getcamera()
        {
            return cam;
        }

        public Transform camTrans;
        public Transform target;
        public Transform pivot;
        public Transform mTransform;
        public bool leftPivot;
        float delt;
        float mouseX, mouseY, smothx, smothY, smotxVelocity, smothyVelocity;
        float lookAngle, tiltAngle;
        TD_StateManager states;
        public Cameravalues values;
        TD_InputHandler inp;
        string playerId = "P1_";
        public void Init(TD_InputHandler _inp)
        {
            inp = _inp;
            cam = GetComponentInChildren<Camera>();
            mTransform = this.transform;
            states = inp.states;
            target = states.getTransform();
            playerId = inp.playerID;
        }
        public void fixedTick(float d)
        {
            delt = d;
            if (target == null) return;
            handlePosition();
            HandleRotation();
            float speed = values.moveSpeed;
            if (states.isAiming) { speed = values.aimSpeed; }
            Vector3 targetPos = Vector3.Lerp(mTransform.position, target.position, delt * speed);
            mTransform.position = target.position;
        }
        void handlePosition()
        {
            float targetX = values.normalX;
            float targeY = values.normalY;
            float targetZ = values.normalZ;

            if (states.isAiming)
            {
                targetX = values.aimX;
                targetZ = values.aimZ;
            }
            if (leftPivot)
            {
                targetX = -targetX;
            }
            Vector3 newPivotPos = pivot.localPosition;
            newPivotPos.x = targetX;
            newPivotPos.z = targeY;
            Vector3 newCameraPos = camTrans.localPosition;
            newCameraPos.z = targetZ;
            float t = delt * values.adaptSpeed;
            pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivotPos, t);
            camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, newCameraPos, t);
        }
        void HandleRotation()
        {
            mouseX = Input.GetAxis(playerId + StaticStrings.cameraX);
            mouseY = Input.GetAxis(playerId + StaticStrings.cameraY);
            if (values.turnSmooth > 0)
            {
                smothx = Mathf.SmoothDamp(smothx, mouseX, ref smotxVelocity, values.turnSmooth);
                smothY = Mathf.SmoothDamp(smothY, mouseY, ref smothyVelocity, values.turnSmooth);
            }
            else
            {
                smothx = mouseX;
                smothY = mouseY;
            }

            lookAngle += smothx * values.y_rotate_speed;
            Quaternion targetRot = Quaternion.Euler(0, lookAngle, 0);
            mTransform.rotation = targetRot;
            tiltAngle -= smothY * values.x_rotate_speed;
            tiltAngle = Mathf.Clamp(tiltAngle, values.minAngle, values.maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        }
    }
}


