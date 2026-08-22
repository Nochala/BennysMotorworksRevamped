using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace BennysMotorworksRevamped
{
    public static class CutsceneManager
    {
        private static readonly IntersectFlags DefaultRaycastFlags =
            IntersectFlags.Foliage |
            IntersectFlags.Objects |
            IntersectFlags.Peds |
            IntersectFlags.Vehicles |
            IntersectFlags.Map;

        public static double BoundRotationDeg(double angleDeg)
        {
            int wholeTurns = (int)(angleDeg / 360.0);
            double bounded = angleDeg - (wholeTurns * 360.0);

            if (bounded < 0.0)
            {
                bounded += 360.0;
            }

            return bounded;
        }

        public static Vector3 CrossWith(Vector3 left, Vector3 right)
        {
            return new Vector3(
                left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X);
        }

        public static double DegToRad(double deg) => deg * Math.PI / 180.0;
        public static double RadToDeg(double rad) => rad * 180.0 / Math.PI;

        public static Vector3 DirectionToRotation(Vector3 direction)
        {
            direction.Normalize();
            double pitch = Math.Atan2(direction.Z, direction.Y);
            double roll = 0.0;
            double yaw = -Math.Atan2(direction.X, direction.Y);

            return new Vector3((float)RadToDeg(pitch), (float)RadToDeg(roll), (float)RadToDeg(yaw));
        }

        public static Vector3 ForwardVector(Vector3 vector, float yaw)
        {
            float cos = (float)Math.Cos(yaw + 1.5707963267948966);
            float sin = (float)Math.Sin(yaw + 1.5707963267948966);
            Vector3 right = new Vector3(57.29578f * cos, 0.0f, 57.29578f * sin);
            return CrossWith(vector, right);
        }

        public static Entity RaycastEntity(Vector2 screenCoord, Vector3 camPos, Vector3 camRot)
        {
            Vector3 origin = camPos;
            Entity ignoreEntity = Game.Player.Character;
            Vector3 direction = ScreenRelToWorld(camPos, camRot, screenCoord) - origin;
            direction.Normalize();

            RaycastResult result = World.Raycast(
                origin + direction * 1.0f,
                origin + direction * 100.0f,
                DefaultRaycastFlags,
                ignoreEntity);

            return result.DidHit && result.HitEntity != null ? result.HitEntity : null;
        }

        public static Vector3 RaycastEverything(Vector2 screenCoord)
        {
            Vector3 camPos = GameplayCamera.Position;
            Vector3 camRot = GameplayCamera.Rotation;
            Vector3 worldTarget = ScreenRelToWorld(camPos, camRot, screenCoord);
            Entity ignoreEntity = Game.Player.Character;

            if (Game.Player.Character.IsInVehicle())
            {
                ignoreEntity = Game.Player.Character.CurrentVehicle;
            }

            Vector3 direction = worldTarget - camPos;
            direction.Normalize();

            RaycastResult result = World.Raycast(
                camPos + direction * 1.0f,
                camPos + direction * 100.0f,
                DefaultRaycastFlags,
                ignoreEntity);

            return result.DidHit ? result.HitPosition : camPos + direction * 100.0f;
        }

        public static Vector3 RaycastEverything(Vector2 screenCoord, Vector3 camPos, Vector3 camRot, Entity toIgnore)
        {
            Vector3 origin = camPos;
            Vector3 direction = ScreenRelToWorld(camPos, camRot, screenCoord) - origin;
            direction.Normalize();

            RaycastResult result = World.Raycast(
                origin + direction * 1.0f,
                origin + direction * 100.0f,
                DefaultRaycastFlags,
                toIgnore);

            return result.DidHit ? result.HitPosition : camPos + direction * 100.0f;
        }

        public static Vector3 RotationToDirection(Vector3 rotation)
        {
            double yaw = DegToRad(rotation.Z);
            double pitch = DegToRad(rotation.X);
            double cosPitch = Math.Abs(Math.Cos(pitch));

            return new Vector3(
                (float)(-Math.Sin(yaw) * cosPitch),
                (float)(Math.Cos(yaw) * cosPitch),
                (float)Math.Sin(pitch));
        }

        public static Vector3 ScreenRelToWorld(Vector3 camPos, Vector3 camRot, Vector2 coord)
        {
            Vector3 forward = RotationToDirection(camRot);
            Vector3 rotUp = camRot + new Vector3(10.0f, 0.0f, 0.0f);
            Vector3 rotDown = camRot + new Vector3(-10.0f, 0.0f, 0.0f);
            Vector3 rotLeft = camRot + new Vector3(0.0f, 0.0f, -10.0f);
            Vector3 stepVertical = RotationToDirection(rotUp) - RotationToDirection(rotDown);
            double rollRadians = -DegToRad(camRot.Y);
            Vector3 stepHorizontal = RotationToDirection(camRot + new Vector3(0.0f, 0.0f, 10.0f)) - RotationToDirection(rotLeft);
            Vector3 right = stepHorizontal * (float)Math.Cos(rollRadians) - stepVertical * (float)Math.Sin(rollRadians);
            Vector3 up = stepHorizontal * (float)Math.Sin(rollRadians) + stepVertical * (float)Math.Cos(rollRadians);
            Vector3 testPoint = camPos + forward * 10.0f;

            if (!WorldToScreenRel(testPoint + right + up, out Vector2 corner))
            {
                return testPoint;
            }

            if (!WorldToScreenRel(testPoint, out Vector2 center))
            {
                return testPoint;
            }

            if (Math.Abs(corner.X - center.X) < 0.001f || Math.Abs(corner.Y - center.Y) < 0.001f)
            {
                return testPoint;
            }

            float scaleX = (coord.X - center.X) / (corner.X - center.X);
            float scaleY = (coord.Y - center.Y) / (corner.Y - center.Y);
            return testPoint + right * scaleX + up * scaleY;
        }

        public static bool WorldToScreenRel(Vector3 worldCoords, out Vector2 screenCoords)
        {
            OutputArgument x = new OutputArgument();
            OutputArgument y = new OutputArgument();

            if (!Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, worldCoords.X, worldCoords.Y, worldCoords.Z, x, y))
            {
                screenCoords = new Vector2();
                return false;
            }

            screenCoords = new Vector2(
                (x.GetResult<float>() - 0.5f) * 2.0f,
                (y.GetResult<float>() - 0.5f) * 2.0f);

            return true;
        }
    }
}
