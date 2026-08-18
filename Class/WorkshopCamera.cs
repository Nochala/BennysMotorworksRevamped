using GTA;
using GTA.Math;
using GTA.Native;
using LemonUI.Tools;
using System;
using System.Drawing;
using System.Reflection;
using Control = GTA.Control;

namespace BennysMotorworksRevamped
{
    public enum CameraPosition
    {
        Car,
        Interior,
        Engine,
        RearEngine,
        Trunk,
        FrontBumper,
        RearBumper,
        Grille,
        Tank,
        Plaque,
        BackPlate,
        FrontPlate,
        Wheels,
        Hood,
        RearWindscreen,
        BikeExhaust,
        FrontMuguard,
        RearMuguard,
        RearHood,
        FrontTrunk,
        Boost,
        Exhaust,
    }

    public enum CameraRotationMode
    {
        Around,
        FirstPerson,
    }

    internal enum InteriorCameraFocus
    {
        Dashboard,
        DoorSpeaker,
        Ornaments,
        RearSpeakers,
        Seats,
        SteeringWheel,
    }

    public class WorkshopCamera
    {
        private Camera _mainCamera;
        private bool _isDragging;
        private PointF _dragOffset;
        private Entity _target;
        private Vector3 _targetPos;
        private CameraPosition _internalCameraPosition;
        private CameraRotationMode _rotationMode;
        private CameraClamp _cameraClamp;
        private float _cameraZoom;
        private bool _justSwitched;

        public bool IsLerping;
        private DateTime startTime;
        private float duration;
        private Vector3 startValuePosition;
        private Vector3 endValuePosition;
        private Vector3 startValueRotation;
        private Vector3 endValueRotation;

        private const float MinimumSafeCameraDistance = 0.35f;
        private const float MaximumSafeCameraDistance = 16.0f;
        private const float MinimumSafeCameraDepthOffset = 2.25f;
        private const float MinimumValidStoredCameraDistance = 1.5f;
        private const float MaximumValidStoredCameraDistance = 10.5f;
        private const float DefaultCameraForwardOffset = 4.25f;
        private const float DefaultCameraSideOffset = -3.0f;
        private const float DefaultCameraHeightOffset = 1.1f;
        private const float DefaultTargetHeightOffset = 0.2f;

        public WorkshopCamera()
        {
            Camera.DeleteAllCameras();
        }

        public CameraPosition MainCameraPosition
        {
            get => _internalCameraPosition;
            set
            {
                if (EnsureCameraReady())
                {
                    OnCameraChange(value);
                }
                _internalCameraPosition = value;
            }
        }

        public Vector3 Rotation => _mainCamera?.Rotation ?? Vector3.Zero;

        public bool IsDragging => _isDragging;

        public CameraRotationMode RotationMode
        {
            get => _rotationMode;
            set => _rotationMode = value;
        }

        public float CameraZoom
        {
            get => _cameraZoom;
            set
            {
                if (_mainCamera != null)
                {
                    Vector3 dir = CutsceneManager.RotationToDirection(_mainCamera.Rotation);
                    _mainCamera.Position += dir * (_cameraZoom - value);
                }

                _cameraZoom = value;
            }
        }

        public CameraClamp CameraClamp
        {
            get => _cameraClamp;
            set => _cameraClamp = value;
        }

        internal void FocusInteriorComponent(InteriorCameraFocus focus)
        {
            if (!EnsureCameraReady())
            {
                return;
            }

            if (_internalCameraPosition != CameraPosition.Interior || RotationMode != CameraRotationMode.FirstPerson)
            {
                OnCameraChange(CameraPosition.Interior);
                _internalCameraPosition = CameraPosition.Interior;
            }

            if (_mainCamera == null)
            {
                return;
            }

            Vector3 defaultCameraPosition = Game.Player.Character.Bones[Bone.IKHead].Position + new Vector3(0f, 0f, 0.1f);
            Vector3 cameraPosition = GetInteriorCameraPosition(focus, defaultCameraPosition);
            Vector3 focusPosition = GetInteriorFocusPosition(focus, cameraPosition);
            if (!IsFiniteVector(cameraPosition) || !IsFiniteVector(focusPosition) || cameraPosition.DistanceTo(focusPosition) < 0.1f)
            {
                return;
            }

            _mainCamera.StopPointing();
            _targetPos = focusPosition;
            RotationMode = CameraRotationMode.FirstPerson;
            Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 0, false);
            CameraClamp = new CameraClamp { MaxVerticalValue = -60.0f, MinVerticalValue = -3.0f };
            StartLerp(cameraPosition, GetStableLookRotation(focusPosition - cameraPosition, _mainCamera.Rotation.Z));
            _justSwitched = true;
        }

        private static Vector3 GetStableLookRotation(Vector3 direction, float referenceYaw)
        {
            if (!IsFiniteVector(direction))
            {
                return new Vector3(0f, 0f, referenceYaw);
            }

            float horizontalDistance = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            float pitch = (float)CutsceneManager.RadToDeg(Math.Atan2(direction.Z, horizontalDistance));
            float yaw = horizontalDistance > 0.001f
                ? (float)CutsceneManager.RadToDeg(-Math.Atan2(direction.X, direction.Y))
                : referenceYaw;

            while (yaw - referenceYaw > 180.0f)
            {
                yaw -= 360.0f;
            }
            while (yaw - referenceYaw < -180.0f)
            {
                yaw += 360.0f;
            }

            return new Vector3(Clamp(pitch, -89.0f, 89.0f), 0f, yaw);
        }

        private Vector3 GetInteriorCameraPosition(InteriorCameraFocus focus, Vector3 defaultPosition)
        {
            if (focus != InteriorCameraFocus.RearSpeakers)
            {
                return defaultPosition;
            }

            Vector3 rearWindowPosition;
            if (TryGetUsableBonePosition(_target, "windscreen_r", out rearWindowPosition))
            {
                return rearWindowPosition - _target.ForwardVector * 0.75f + _target.UpVector * 0.1f;
            }

            if (TryGetUsableBonePosition(_target, "boot", out rearWindowPosition))
            {
                return rearWindowPosition - _target.ForwardVector * 0.75f + _target.UpVector * 0.75f;
            }

            if (TryGetUsableBonePosition(_target, "bumper_r", out rearWindowPosition))
            {
                return rearWindowPosition - _target.ForwardVector * 0.75f + _target.UpVector * 1.0f;
            }

            return _target.Position - _target.ForwardVector * 2.0f + _target.UpVector * 0.9f;
        }

        private Vector3 GetInteriorFocusPosition(InteriorCameraFocus focus, Vector3 cameraPosition)
        {
            Vector3 bonePosition;
            switch (focus)
            {
                case InteriorCameraFocus.Dashboard:
                    return cameraPosition + _target.ForwardVector * 0.75f - _target.UpVector * 0.2f;
                case InteriorCameraFocus.DoorSpeaker:
                    if (TryGetUsableBonePosition(_target, "door_dside_f", out bonePosition))
                    {
                        return bonePosition;
                    }
                    return cameraPosition - _target.RightVector * 0.65f - _target.UpVector * 0.25f;
                case InteriorCameraFocus.Ornaments:
                    return cameraPosition + _target.ForwardVector * 0.65f + _target.RightVector * 0.35f - _target.UpVector * 0.1f;
                case InteriorCameraFocus.RearSpeakers:
                    if (TryGetUsableBonePosition(_target, "windscreen_r", out bonePosition))
                    {
                        return bonePosition + _target.ForwardVector * 0.4f - _target.UpVector * 0.15f;
                    }
                    if (TryGetAverageBonePosition(_target, "seat_dside_r", "seat_pside_r", out bonePosition))
                    {
                        return bonePosition - _target.ForwardVector * 0.35f + _target.UpVector * 0.35f;
                    }
                    return _target.Position - _target.ForwardVector * 0.65f + _target.UpVector * 0.65f;
                case InteriorCameraFocus.Seats:
                    if (TryGetUsableBonePosition(_target, "seat_pside_f", out bonePosition))
                    {
                        return bonePosition + _target.UpVector * 0.2f;
                    }
                    return cameraPosition + _target.ForwardVector * 0.15f + _target.RightVector * 0.45f - _target.UpVector * 0.45f;
                case InteriorCameraFocus.SteeringWheel:
                    if (TryGetUsableBonePosition(_target, "steeringwheel", out bonePosition))
                    {
                        return bonePosition;
                    }
                    return cameraPosition + _target.ForwardVector * 0.5f - _target.UpVector * 0.3f;
                default:
                    return cameraPosition + _target.ForwardVector;
            }
        }

        public void Stop()
        {
            ResetTransientState();

            try { Function.Call((Hash)0x8DB8CFFD58B62552UL, 0); } catch { }

            StopRenderingCamera();

            try { Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, false, 0); } catch { }

            Camera.DeleteAllCameras();
            _mainCamera = null;
            _target = null;
            _targetPos = Vector3.Zero;
            _internalCameraPosition = CameraPosition.Car;
        }

        public void RepositionFor(Vehicle lowrider)
        {
            if (lowrider == null || !lowrider.Exists())
            {
                return;
            }

            ResetTransientState();
            _target = lowrider;
            _targetPos = GetDefaultTargetPosition(lowrider);
            Vector3 defaultCameraPosition = GetDefaultCameraPosition(lowrider);

            Camera.DeleteAllCameras();
            _mainCamera = CreateScriptedCamera(
                defaultCameraPosition,
                CutsceneManager.DirectionToRotation(_targetPos - defaultCameraPosition),
                GameplayCamera.FieldOfView);

            _mainCamera.PointAt(_targetPos);

            StartRenderingCamera(_mainCamera);
            _cameraZoom = (_targetPos - _mainCamera.Position).Length();
            _internalCameraPosition = CameraPosition.Car;
            RotationMode = CameraRotationMode.Around;
            CameraClamp = new CameraClamp
            {
                MaxVerticalValue = -40.0f,
                MinVerticalValue = -3.0f,
            };
            _mainCamera.Shake(CameraShake.Hand, 0.5f);
        }

        private static Vector3 GetDefaultTargetPosition(Entity target)
        {
            return target.Position + target.UpVector * DefaultTargetHeightOffset;
        }

        private static Vector3 GetDefaultCameraPosition(Entity target)
        {
            return target.Position
                + target.ForwardVector * DefaultCameraForwardOffset
                + target.RightVector * DefaultCameraSideOffset
                + target.UpVector * DefaultCameraHeightOffset;
        }

        private void ResetTransientState()
        {
            IsLerping = false;
            _justSwitched = false;
            _isDragging = false;
            _dragOffset = PointF.Empty;
        }

        private bool HasUsableMainCamera()
        {
            if (_mainCamera == null)
            {
                return false;
            }

            try
            {
                return _mainCamera.Exists();
            }
            catch
            {
                return false;
            }
        }

        private bool EnsureCameraReady()
        {
            Vehicle currentVehicle = Helper.veh;
            bool targetMatchesCurrentVehicle = currentVehicle == null
                || !currentVehicle.Exists()
                || (_target != null && _target.Exists() && _target.Handle == currentVehicle.Handle);

            if (!HasUsableMainCamera() || _target == null || !_target.Exists() || !targetMatchesCurrentVehicle)
            {
                if (currentVehicle == null || !currentVehicle.Exists())
                {
                    return false;
                }

                RepositionFor(currentVehicle);
            }

            StartRenderingCamera(_mainCamera);
            return HasUsableMainCamera();
        }

        public bool IsMouseInMenu()
        {
            PointF topLeft = SafeZone.GetSafePosition(new PointF(0f, 0f));
            SizeF size = new SizeF(431f, 550f);
            return GameScreen.IsCursorInArea(topLeft, size);
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }

        private static bool IsFiniteVector(Vector3 value)
        {
            return IsFinite(value.X) && IsFinite(value.Y) && IsFinite(value.Z);
        }

        private static bool IsValidStoredCameraPose(Entity target, Vector3 position, Vector3 rotation)
        {
            if (target == null || !target.Exists() || !IsFiniteVector(position) || !IsFiniteVector(rotation))
            {
                return false;
            }

            float distance = target.Position.DistanceTo(position);
            if (distance < MinimumValidStoredCameraDistance || distance > MaximumValidStoredCameraDistance)
            {
                return false;
            }

            if (position.Z < target.Position.Z - 0.5f)
            {
                return false;
            }

            return true;
        }

        private bool IsCurrentCameraPoseSafe()
        {
            if (_mainCamera == null || _target == null || !_target.Exists() || !IsFiniteVector(_mainCamera.Position) || !IsFiniteVector(_mainCamera.Rotation))
            {
                return false;
            }

            Vector3 focus = _targetPos;
            if (!IsFiniteVector(focus))
            {
                focus = _target.Position;
            }

            float distance = focus.DistanceTo(_mainCamera.Position);
            if (distance < MinimumSafeCameraDistance || distance > MaximumSafeCameraDistance)
            {
                return false;
            }

            if (_mainCamera.Position.Z < focus.Z - MinimumSafeCameraDepthOffset)
            {
                return false;
            }

            return true;
        }

        private void RestoreSafeAroundCameraPose()
        {
            if (_mainCamera == null || _target == null || !_target.Exists())
            {
                return;
            }

            Vector3 focus = IsFiniteVector(_targetPos) ? _targetPos : _target.Position;
            Vector3 backward = _target.ForwardVector * -1f;
            if (!IsFiniteVector(backward) || backward.Length() < 0.001f)
            {
                backward = Vector3.WorldSouth;
            }

            float safeZoom = Clamp(_cameraZoom > 0.01f ? _cameraZoom : 5.0f, 1.0f, 6.5f);
            Vector3 safePosition = focus + backward * safeZoom + Vector3.WorldUp * Math.Max(0.8f, safeZoom * 0.2f);

            _mainCamera.StopPointing();
            _mainCamera.Position = safePosition;
            _mainCamera.PointAt(focus);

            _isDragging = false;
            _dragOffset = PointF.Empty;

            try { Function.Call((Hash)0x8DB8CFFD58B62552UL, 0); } catch { }
        }

        private void EnsureCameraPoseIsSafe()
        {
            if (_mainCamera == null || RotationMode != CameraRotationMode.Around || MainCameraPosition == CameraPosition.Interior || IsLerping)
            {
                return;
            }

            if (IsCurrentCameraPoseSafe())
            {
                return;
            }

            RestoreSafeAroundCameraPose();
        }

        public void Update()
        {
            if (_mainCamera == null)
            {
                return;
            }

            Game.DisableControlThisFrame(Control.VehicleMouseControlOverride);

            if (IsLerping)
            {
                DateTime now = DateTime.Now;
                float elapsed = (float)now.Subtract(startTime).TotalMilliseconds;
                if (elapsed > duration)
                {
                    IsLerping = false;
                    _mainCamera.Position = endValuePosition;
                    _mainCamera.Rotation = endValueRotation;
                    return;
                }

                _mainCamera.Position = LerpVector(elapsed, duration, startValuePosition, endValuePosition);
                _mainCamera.Rotation = LerpVector(elapsed, duration, startValueRotation, endValueRotation);
                return;
            }

            if (_justSwitched)
            {
                _justSwitched = false;
                return;
            }

            if (Game.IsControlJustPressed(Control.Attack) && !_isDragging && !IsMouseInMenu())
            {
                _isDragging = true;
                float mouseX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorX);
                float mouseY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorY);
                Function.Call((Hash)0x8DB8CFFD58B62552UL, 4);
                mouseX = (mouseX * 2f) - 1f;
                mouseY = (mouseY * 2f) - 1f;
                _dragOffset = new PointF(mouseX, mouseY);
            }

            if (Game.IsControlJustReleased(Control.Attack) && _isDragging)
            {
                _isDragging = false;
                _dragOffset = PointF.Empty;
                Function.Call((Hash)0x8DB8CFFD58B62552UL, 0);
            }

            if (RotationMode == CameraRotationMode.Around)
            {
                UpdateAroundCamera();
            }
            else if (RotationMode == CameraRotationMode.FirstPerson)
            {
                UpdateFirstPersonCamera();
            }

            EnsureCameraPoseIsSafe();

        }

        private void UpdateAroundCamera()
        {
            if (_isDragging)
            {
                GTA.UI.Hud.ShowCursorThisFrame();
                Vector3 dir = CutsceneManager.RotationToDirection(_mainCamera.Rotation);
                float len = (_targetPos - _mainCamera.Position).Length();

                Vector3 rotLeft = _mainCamera.Rotation + new Vector3(0f, 0f, -10f);
                Vector3 rotRight = _mainCamera.Rotation + new Vector3(0f, 0f, 10f);
                Vector3 right = CutsceneManager.RotationToDirection(rotRight) - CutsceneManager.RotationToDirection(rotLeft);

                Vector3 rotUp = _mainCamera.Rotation + new Vector3(-20f, 0f, 0f);
                Vector3 rotDown = _mainCamera.Rotation + new Vector3(20f, 0f, 0f);
                Vector3 up = CutsceneManager.RotationToDirection(rotUp) - CutsceneManager.RotationToDirection(rotDown);

                float mouseX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorX);
                float mouseY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorY);
                mouseX = (mouseX * 2f) - 1f;
                mouseY = (mouseY * 2f) - 1f;

                Vector3 rotation = Vector3.Zero;
                if (!IsCameraClamped(true, mouseX - _dragOffset.X))
                {
                    rotation += right * 15f * (mouseX - _dragOffset.X);
                }
                if (!IsCameraClamped(false, mouseY - _dragOffset.Y))
                {
                    rotation += up * -((mouseY - _dragOffset.Y) * 15f);
                }
                rotation += dir * (len - CameraZoom);
                _mainCamera.Position += rotation;
                _dragOffset = new PointF(mouseX, mouseY);
            }

            if (Game.LastInputMethod == InputMethod.GamePad)
            {
                Vector3 dir = CutsceneManager.RotationToDirection(_mainCamera.Rotation);
                float len = (_targetPos - _mainCamera.Position).Length();

                Vector3 rotLeft = _mainCamera.Rotation + new Vector3(0f, 0f, -10f);
                Vector3 rotRight = _mainCamera.Rotation + new Vector3(0f, 0f, 10f);
                Vector3 right = CutsceneManager.RotationToDirection(rotRight) - CutsceneManager.RotationToDirection(rotLeft);

                Vector3 rotUp = _mainCamera.Rotation + new Vector3(-20f, 0f, 0f);
                Vector3 rotDown = _mainCamera.Rotation + new Vector3(20f, 0f, 0f);
                Vector3 up = CutsceneManager.RotationToDirection(rotUp) - CutsceneManager.RotationToDirection(rotDown);

                float mouseX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.LookLeftRight);
                float mouseY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.LookUpDown);
                Vector3 rotation = Vector3.Zero;

                if (!IsCameraClamped(true, mouseX))
                {
                    rotation += right * mouseX * 0.6f;
                }
                if (!IsCameraClamped(false, mouseY))
                {
                    rotation += up * -mouseY * 0.5f;
                }
                rotation += dir * (len - CameraZoom);
                _mainCamera.Position += rotation;
            }
        }

        private void UpdateFirstPersonCamera()
        {
            if (_isDragging)
            {
                GTA.UI.Hud.ShowCursorThisFrame();
                float mouseX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorX);
                float mouseY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.CursorY);
                mouseX = (mouseX * 2f) - 1f;
                mouseY = ((mouseY * 2f) - 1f) * -1f;

                Vector3 right = new Vector3(0f, 0f, 1f);
                Vector3 up = new Vector3(1f, 0f, 0f);
                Vector3 rotation = Vector3.Zero;

                if (!IsCameraClamped(true, mouseX - _dragOffset.X))
                {
                    rotation += right * 20f * (mouseX - _dragOffset.X);
                }
                if (!IsCameraClamped(false, mouseY - _dragOffset.Y))
                {
                    rotation += up * -((mouseY - _dragOffset.Y) * 20f);
                }
                _mainCamera.Rotation += rotation;
                _dragOffset = new PointF(mouseX, mouseY);
            }

            if (Game.LastInputMethod == InputMethod.GamePad)
            {
                float mouseX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.LookLeftRight) * -1f;
                float mouseY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)Control.LookUpDown);
                Vector3 right = new Vector3(0f, 0f, 1f);
                Vector3 up = new Vector3(1f, 0f, 0f);
                Vector3 rotation = Vector3.Zero;

                if (!IsCameraClamped(true, mouseX))
                {
                    rotation += right * mouseX * 4.0f;
                }
                if (!IsCameraClamped(false, mouseY))
                {
                    rotation += up * -mouseY * 4.0f;
                }
                _mainCamera.Rotation += rotation;
            }
        }

        public bool IsCameraClamped(bool horizontally, float delta)
        {
            if (_mainCamera == null || CameraClamp == null)
            {
                return false;
            }

            if (horizontally)
            {
                bool goingLeft = delta < 0f;
                float left = CameraClamp.LeftHorizontalValue;
                float right = CameraClamp.RightHorizontalValue;

                if (left > 180f)
                {
                    left -= 360f * ((int)(left / 360f) + 1);
                }
                if (right > 180f)
                {
                    right -= 360f * ((int)(right / 360f) + 1);
                }

                bool sameHemisphereLeft = (_mainCamera.Rotation.Z > 0f && left > 0f) || (_mainCamera.Rotation.Z < 0f && left < 0f);
                bool sameHemisphereRight = (_mainCamera.Rotation.Z > 0f && right > 0f) || (_mainCamera.Rotation.Z < 0f && right < 0f);

                if (goingLeft && _mainCamera.Rotation.Z > right && sameHemisphereRight)
                {
                    return true;
                }
                if (!goingLeft && _mainCamera.Rotation.Z < left && sameHemisphereLeft)
                {
                    return true;
                }
                return false;
            }
            else
            {
                bool goingDown = delta < 0f;
                if (goingDown && _mainCamera.Rotation.X > CameraClamp.MinVerticalValue)
                {
                    return true;
                }
                if (!goingDown && _mainCamera.Rotation.X < CameraClamp.MaxVerticalValue)
                {
                    return true;
                }
                return false;
            }
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
            {
                return max;
            }
            if (value < min)
            {
                return min;
            }
            return value;
        }

        public static Vector3 GetBonePosition(Entity target, string bone)
        {
            return target.Bones[bone].Position;
        }

        private static bool TryGetUsableBonePosition(Entity target, string bone, out Vector3 position)
        {
            position = Vector3.Zero;
            if (target == null || !target.Exists() || string.IsNullOrEmpty(bone))
            {
                return false;
            }

            try
            {
                if (!target.Bones.Contains(bone))
                {
                    return false;
                }

                position = target.Bones[bone].Position;
            }
            catch
            {
                return false;
            }

            return IsFiniteVector(position)
                && target.Position.DistanceTo(position) <= 12.0f
                && position.Z >= target.Position.Z - 2.0f;
        }

        private static bool TryGetAverageBonePosition(Entity target, string firstBone, string secondBone, out Vector3 position)
        {
            Vector3 firstPosition;
            Vector3 secondPosition;
            bool hasFirst = TryGetUsableBonePosition(target, firstBone, out firstPosition);
            bool hasSecond = TryGetUsableBonePosition(target, secondBone, out secondPosition);

            if (hasFirst && hasSecond)
            {
                position = (firstPosition + secondPosition) * 0.5f;
                return true;
            }

            if (hasFirst)
            {
                position = firstPosition;
                return true;
            }

            if (hasSecond)
            {
                position = secondPosition;
                return true;
            }

            position = Vector3.Zero;
            return false;
        }

        private static Vector3 GetPlaqueTargetPosition(Entity target, string preferredBone)
        {
            Vector3 position;
            if (TryGetUsableBonePosition(target, preferredBone, out position) && position.Z >= target.Position.Z - 0.5f)
            {
                return position;
            }

            if (TryGetUsableBonePosition(target, "windscreen_r", out position) && position.Z >= target.Position.Z - 0.5f)
            {
                return position;
            }

            if (TryGetAverageBonePosition(target, "seat_dside_r", "seat_pside_r", out position) && position.Z >= target.Position.Z - 0.5f)
            {
                return position - target.ForwardVector * 0.25f + target.UpVector * 0.35f;
            }

            return target.Position - target.ForwardVector * 0.55f + target.UpVector * 0.65f;
        }

        private static Vector3 GetPlaqueCameraPosition(Entity target, Vector3 plaqueTargetPosition)
        {
            try
            {
                Vector3 localPlaquePosition = target.GetPositionOffset(plaqueTargetPosition);
                Vector3 rearWindowPosition;
                if (TryGetUsableBonePosition(target, "windscreen_r", out rearWindowPosition))
                {
                    localPlaquePosition.Z = target.GetPositionOffset(rearWindowPosition).Z + 0.05f;
                }

                Vector3 minimumDimensions;
                Vector3 maximumDimensions;
                target.Model.GetDimensions(out minimumDimensions, out maximumDimensions);
                if (IsFiniteVector(minimumDimensions)
                    && IsFiniteVector(maximumDimensions)
                    && minimumDimensions.Y < -0.1f
                    && minimumDimensions.Y > -10.0f)
                {
                    Vector3 cameraPosition = target.GetOffsetPosition(new Vector3(localPlaquePosition.X, minimumDimensions.Y - 0.35f, localPlaquePosition.Z));
                    if (IsFiniteVector(cameraPosition) && target.Position.DistanceTo(cameraPosition) <= 12.0f)
                    {
                        return cameraPosition;
                    }
                }
            }
            catch
            {
            }

            return plaqueTargetPosition - target.ForwardVector * 1.5f + target.UpVector * 0.1f;
        }

        public static float QuadraticEasing(float currentTime, float startValue, float changeInValue, float duration)
        {
            currentTime /= duration / 2f;
            if (currentTime < 1f)
            {
                return changeInValue / 2f * currentTime * currentTime + startValue;
            }
            currentTime -= 1f;
            return -changeInValue / 2f * (currentTime * (currentTime - 2f) - 1f) + startValue;
        }

        public static Vector3 LerpVector(float currentTime, float duration, Vector3 startValue, Vector3 destination)
        {
            return new Vector3(
                QuadraticEasing(currentTime, startValue.X, destination.X - startValue.X, duration),
                QuadraticEasing(currentTime, startValue.Y, destination.Y - startValue.Y, duration),
                QuadraticEasing(currentTime, startValue.Z, destination.Z - startValue.Z, duration));
        }

        private void StartLerp(Vector3 endPosition, Vector3 endRotation)
        {
            if (_mainCamera == null)
            {
                return;
            }

            startValueRotation = _mainCamera.Rotation;
            startValuePosition = _mainCamera.Position;
            duration = 1000.0f;
            IsLerping = true;
            startTime = DateTime.Now;
            endValuePosition = endPosition;
            endValueRotation = endRotation;
        }

        private void SetAroundCamera(Vector3 targetPos, float zoom, Vector3 endPosition, Vector3 endRotation, CameraClamp clamp, bool pointAtTarget = true)
        {
            Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 255, false);
            RotationMode = CameraRotationMode.Around;
            _targetPos = targetPos;
            _cameraZoom = zoom;
            if (pointAtTarget)
            {
                _mainCamera.StopPointing();
                _mainCamera.PointAt(_targetPos);
            }
            CameraClamp = clamp;
            StartLerp(endPosition, endRotation);
            _justSwitched = true;
        }

        private static Camera CreateScriptedCamera(Vector3 position, Vector3 rotation, float fieldOfView)
        {
            try
            {
                MethodInfo createMethod = typeof(Camera).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
                if (createMethod != null)
                {
                    ParameterInfo[] parameters = createMethod.GetParameters();
                    if (parameters.Length == 3 && parameters[0].ParameterType.IsEnum)
                    {
                        Type enumType = parameters[0].ParameterType;
                        object camHash;

                        string[] preferredNames = new[]
                        {
                            "DefaultScriptedCamera",
                            "DEFAULT_SCRIPTED_CAMERA",
                            "Default",
                        };

                        camHash = null;
                        foreach (string name in preferredNames)
                        {
                            if (Enum.IsDefined(enumType, name))
                            {
                                camHash = Enum.Parse(enumType, name);
                                break;
                            }
                        }

                        if (camHash == null)
                        {
                            Array values = Enum.GetValues(enumType);
                            if (values.Length > 0)
                            {
                                camHash = values.GetValue(0);
                            }
                        }

                        if (camHash != null)
                        {
                            Camera camera = (Camera)createMethod.Invoke(null, new object[] { camHash, position, rotation });
                            if (camera != null)
                            {
                                camera.FieldOfView = fieldOfView;
                                return camera;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            int handle = Function.Call<int>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
            Camera fallbackCamera = new Camera(handle)
            {
                Position = position,
                Rotation = rotation,
                FieldOfView = fieldOfView,
            };
            return fallbackCamera;
        }

        private static void StartRenderingCamera(Camera camera)
        {
            if (camera == null)
            {
                return;
            }

            try
            {
                camera.IsActive = true;
            }
            catch
            {
            }

            try
            {
                MethodInfo[] methods = typeof(ScriptCameraDirector).GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name != "StartRendering")
                    {
                        continue;
                    }

                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 0)
                    {
                        method.Invoke(null, null);
                        break;
                    }
                    if (parameters.Length == 1)
                    {
                        Type paramType = parameters[0].ParameterType;
                        if (paramType == typeof(bool))
                        {
                            method.Invoke(null, new object[] { true });
                            break;
                        }
                        if (paramType.IsAssignableFrom(typeof(Camera)))
                        {
                            method.Invoke(null, new object[] { camera });
                            break;
                        }
                    }
                }
            }
            catch
            {
            }

            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, false, 0, true, false, 0);
        }

        private static void StopRenderingCamera()
        {
            try
            {
                MethodInfo[] methods = typeof(ScriptCameraDirector).GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == "StopRendering" && method.GetParameters().Length == 0)
                    {
                        method.Invoke(null, null);
                        return;
                    }
                }
            }
            catch
            {
            }

            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, false, 0);
        }

        private void OnCameraChange(CameraPosition newPos)
        {
            if (_mainCamera == null || _target == null || !_target.Exists())
            {
                return;
            }

            switch (newPos)
            {
                case CameraPosition.Car:
                    Game.Player.Character.Opacity = 255;
                    RotationMode = CameraRotationMode.Around;

                    _targetPos = GetDefaultTargetPosition(_target);

                    Vector3 newCamPos = GetDefaultCameraPosition(_target);
                    _cameraZoom = _targetPos.DistanceTo(newCamPos);
                    CameraClamp = new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f };
                    _mainCamera.Shake(CameraShake.Hand, 0.5f);

                    _mainCamera.StopPointing();
                    _mainCamera.PointAt(_targetPos);

                    StartLerp(newCamPos, CutsceneManager.DirectionToRotation(_targetPos - newCamPos));
                    _justSwitched = true;
                    break;
                case CameraPosition.Wheels:
                    Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 255, false);
                    RotationMode = CameraRotationMode.Around;
                    if (_internalCameraPosition != CameraPosition.Car)
                    {
                        RepositionFor((Vehicle)_target);
                    }
                    CameraClamp = new CameraClamp
                    {
                        MaxVerticalValue = -60.0f,
                        MinVerticalValue = -3.0f,
                        LeftHorizontalValue = _target.Heading - 130.0f,
                        RightHorizontalValue = _target.Heading - 380.0f,
                    };
                    _cameraZoom = 4.0f;
                    StartLerp(_target.Position - _target.RightVector * 4.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading - 90.0f));
                    break;
                case CameraPosition.Interior:
                    IsLerping = false;
                    Vector3 headPos = Game.Player.Character.Bones[Bone.IKHead].Position;
                    Camera.DeleteAllCameras();
                    _mainCamera = CreateScriptedCamera(headPos + new Vector3(0f, 0f, 0.1f), new Vector3(0f, 0f, _target.Heading), GameplayCamera.FieldOfView);
                    StartRenderingCamera(_mainCamera);
                    _targetPos = headPos;
                    RotationMode = CameraRotationMode.FirstPerson;
                    Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 0, false);
                    CameraClamp = new CameraClamp { MaxVerticalValue = -60.0f, MinVerticalValue = -3.0f };
                    _justSwitched = true;
                    break;
                case CameraPosition.Engine:
                    SetAroundCamera(GetBonePosition(_target, "engine"), 3.0f, _targetPos + _target.ForwardVector * 3.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.Hood:
                    SetAroundCamera(GetBonePosition(_target, "bonnet"), 3.0f, _targetPos + _target.ForwardVector * 3.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.Boost:
                    _targetPos = _target.Position;
                    SetAroundCamera(_targetPos, 1.5f, _targetPos + _target.ForwardVector * -2.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.Exhaust:
                    _targetPos = _target.Position;
                    SetAroundCamera(_targetPos, 4.0f, _target.Position - _target.RightVector + new Vector3(1f, 0f, 0f) * 4.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading - 60.0f), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.FrontTrunk:
                    _targetPos = GetBonePosition(_target, "boot");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * 3.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.FrontMuguard:
                    _targetPos = _target.Bones.Contains("misc_i") ? GetBonePosition(_target, "misc_i") : GetBonePosition(_target, "forks_l");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * 3.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.Trunk:
                    _targetPos = _target.Bones.Contains("boot") ? GetBonePosition(_target, "boot") : GetBonePosition(_target, "bumper_r");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * -3.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.RearHood:
                    _targetPos = _target.Bones.Contains("bonnet") ? GetBonePosition(_target, "bonnet") : GetBonePosition(_target, "bumper_r");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * -3.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.BikeExhaust:
                    _targetPos = GetBonePosition(_target, "exhaust");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * -3.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.RearMuguard:
                    _targetPos = GetBonePosition(_target, "misc_d");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * -3.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.RearWindscreen:
                    _targetPos = _target.Bones.Contains("windscreen_r") ? GetBonePosition(_target, "windscreen_r") : GetBonePosition(_target, "bumper_r");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * -3.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.RearEngine:
                    _targetPos = GetBonePosition(_target, "engine");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * -3.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.FrontBumper:
                    _targetPos = GetBonePosition(_target, "neon_f");
                    SetAroundCamera(_targetPos, 2.0f, _targetPos + _target.ForwardVector * 2.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.Grille:
                    _targetPos = GetBonePosition(_target, "neon_f");
                    SetAroundCamera(_targetPos, 2.0f, _targetPos + _target.ForwardVector * 2.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.RearBumper:
                    _targetPos = GetBonePosition(_target, "neon_b");
                    SetAroundCamera(_targetPos, 2.0f, _targetPos + _target.ForwardVector * -2.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.Tank:
                    _targetPos = GetBonePosition(_target, "neon_b");
                    SetAroundCamera(_targetPos, 2.0f, _targetPos + _target.ForwardVector * -2.0f, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.Plaque:
                    string modelName = _target != null && _target.Exists() ? _target.Model.ToString().ToLowerInvariant() : string.Empty;
                    string plaqueBone;
                    switch (modelName)
                    {
                        case "buccaneer2":
                        case "faction2":
                        case "moonbeam2":
                        case "slamvan3":
                        case "faction3":
                            plaqueBone = "misc_h";
                            break;
                        case "voodoo":
                        case "chino2":
                            plaqueBone = "misc_j";
                            break;
                        case "primo2":
                            plaqueBone = "misc_d";
                            break;
                        case "sabregt2":
                        case "virgo2":
                            plaqueBone = "misc_n";
                            break;
                        case "tornado5":
                            plaqueBone = "misc_o";
                            break;
                        case "minivan2":
                            plaqueBone = "misc_c";
                            break;
                        default:
                            plaqueBone = "windscreen_r";
                            break;
                    }
                    _targetPos = GetPlaqueTargetPosition(_target, plaqueBone);
                    Vector3 plaqueCameraPosition = GetPlaqueCameraPosition(_target, _targetPos);
                    float plaqueCameraZoom = Math.Max(1.0f, _targetPos.DistanceTo(plaqueCameraPosition));
                    SetAroundCamera(_targetPos, plaqueCameraZoom, plaqueCameraPosition, GetStableLookRotation(_targetPos - plaqueCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -20.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.BackPlate:
                    _targetPos = _target.Bones.Contains("platelight") ? GetBonePosition(_target, "platelight") : GetBonePosition(_target, "neon_b");
                    SetAroundCamera(_targetPos, 1.0f, _targetPos + _target.ForwardVector * -1.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.FrontPlate:
                    _targetPos = GetBonePosition(_target, "neon_f");
                    SetAroundCamera(_targetPos, 1.0f, _targetPos + _target.ForwardVector * 2.0f + _target.UpVector * 2.0f, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
            }
        }
    }
}
