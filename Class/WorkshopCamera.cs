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
        BikeBackrest,
        BikeWindshield,
        RearHood,
        FrontTrunk,
        Boost,
        Exhaust,
        Spoiler,
        Roof,
        FrontRoof,
        RearRoof,
        Headlights,
        RollCage,
        EngineBaySide,
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
        private int _shopDepthOfFieldCameraHandle;
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
        private Vector3 startValueFocus;
        private Vector3 endValueFocus;
        private Vector3 _renderedFocus;
        private bool _hasRenderedFocus;
        private bool _lerpAroundFocus;
        private bool _curvedTransition;
        private Vector3 _curveControlOne;
        private Vector3 _curveControlTwo;

        private const float MinimumSafeCameraDistance = 0.35f;
        private const float MaximumSafeCameraDistance = 16.0f;
        private const float MinimumSafeCameraDepthOffset = 2.25f;
        private const float MinimumValidStoredCameraDistance = 1.5f;
        private const float MaximumValidStoredCameraDistance = 10.5f;
        private const float DefaultCameraForwardOffset = 4.25f;
        private const float DefaultCameraSideOffset = -3.0f;
        private const float DefaultCameraHeightOffset = 1.1f;
        private const float DefaultTargetHeightOffset = 0.2f;
        private const float HeadlightCameraAngleDegrees = 60.0f;
        private const float HeadlightCameraForwardOffset = -0.45f;
        private const float HeadlightCameraRightOffset = 3.0f;
        private const float HeadlightCameraHeightOffset = 0.65f;
        private const float HeadlightFocusHeightOffset = 0.2f;

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

        private bool _shopDepthOfFieldUnavailable;

        public void UpdateShopDepthOfField(bool enabled)
        {
            if (!enabled || _mainCamera == null)
            {
                ResetShopDepthOfField();
                return;
            }

            if (_shopDepthOfFieldUnavailable)
            {
                return;
            }

            try
            {
                if (!_mainCamera.Exists())
                {
                    ResetShopDepthOfField();
                    return;
                }

                int handle = _mainCamera.Handle;
                if (_shopDepthOfFieldCameraHandle != handle)
                {
                    ResetShopDepthOfField();
                    Function.Call(Hash.SET_CAM_NEAR_DOF, handle, 0.15f);
                    Function.Call(Hash.SET_CAM_FAR_DOF, handle, 10.0f);
                    Function.Call(Hash.SET_CAM_DOF_STRENGTH, handle, 0.88f);
                    Function.Call(Hash.SET_CAM_USE_SHALLOW_DOF_MODE, handle, true);
                    _shopDepthOfFieldCameraHandle = handle;
                }

                Function.Call(Hash.SET_USE_HI_DOF);
            }
            catch (Exception ex)
            {
                ResetShopDepthOfField();
                _shopDepthOfFieldUnavailable = true;
                Logger.Log("Workshop depth of field unavailable: " + ex.Message);
            }
        }

        private void ResetShopDepthOfField()
        {
            int handle = _shopDepthOfFieldCameraHandle;
            _shopDepthOfFieldCameraHandle = 0;
            if (handle == 0)
            {
                return;
            }

            try
            {
                if (Function.Call<bool>(Hash.DOES_CAM_EXIST, handle))
                {
                    Function.Call(Hash.SET_CAM_USE_SHALLOW_DOF_MODE, handle, false);
                    Function.Call(Hash.SET_CAM_DOF_STRENGTH, handle, 0.0f);
                }
            }
            catch
            {
            }
        }

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
            _hasRenderedFocus = false;
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
            ResetShopDepthOfField();
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

            ResetShopDepthOfField();
            Camera.DeleteAllCameras();
            _mainCamera = CreateScriptedCamera(
                defaultCameraPosition,
                CutsceneManager.DirectionToRotation(_targetPos - defaultCameraPosition),
                GameplayCamera.FieldOfView);

            _mainCamera.PointAt(_targetPos);
            _renderedFocus = _targetPos;
            _hasRenderedFocus = true;

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
            _lerpAroundFocus = false;
            _curvedTransition = false;
            _hasRenderedFocus = false;
            _justSwitched = false;
            EndMouseDrag();
            _dragOffset = PointF.Empty;
        }

        private void EndMouseDrag()
        {
            if (!_isDragging)
            {
                return;
            }

            _isDragging = false;
            _dragOffset = PointF.Empty;
            Function.Call((Hash)0x8DB8CFFD58B62552UL, 0);
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

        private static bool IsControlJustPressedIncludingDisabled(Control control)
        {
            return Game.IsControlJustPressed(control)
                || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)control);
        }

        private static bool IsControlJustReleasedIncludingDisabled(Control control)
        {
            return Game.IsControlJustReleased(control)
                || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, (int)control);
        }

        private static bool IsControlPressedIncludingDisabled(Control control)
        {
            return Game.IsControlPressed(control)
                || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)control);
        }

        private static float GetControlNormalIncludingDisabled(Control control)
        {
            if (Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Game.Player.Handle))
            {
                return Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, (int)control);
            }

            return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, (int)control);
        }

        public bool IsMouseInMenu()
        {
            PointF topLeft = SafeZone.GetSafePosition(new PointF(0f, 0f));
            SizeF size = new SizeF(431f, 550f);
            float cursorX = GetControlNormalIncludingDisabled(Control.CursorX) * (1080f * Function.Call<float>(Hash.GET_ASPECT_RATIO, false));
            float cursorY = GetControlNormalIncludingDisabled(Control.CursorY) * 1080f;

            return cursorX >= topLeft.X
                && cursorX <= topLeft.X + size.Width
                && cursorY > topLeft.Y
                && cursorY < topLeft.Y + size.Height;
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
            _renderedFocus = focus;
            _hasRenderedFocus = true;

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
            if (_isDragging && (!Helper.optEnableMouse
                || IsControlJustReleasedIncludingDisabled(Control.Attack)
                || !IsControlPressedIncludingDisabled(Control.Attack)))
            {
                EndMouseDrag();
            }

            if (_mainCamera == null)
            {
                return;
            }

            Game.DisableControlThisFrame(Control.VehicleMouseControlOverride);

            if (Helper.optEnableMouse && (IsLerping || _justSwitched))
            {
                GTA.UI.Hud.ShowCursorThisFrame();
            }

            if (_justSwitched && !IsLerping)
            {
                _justSwitched = false;
                if (!Helper.optEnableMouse || !IsControlJustPressedIncludingDisabled(Control.Attack) || IsMouseInMenu())
                {
                    return;
                }
            }

            if (Helper.optEnableMouse && IsControlJustPressedIncludingDisabled(Control.Attack) && !_isDragging && !IsMouseInMenu())
            {
                _isDragging = true;
                float mouseX = GetControlNormalIncludingDisabled(Control.CursorX);
                float mouseY = GetControlNormalIncludingDisabled(Control.CursorY);
                Function.Call((Hash)0x8DB8CFFD58B62552UL, 4);
                mouseX = (mouseX * 2f) - 1f;
                mouseY = (mouseY * 2f) - 1f;
                if (RotationMode == CameraRotationMode.FirstPerson)
                {
                    mouseY *= -1f;
                }
                _dragOffset = new PointF(mouseX, mouseY);
            }

            if (IsLerping)
            {
                float elapsed = (float)DateTime.UtcNow.Subtract(startTime).TotalMilliseconds;
                bool completed = elapsed >= duration;
                _mainCamera.Position = completed ? endValuePosition
                    : _curvedTransition ? GetCurvePosition(startValuePosition, _curveControlOne, _curveControlTwo, endValuePosition,
                        QuadraticEasing(elapsed, 0.0f, 1.0f, duration))
                    : LerpVector(elapsed, duration, startValuePosition, endValuePosition);
                if (_lerpAroundFocus)
                {
                    Vector3 focus = completed ? endValueFocus
                        : LerpVector(elapsed, duration, startValueFocus, endValueFocus);
                    if (IsFiniteVector(focus) && _mainCamera.Position.DistanceTo(focus) >= MinimumSafeCameraDistance)
                    {
                        _mainCamera.PointAt(focus);
                        _renderedFocus = focus;
                        _hasRenderedFocus = true;
                    }
                }
                else
                {
                    _mainCamera.Rotation = completed ? endValueRotation : LerpVector(elapsed, duration, startValueRotation, endValueRotation);
                }

                if (_isDragging)
                {
                    float mouseX = (GetControlNormalIncludingDisabled(Control.CursorX) * 2f) - 1f;
                    float mouseY = (GetControlNormalIncludingDisabled(Control.CursorY) * 2f) - 1f;
                    if (RotationMode == CameraRotationMode.FirstPerson)
                    {
                        mouseY *= -1f;
                    }

                    float deltaX = mouseX - _dragOffset.X;
                    float deltaY = mouseY - _dragOffset.Y;
                    Vector3 rotationDelta = Vector3.Zero;
                    if (!IsCameraClamped(true, deltaX))
                    {
                        rotationDelta.Z = deltaX * 20f;
                    }
                    if (!IsCameraClamped(false, deltaY))
                    {
                        rotationDelta.X = -deltaY * 20f;
                    }

                    if (!_lerpAroundFocus)
                    {
                        _mainCamera.Rotation += rotationDelta;
                        startValueRotation += rotationDelta;
                        endValueRotation += rotationDelta;
                    }
                    _dragOffset = new PointF(mouseX, mouseY);
                }

                if (completed)
                {
                    IsLerping = false;
                    _curvedTransition = false;
                    _justSwitched = false;
                    if (RotationMode == CameraRotationMode.Around)
                    {
                        _mainCamera.PointAt(_targetPos);
                        _renderedFocus = _targetPos;
                        _hasRenderedFocus = true;
                    }
                }
                return;
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

        private void OrbitAroundTarget(float horizontalInput, float verticalInput, float horizontalSensitivity, float verticalSensitivity)
        {
            if (_mainCamera == null || !IsFiniteVector(_targetPos))
            {
                return;
            }

            Vector3 displacement = _mainCamera.Position - _targetPos;
            float radius = displacement.Length();
            if (!IsFinite(radius) || radius < MinimumSafeCameraDistance)
            {
                return;
            }

            float horizontalDistance = (float)Math.Sqrt(displacement.X * displacement.X + displacement.Y * displacement.Y);
            float yaw = (float)Math.Atan2(displacement.X, displacement.Y);
            float elevation = (float)(Math.Atan2(displacement.Z, horizontalDistance) * (180.0 / Math.PI));
            if (!IsCameraClamped(true, horizontalInput))
            {
                yaw += horizontalInput * horizontalSensitivity * (float)(Math.PI / 180.0);
            }

            float minimumElevation = 3.0f;
            float maximumElevation = 65.0f;
            if (CameraClamp != null && CameraClamp.MaxVerticalValue < CameraClamp.MinVerticalValue)
            {
                minimumElevation = Math.Max(2.0f, -CameraClamp.MinVerticalValue);
                maximumElevation = Math.Min(80.0f, -CameraClamp.MaxVerticalValue);
            }

            elevation = Clamp(elevation + verticalInput * verticalSensitivity, minimumElevation, maximumElevation);
            float elevationRadians = elevation * (float)(Math.PI / 180.0);
            float horizontalRadius = radius * (float)Math.Cos(elevationRadians);
            Vector3 nextPosition = _targetPos + new Vector3(
                horizontalRadius * (float)Math.Sin(yaw),
                horizontalRadius * (float)Math.Cos(yaw),
                radius * (float)Math.Sin(elevationRadians));

            if (IsFiniteVector(nextPosition))
            {
                _mainCamera.Position = nextPosition;
                _mainCamera.PointAt(_targetPos);
                _renderedFocus = _targetPos;
                _hasRenderedFocus = true;
            }
        }

        private void UpdateAroundCamera(bool duringTransition = false)
        {
            if (_isDragging)
            {
                if (Helper.optEnableMouse)
                {
                    GTA.UI.Hud.ShowCursorThisFrame();
                }

                float mouseX = (GetControlNormalIncludingDisabled(Control.CursorX) * 2f) - 1f;
                float mouseY = (GetControlNormalIncludingDisabled(Control.CursorY) * 2f) - 1f;
                if (!duringTransition)
                {
                    float deltaX = mouseX - _dragOffset.X;
                    float deltaY = mouseY - _dragOffset.Y;
                    if (Math.Abs(deltaX) > 0.00001f || Math.Abs(deltaY) > 0.00001f)
                    {
                        OrbitAroundTarget(deltaX, deltaY, 90f, 85f);
                    }
                }
                _dragOffset = new PointF(mouseX, mouseY);
            }

            if (!duringTransition && Game.LastInputMethod == InputMethod.GamePad)
            {
                float horizontalInput = GetControlNormalIncludingDisabled(Control.LookLeftRight);
                float verticalInput = GetControlNormalIncludingDisabled(Control.LookUpDown);
                if (Math.Abs(horizontalInput) > 0.001f || Math.Abs(verticalInput) > 0.001f)
                {
                    OrbitAroundTarget(horizontalInput, verticalInput, 1.6f, 1.3f);
                }
            }
        }

        private void UpdateFirstPersonCamera(bool duringTransition = false)
        {
            if (_isDragging)
            {
                if (Helper.optEnableMouse)
                {
                    GTA.UI.Hud.ShowCursorThisFrame();
                }
                float mouseX = GetControlNormalIncludingDisabled(Control.CursorX);
                float mouseY = GetControlNormalIncludingDisabled(Control.CursorY);
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

            if (!duringTransition && Game.LastInputMethod == InputMethod.GamePad)
            {
                float mouseX = GetControlNormalIncludingDisabled(Control.LookLeftRight) * -1f;
                float mouseY = GetControlNormalIncludingDisabled(Control.LookUpDown);
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
            if (duration <= 0f)
            {
                return startValue + changeInValue;
            }

            float progress = Clamp(currentTime / duration, 0f, 1f);
            float easedProgress = progress * (0.45f + progress * (2.1f - 1.55f * progress));
            return startValue + changeInValue * easedProgress;
        }

        public static Vector3 LerpVector(float currentTime, float duration, Vector3 startValue, Vector3 destination)
        {
            return new Vector3(
                QuadraticEasing(currentTime, startValue.X, destination.X - startValue.X, duration),
                QuadraticEasing(currentTime, startValue.Y, destination.Y - startValue.Y, duration),
                QuadraticEasing(currentTime, startValue.Z, destination.Z - startValue.Z, duration));
        }

        private static float ProjectOnto(Vector3 value, Vector3 axis)
        {
            return value.X * axis.X + value.Y * axis.Y + value.Z * axis.Z;
        }

        private static bool ClipCurveSegment(float p, float q, ref float entering, ref float leaving)
        {
            if (Math.Abs(p) < 0.0001f)
            {
                return q >= 0.0f;
            }

            float intersection = q / p;
            if (p < 0.0f)
            {
                if (intersection > leaving) return false;
                if (intersection > entering) entering = intersection;
            }
            else
            {
                if (intersection < entering) return false;
                if (intersection < leaving) leaving = intersection;
            }
            return true;
        }

        private static bool CrossesVehicleBody(float startX, float startY, float startZ,
            float endX, float endY, float endZ, float halfWidth, float halfLength, float minimumHeight, float maximumHeight)
        {
            float entering = 0.0f;
            float leaving = 1.0f;
            float dx = endX - startX;
            float dy = endY - startY;
            float dz = endZ - startZ;
            return ClipCurveSegment(-dx, startX + halfWidth, ref entering, ref leaving)
                && ClipCurveSegment(dx, halfWidth - startX, ref entering, ref leaving)
                && ClipCurveSegment(-dy, startY + halfLength, ref entering, ref leaving)
                && ClipCurveSegment(dy, halfLength - startY, ref entering, ref leaving)
                && ClipCurveSegment(-dz, startZ - minimumHeight, ref entering, ref leaving)
                && ClipCurveSegment(dz, maximumHeight - startZ, ref entering, ref leaving);
        }

        private static Vector3 GetCurvePosition(Vector3 start, Vector3 controlOne, Vector3 controlTwo, Vector3 end, float progress)
        {
            float t = Clamp(progress, 0.0f, 1.0f);
            float inverse = 1.0f - t;
            return start * (inverse * inverse * inverse)
                + controlOne * (3.0f * inverse * inverse * t)
                + controlTwo * (3.0f * inverse * t * t)
                + end * (t * t * t);
        }

        private static bool TryMeasureCurve(Vector3 start, Vector3 controlOne, Vector3 controlTwo, Vector3 destination,
            Vector3 center, Vector3 right, Vector3 forward, Vector3 up, float halfWidth, float halfLength,
            float minimumHeight, float maximumHeight, out float length)
        {
            length = 0.0f;
            Vector3 previous = start;
            Vector3 previousOffset = previous - center;
            float previousX = ProjectOnto(previousOffset, right);
            float previousY = ProjectOnto(previousOffset, forward);
            float previousZ = ProjectOnto(previousOffset, up);
            for (int i = 1; i <= 24; i++)
            {
                Vector3 next = GetCurvePosition(start, controlOne, controlTwo, destination, i / 24.0f);
                Vector3 offset = next - center;
                float x = ProjectOnto(offset, right);
                float y = ProjectOnto(offset, forward);
                float z = ProjectOnto(offset, up);
                if (!IsFiniteVector(next) || CrossesVehicleBody(previousX, previousY, previousZ, x, y, z,
                    halfWidth, halfLength, minimumHeight, maximumHeight))
                {
                    return false;
                }

                length += next.DistanceTo(previous);
                previous = next;
                previousX = x;
                previousY = y;
                previousZ = z;
            }
            return true;
        }

        private bool PrepareCurvedTransition(Vector3 startPosition, Vector3 destination)
        {
            if (_target == null || !_target.Exists() || _internalCameraPosition == CameraPosition.Interior
                || !IsFiniteVector(startPosition) || !IsFiniteVector(destination))
            {
                return false;
            }

            Vector3 minimum;
            Vector3 maximum;
            try
            {
                _target.Model.GetDimensions(out minimum, out maximum);
            }
            catch
            {
                return false;
            }

            float width = maximum.X - minimum.X;
            float vehicleLength = maximum.Y - minimum.Y;
            if (!IsFiniteVector(minimum) || !IsFiniteVector(maximum)
                || width < 0.4f || width > 12.0f || vehicleLength < 1.0f || vehicleLength > 24.0f)
            {
                return false;
            }

            Vector3 center = _target.GetOffsetPosition(new Vector3(
                (minimum.X + maximum.X) * 0.5f, (minimum.Y + maximum.Y) * 0.5f, 0.0f));
            Vector3 right = _target.RightVector;
            Vector3 forward = _target.ForwardVector;
            Vector3 up = _target.UpVector;
            Vector3 startOffset = startPosition - center;
            Vector3 endOffset = destination - center;
            float startX = ProjectOnto(startOffset, right);
            float startY = ProjectOnto(startOffset, forward);
            float endX = ProjectOnto(endOffset, right);
            float endY = ProjectOnto(endOffset, forward);
            float halfWidth = width * 0.5f + 0.2f;
            float halfLength = vehicleLength * 0.5f + 0.2f;
            float minimumHeight = minimum.Z - 0.15f;
            float maximumHeight = maximum.Z + 0.15f;
            float startHeight = ProjectOnto(startOffset, up);
            float endHeight = ProjectOnto(endOffset, up);
            bool startInside = Math.Abs(startX) < halfWidth && Math.Abs(startY) < halfLength
                && startHeight > minimumHeight && startHeight < maximumHeight;
            bool endInside = Math.Abs(endX) < halfWidth && Math.Abs(endY) < halfLength
                && endHeight > minimumHeight && endHeight < maximumHeight;
            if (!CrossesVehicleBody(startX, startY, startHeight, endX, endY, endHeight,
                halfWidth, halfLength, minimumHeight, maximumHeight) || startInside || endInside)
            {
                return false;
            }

            float shortestLength = float.MaxValue;
            bool found = false;
            for (int axis = 0; axis < 2; axis++)
            {
                float halfGuide = axis == 0 ? halfWidth : halfLength;
                float halfOther = axis == 0 ? halfLength : halfWidth;
                float startGuide = axis == 0 ? startX : startY;
                float endGuide = axis == 0 ? endX : endY;
                float startOther = axis == 0 ? startY : startX;
                float endOther = axis == 0 ? endY : endX;
                bool extendStart = Math.Abs(startGuide) < halfGuide && Math.Abs(startOther) >= halfOther;
                bool extendEnd = Math.Abs(endGuide) < halfGuide && Math.Abs(endOther) >= halfOther;
                for (int side = -1; side <= 1; side += 2)
                {
                    for (int step = 0; step < 10; step++)
                    {
                        float guide = side * (halfGuide + 0.7f + step * 0.3f);
                        for (int extension = 0; extension < 7; extension++)
                        {
                            float lead = halfOther + 0.4f + extension * 0.45f;
                            float firstOther = extendStart
                                ? (startOther < 0.0f ? -1.0f : 1.0f) * Math.Max(Math.Abs(startOther), lead) : startOther;
                            float secondOther = extendEnd
                                ? (endOther < 0.0f ? -1.0f : 1.0f) * Math.Max(Math.Abs(endOther), lead) : endOther;
                            Vector3 controlOne = center + right * (axis == 0 ? guide : firstOther)
                                + forward * (axis == 0 ? firstOther : guide) + up * startHeight;
                            Vector3 controlTwo = center + right * (axis == 0 ? guide : secondOther)
                                + forward * (axis == 0 ? secondOther : guide) + up * endHeight;
                            float pathLength;
                            if (!TryMeasureCurve(startPosition, controlOne, controlTwo, destination,
                                center, right, forward, up, halfWidth, halfLength, minimumHeight, maximumHeight, out pathLength))
                            {
                                continue;
                            }

                            if (pathLength < shortestLength)
                            {
                                shortestLength = pathLength;
                                _curveControlOne = controlOne;
                                _curveControlTwo = controlTwo;
                                found = true;
                            }
                            break;
                        }
                    }
                }
            }
            return found;
        }

        private void StartLerp(Vector3 endPosition, Vector3 endRotation)
        {
            if (_mainCamera == null)
            {
                return;
            }

            EndMouseDrag();
            Vector3 currentRotation = _mainCamera.Rotation;
            Vector3 currentPosition = _mainCamera.Position;
            _lerpAroundFocus = RotationMode == CameraRotationMode.Around
                && IsFiniteVector(_targetPos) && IsFiniteVector(endPosition);
            if (_lerpAroundFocus)
            {
                endValueFocus = _targetPos;
                if (_hasRenderedFocus && IsFiniteVector(_renderedFocus))
                {
                    startValueFocus = _renderedFocus;
                }
                else
                {
                    float focusDistance = Clamp(currentPosition.DistanceTo(endValueFocus), 1.0f, 8.0f);
                    startValueFocus = currentPosition + CutsceneManager.RotationToDirection(currentRotation) * focusDistance;
                }
            }
            _mainCamera.StopPointing();
            _mainCamera.Rotation = currentRotation;
            startValueRotation = currentRotation;
            startValuePosition = _mainCamera.Position;
            _curvedTransition = _lerpAroundFocus && PrepareCurvedTransition(startValuePosition, endPosition);
            duration = _curvedTransition ? 1400.0f : 1000.0f;
            IsLerping = true;
            startTime = DateTime.UtcNow;
            endValuePosition = endPosition;
            while (endRotation.Z - currentRotation.Z > 180f)
            {
                endRotation.Z -= 360f;
            }
            while (endRotation.Z - currentRotation.Z < -180f)
            {
                endRotation.Z += 360f;
            }
            endValueRotation = endRotation;
        }

        private void SetAroundCamera(Vector3 targetPos, float zoom, Vector3 endPosition, Vector3 endRotation, CameraClamp clamp)
        {
            Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 255, false);
            RotationMode = CameraRotationMode.Around;
            _targetPos = targetPos;
            _cameraZoom = zoom;
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

        private void SetRoofCamera(CameraPosition position)
        {
            Vector3 minimumDimensions;
            Vector3 maximumDimensions;
            _target.Model.GetDimensions(out minimumDimensions, out maximumDimensions);

            if (!IsFiniteVector(minimumDimensions) || !IsFiniteVector(maximumDimensions)
                || maximumDimensions.Y <= minimumDimensions.Y || maximumDimensions.X <= minimumDimensions.X)
            {
                Vector3 fallbackRoofFocus = _target.Position + _target.UpVector * 1.25f;
                Vector3 fallbackRoofPosition = _target.Position - _target.RightVector * 3.2f - _target.ForwardVector * 1.85f + _target.UpVector * 2.45f;
                SetAroundCamera(fallbackRoofFocus, fallbackRoofFocus.DistanceTo(fallbackRoofPosition), fallbackRoofPosition,
                    GetStableLookRotation(fallbackRoofFocus - fallbackRoofPosition, _mainCamera.Rotation.Z),
                    new CameraClamp { MaxVerticalValue = -65.0f, MinVerticalValue = -5.0f });
                return;
            }

            float length = maximumDimensions.Y - minimumDimensions.Y;
            float width = maximumDimensions.X - minimumDimensions.X;
            float roofY = minimumDimensions.Y + length * (position == CameraPosition.FrontRoof ? 0.63f
                : position == CameraPosition.RearRoof ? 0.37f : 0.5f);
            float roofZ = maximumDimensions.Z - 0.12f;
            Vector3 focus = _target.GetOffsetPosition(new Vector3(0f, roofY, roofZ));
            float sideDistance = Math.Max(2.55f, width * 0.96f + 1.12f);
            if (maximumDimensions.Z > 2.0f)
            {
                sideDistance += Math.Min(0.9f, (maximumDimensions.Z - 2.0f) * 0.65f);
            }
            float lengthDistance = Math.Max(1.48f, Math.Min(2.5f, length * 0.31f));
            float cameraY = roofY + (position == CameraPosition.FrontRoof ? lengthDistance
                : position == CameraPosition.RearRoof ? -lengthDistance : -0.75f);
            float cameraZ = Math.Min(maximumDimensions.Z + (maximumDimensions.Z > 2.0f ? 0.25f : 1.05f), maximumDimensions.Z > 2.0f ? 2.25f : 2.0f);
            Vector3 cameraPosition = _target.GetOffsetPosition(new Vector3(-sideDistance, cameraY, cameraZ));
            SetAroundCamera(focus, focus.DistanceTo(cameraPosition), cameraPosition,
                GetStableLookRotation(focus - cameraPosition, _mainCamera.Rotation.Z),
                new CameraClamp { MaxVerticalValue = -65.0f, MinVerticalValue = -5.0f });
        }

        private void SetSpoilerCamera()
        {
            Vector3 minimumDimensions;
            Vector3 maximumDimensions;
            _target.Model.GetDimensions(out minimumDimensions, out maximumDimensions);

            if (!IsFiniteVector(minimumDimensions) || !IsFiniteVector(maximumDimensions)
                || maximumDimensions.Y <= minimumDimensions.Y || maximumDimensions.X <= minimumDimensions.X)
            {
                Vector3 fallbackSpoilerFocus = _target.Position + _target.UpVector * 1.0f;
                Vector3 fallbackSpoilerPosition = _target.Position - _target.ForwardVector * 3.3f - _target.RightVector * 1.9f + _target.UpVector * 1.5f;
                SetAroundCamera(fallbackSpoilerFocus, fallbackSpoilerFocus.DistanceTo(fallbackSpoilerPosition), fallbackSpoilerPosition,
                    GetStableLookRotation(fallbackSpoilerFocus - fallbackSpoilerPosition, _mainCamera.Rotation.Z),
                    new CameraClamp { MaxVerticalValue = -60.0f, MinVerticalValue = -5.0f });
                return;
            }

            float length = maximumDimensions.Y - minimumDimensions.Y;
            float width = maximumDimensions.X - minimumDimensions.X;
            float focusY = minimumDimensions.Y + Math.Min(0.75f, length * 0.18f);
            float focusZ = maximumDimensions.Z - Math.Min(0.3f, (maximumDimensions.Z - minimumDimensions.Z) * 0.2f);
            Vector3 focus = _target.GetOffsetPosition(new Vector3(0f, focusY, focusZ));
            float sideDistance = Math.Max(1.5f, width * 0.8f + 0.95f);
            float rearDistance = Math.Max(0.65f, Math.Min(1.9f, length * 0.28f));
            float cameraHeightOffset = Math.Min(0.35f, Math.Max(0.05f, (maximumDimensions.Z - minimumDimensions.Z) * 0.1f));
            Vector3 cameraPosition = _target.GetOffsetPosition(new Vector3(-sideDistance, minimumDimensions.Y - rearDistance, maximumDimensions.Z + cameraHeightOffset));
            SetAroundCamera(focus, focus.DistanceTo(cameraPosition), cameraPosition,
                GetStableLookRotation(focus - cameraPosition, _mainCamera.Rotation.Z),
                new CameraClamp { MaxVerticalValue = -30.0f, MinVerticalValue = -2.0f });
        }

        private void SetHeadlightCamera()
        {
            Vector3 headlightPosition;
            if (!TryGetAverageBonePosition(_target, "headlight_l", "headlight_r", out headlightPosition)
                && !TryGetUsableBonePosition(_target, "headlight", out headlightPosition)
                && !TryGetUsableBonePosition(_target, "bumper_f", out headlightPosition))
            {
                Vector3 minimumDimensions;
                Vector3 maximumDimensions;
                _target.Model.GetDimensions(out minimumDimensions, out maximumDimensions);
                float frontOffset = IsFiniteVector(maximumDimensions) && maximumDimensions.Y > 0.1f && maximumDimensions.Y < 10.0f
                    ? maximumDimensions.Y - 0.2f : 1.4f;
                headlightPosition = _target.Position + _target.ForwardVector * frontOffset + _target.UpVector * 0.5f;
            }

            float focusForwardOffset = HeadlightCameraForwardOffset + HeadlightCameraRightOffset
                / (float)Math.Tan(HeadlightCameraAngleDegrees * Math.PI / 180.0);
            Vector3 focus = headlightPosition + _target.ForwardVector * focusForwardOffset
                + _target.UpVector * HeadlightFocusHeightOffset;
            Vector3 position = headlightPosition + _target.ForwardVector * HeadlightCameraForwardOffset
                + _target.RightVector * HeadlightCameraRightOffset + _target.UpVector * HeadlightCameraHeightOffset;
            SetAroundCamera(focus, focus.DistanceTo(position), position,
                GetStableLookRotation(focus - position, _mainCamera.Rotation.Z),
                new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f });
        }

        private void SetRollCageCamera()
        {
            Vector3 minimum;
            Vector3 maximum;
            _target.Model.GetDimensions(out minimum, out maximum);
            bool hasDimensions = IsFiniteVector(minimum) && IsFiniteVector(maximum)
                && maximum.X > minimum.X && maximum.Y > minimum.Y && maximum.Z > minimum.Z;

            Vector3 focus;
            if (TryGetAverageBonePosition(_target, "seat_dside_f", "seat_pside_f", out focus))
            {
                focus += _target.UpVector * 0.55f;
            }
            else if (hasDimensions)
            {
                focus = _target.GetOffsetPosition(new Vector3(0f, minimum.Y + (maximum.Y - minimum.Y) * 0.58f,
                    minimum.Z + (maximum.Z - minimum.Z) * 0.78f));
            }
            else
            {
                focus = _target.Position + _target.UpVector * 1.05f;
            }

            float sideDistance = hasDimensions
                ? Math.Max(2.75f, (maximum.X - minimum.X) * 0.65f + 1.25f)
                : 2.85f;
            Vector3 position = focus + _target.ForwardVector * 1.35f
                - _target.RightVector * sideDistance + _target.UpVector * 0.4f;
            SetAroundCamera(focus, focus.DistanceTo(position), position,
                GetStableLookRotation(focus - position, _mainCamera.Rotation.Z),
                new CameraClamp { MaxVerticalValue = -55.0f, MinVerticalValue = -3.0f });
        }

        private void SetEngineBaySideCamera()
        {
            Vector3 focus;
            if (!TryGetUsableBonePosition(_target, "engine", out focus))
            {
                focus = _target.Position + _target.ForwardVector * 0.95f + _target.UpVector * 0.7f;
            }

            Vector3 minimum;
            Vector3 maximum;
            _target.Model.GetDimensions(out minimum, out maximum);
            float sideDistance = IsFiniteVector(minimum) && IsFiniteVector(maximum) && maximum.X > minimum.X
                ? Math.Max(2.5f, (maximum.X - minimum.X) * 0.65f + 1.15f)
                : 2.65f;
            Vector3 position = focus - _target.RightVector * sideDistance
                + _target.ForwardVector * 0.25f + _target.UpVector * 1.55f;
            SetAroundCamera(focus, focus.DistanceTo(position), position,
                GetStableLookRotation(focus - position, _mainCamera.Rotation.Z),
                new CameraClamp { MaxVerticalValue = -65.0f, MinVerticalValue = -5.0f });
        }

        private void OnCameraChange(CameraPosition newPos)
        {
            if (_mainCamera == null || _target == null || !_target.Exists())
            {
                return;
            }

            EndMouseDrag();
            switch (newPos)
            {
                case CameraPosition.Headlights:
                    SetHeadlightCamera();
                    break;
                case CameraPosition.RollCage:
                    SetRollCageCamera();
                    break;
                case CameraPosition.EngineBaySide:
                    SetEngineBaySideCamera();
                    break;
                case CameraPosition.Spoiler:
                    SetSpoilerCamera();
                    break;
                case CameraPosition.Roof:
                case CameraPosition.FrontRoof:
                case CameraPosition.RearRoof:
                    SetRoofCamera(newPos);
                    break;
                case CameraPosition.Car:
                    Game.Player.Character.Opacity = 255;
                    RotationMode = CameraRotationMode.Around;

                    _targetPos = GetDefaultTargetPosition(_target);

                    Vector3 newCamPos = GetDefaultCameraPosition(_target);
                    _cameraZoom = _targetPos.DistanceTo(newCamPos);
                    CameraClamp = new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f };
                    _mainCamera.Shake(CameraShake.Hand, 0.5f);

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
                    _curvedTransition = false;
                    Vector3 headPos = Game.Player.Character.Bones[Bone.IKHead].Position;
                    ResetShopDepthOfField();
                    Camera.DeleteAllCameras();
                    _mainCamera = CreateScriptedCamera(headPos + new Vector3(0f, 0f, 0.1f), new Vector3(0f, 0f, _target.Heading), GameplayCamera.FieldOfView);
                    StartRenderingCamera(_mainCamera);
                    _targetPos = headPos;
                    _hasRenderedFocus = false;
                    _lerpAroundFocus = false;
                    RotationMode = CameraRotationMode.FirstPerson;
                    Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 0, false);
                    CameraClamp = new CameraClamp { MaxVerticalValue = -60.0f, MinVerticalValue = -3.0f };
                    _justSwitched = true;
                    break;
                case CameraPosition.Engine:
                    SetAroundCamera(GetBonePosition(_target, "engine"), 3.0f, _targetPos + _target.ForwardVector * 3.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.Hood:
                    SetAroundCamera(GetBonePosition(_target, "bonnet"), 4.0f, _targetPos + _target.ForwardVector * 4.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.Boost:
                    _targetPos = _target.Position;
                    SetAroundCamera(_targetPos, 1.5f, _targetPos + _target.ForwardVector * -2.0f + _target.UpVector, new Vector3(0f, 0f, _target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.Exhaust:
                    _targetPos = _target.Position - _target.ForwardVector * 0.65f + _target.UpVector * 0.2f;
                    Vector3 exhaustCameraPosition = _target.Position - _target.ForwardVector * 4.25f - _target.RightVector * 2.75f + _target.UpVector * 1.15f;
                    SetAroundCamera(_targetPos, _targetPos.DistanceTo(exhaustCameraPosition), exhaustCameraPosition, GetStableLookRotation(_targetPos - exhaustCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.FrontTrunk:
                    _targetPos = GetBonePosition(_target, "boot");
                    SetAroundCamera(_targetPos, 3.0f, _targetPos + _target.ForwardVector * 3.0f + _target.UpVector, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
                case CameraPosition.FrontMuguard:
                    if (!TryGetUsableBonePosition(_target, "misc_i", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "forks_l", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "wheel_lf", out _targetPos))
                    {
                        _targetPos = _target.Position + _target.ForwardVector * 0.85f + _target.UpVector * 0.25f;
                    }

                    Vector3 frontMudguardCameraPosition = _targetPos + _target.ForwardVector * 2.35f + _target.RightVector * 0.45f + _target.UpVector * 0.55f;
                    SetAroundCamera(_targetPos, _targetPos.DistanceTo(frontMudguardCameraPosition), frontMudguardCameraPosition, GetStableLookRotation(_targetPos - frontMudguardCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
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
                    if (!TryGetUsableBonePosition(_target, "misc_d", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "wheel_lr", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "wheel_r", out _targetPos))
                    {
                        _targetPos = _target.Position - _target.ForwardVector * 0.85f + _target.UpVector * 0.3f;
                    }

                    Vector3 rearMudguardCameraPosition = _targetPos - _target.ForwardVector * 2.35f - _target.RightVector * 0.35f + _target.UpVector * 0.55f;
                    SetAroundCamera(_targetPos, _targetPos.DistanceTo(rearMudguardCameraPosition), rearMudguardCameraPosition, GetStableLookRotation(_targetPos - rearMudguardCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.BikeBackrest:
                    if (!TryGetUsableBonePosition(_target, "seat_dside_r", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "seat_pside_r", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "seat_dside_f", out _targetPos))
                    {
                        _targetPos = _target.Position - _target.ForwardVector * 0.45f + _target.UpVector * 0.75f;
                    }
                    else
                    {
                        _targetPos += _target.UpVector * 0.3f;
                    }

                    Vector3 backrestCameraPosition = _targetPos - _target.ForwardVector * 2.45f + _target.RightVector * 0.35f + _target.UpVector * 0.35f;
                    SetAroundCamera(_targetPos, _targetPos.DistanceTo(backrestCameraPosition), backrestCameraPosition, GetStableLookRotation(_targetPos - backrestCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.BikeWindshield:
                    if (!TryGetUsableBonePosition(_target, "windscreen", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "headlight_l", out _targetPos)
                        && !TryGetUsableBonePosition(_target, "headlight_r", out _targetPos))
                    {
                        _targetPos = _target.Position + _target.ForwardVector * 0.55f + _target.UpVector * 0.8f;
                    }
                    else
                    {
                        _targetPos += _target.UpVector * 0.2f;
                    }

                    Vector3 windshieldCameraPosition = _targetPos + _target.ForwardVector * 2.25f + _target.RightVector * 0.3f + _target.UpVector * 0.25f;
                    SetAroundCamera(_targetPos, _targetPos.DistanceTo(windshieldCameraPosition), windshieldCameraPosition, GetStableLookRotation(_targetPos - windshieldCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
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
                    if (_target.Bones.Contains("platelight"))
                    {
                        _targetPos = GetBonePosition(_target, "platelight");
                    }
                    else if (_target.Bones.Contains("neon_b"))
                    {
                        _targetPos = GetBonePosition(_target, "neon_b");
                    }
                    else if (_target.Bones.Contains("bumper_r"))
                    {
                        _targetPos = GetBonePosition(_target, "bumper_r") + _target.UpVector * 0.15f;
                    }
                    else
                    {
                        _targetPos = _target.Position - _target.ForwardVector * 0.75f + _target.UpVector * 0.45f;
                    }

                    Vector3 backPlateCameraPosition = _targetPos - _target.ForwardVector * 1.65f + _target.UpVector * 0.05f;
                    SetAroundCamera(_targetPos, _targetPos.DistanceTo(backPlateCameraPosition), backPlateCameraPosition, GetStableLookRotation(_targetPos - backPlateCameraPosition, _mainCamera.Rotation.Z), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 60.0f, RightHorizontalValue = _target.Heading - 300.0f });
                    break;
                case CameraPosition.FrontPlate:
                    _targetPos = GetBonePosition(_target, "neon_f");
                    SetAroundCamera(_targetPos, 1.0f, _targetPos + _target.ForwardVector * 2.0f + _target.UpVector * 2.0f, new Vector3(0f, 0f, -_target.Heading), new CameraClamp { MaxVerticalValue = -40.0f, MinVerticalValue = -3.0f, LeftHorizontalValue = _target.Heading - 250.6141f, RightHorizontalValue = _target.Heading - 105.30705f });
                    break;
            }
        }
    }
}
