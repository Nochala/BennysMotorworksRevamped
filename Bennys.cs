using System;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using static BennysMotorworksRevamped.Helper;
using static BennysMotorworksRevamped.MenuHelper;

namespace BennysMotorworksRevamped
{
    public class Bennys : Script
    {
        private const int StartupNativeDelayMs = 1000;
        private const int StartupNativeRetryDelayMs = 500;
        private const int PlayerInteriorProbeDelayMs = 250;
        private const float PlayerInteriorProbeRadius = 100.0f;
        private static readonly Vector3 WorkshopInteriorPosition = new Vector3(-211.798f, -1324.292f, 30.37535f);

        private const int BennysGarageDoorModel = unchecked((int)0xE684E276U);
        private static readonly Vector3 BennysGarageDoorPosition =
            new Vector3(-205.6828f, -1310.683f, 30.29572f);

        private const float BennysGarageDoorOpeningDistance = 14.0f;
        private const float BennysGarageDoorRetryDistance = 25.0f;
        private const float BennysGarageDoorInitialCloseDistance = 65.0f;
        private const float BennysGarageDoorModelPreloadDistance = 100.0f;
        private const float BennysGarageDoorModelReleaseDistance = 125.0f;

        private const float BennysGarageDoorInteriorDirX = -0.40987134f;
        private const float BennysGarageDoorInteriorDirY = -0.91214335f;
        private const float BennysGarageDoorInsideCloseDepth = 4.0f;
        private const float BennysGarageDoorInsideExitAssistDistance = 18.0f;
        private const float BennysGarageDoorOutsideResetDepth = -1.5f;
        private const float BennysGarageDoorDirectionEpsilon = 0.03f;

        private const int BennysGarageDoorStateProbeMs = 2000;
        private const int BennysGarageDoorUnavailableRetryMs = 250;
        private const int BennysGarageDoorModelRetryMs = 500;
        private const int BennysGarageDoorUpdateMs = 50;
        private const int BennysGarageDoorOpeningDurationMs = 2000;
        private const int WantedGarageClearConfirmMs = 600;

        private const ulong RequestModelNative = 0x963D27A58DF860ACUL;
        private const ulong HasModelLoadedNative = 0x98A4EB5D89A0C952UL;
        private const ulong SetModelAsNoLongerNeededNative = 0xE532F5D78798DAABUL;
        private const ulong SetLockedUnstreamedInDoorOfTypeNative = 0x9B12F9A24FABEDB0UL;
        private const ulong DoorSystemFindExistingDoorNative = 0x589F80B325CC82C5UL;
        private const ulong DoorSystemGetIsPhysicsLoadedNative = 0xDF97CDD4FC08FD34UL;
        private const ulong DoorSystemGetDoorStateNative = 0x160AA1B32F6139B8UL;
        private const ulong DoorSystemGetOpenRatioNative = 0x65499865FCA6E5ECUL;
        private const ulong DoorSystemSetDoorStateNative = 0x6BAB9442830C7F53UL;
        private const ulong DoorSystemSetOpenRatioNative = 0xB6E6FBA95C7324ACUL;
        private const ulong DoorSystemSetHoldOpenNative = 0xD9B71952F78A2640UL;
        private const ulong IsDoorRegisteredWithSystemNative = 0xC153C43EA202C8C1UL;

        private const ulong IsPauseMenuActiveNative = 0xB0034A223497FFCBUL;
        private const ulong IsScreenFadingOutNative = 0x797AC7CB535BA28FUL;
        private const ulong IsScreenFadedOutNative = 0xB16FCE9DDC7BA182UL;
        private const ulong IsScreenFadingInNative = 0x5C544BC6C57AC575UL;
        private const int GarageDoorPostTransitionSettleMs = 750;

        private int _garageDoorNextAttempt;
        private int _garageDoorModelNextRequest;
        private int _garageDoorNextUpdate;
        private int _garageDoorNextStateProbe;
        private int _garageDoorSystemHash;
        private bool _garageDoorClosedApplied;
        private bool _garageDoorUnstreamedClosedApplied;
        private bool _garageDoorAssistActive;
        private bool _garageDoorHoldOpen;
        private bool _garageDoorUnlocked;
        private float _garageDoorOpeningRatio;
        private int _garageDoorOpeningLastUpdate;
        private bool _garageDoorModelRequested;
        private bool _garageDoorInsideLatched;
        private bool _garageDoorInsideExitOpening;
        private bool _garageDoorDepthSampleValid;
        private float _garageDoorLastInteriorDepth;
        private int _wantedGarageClearSince = -1;
        private bool _garageDoorPoliceBlocked;
        private bool _garageDoorWorldTransitionActive;
        private int _garageDoorWorldStableSince;
        private int _garageDoorLastObservedGameTime;
        private bool _startupNativeInitializationScheduled;
        private bool _startupNativeInitialized;
        private int _startupNativeInitializeAt;
        private int _nextPlayerInteriorProbeTime;
        private int _cachedPlayerInteriorId;
        private bool _wantedWorkshopWaiting;
        private int _wantedWorkshopWaitSince;
        private int _wantedWorkshopWaitLevel;

        public Bennys()
        {
            Tick += OnTick;
            Aborted += OnAborted;

            LoadSettings();
            Logger.Initialize();
            Logger.Debug("Debug logging active. Writing to BennysMotorworksRevamped.log.");
            LogCompatibilityProfile();
            Logger.Log("Bennys startup queued.");
        }

        private bool EnsureGameNativeStartupInitialized()
        {
            if (_startupNativeInitialized)
            {
                return true;
            }

            if (!_startupNativeInitializationScheduled)
            {
                _startupNativeInitializationScheduled = true;
                _startupNativeInitializeAt = Game.GameTime + StartupNativeDelayMs;
                return false;
            }

            if (Game.GameTime < _startupNativeInitializeAt)
            {
                return false;
            }

            try
            {
                Ped player = Game.Player.Character;
                if (player == null || !player.Exists())
                {
                    _startupNativeInitializeAt = Game.GameTime + StartupNativeRetryDelayMs;
                    return false;
                }

                int detectedInteriorId = Helper.GetInteriorID(WorkshopInteriorPosition);
                if (detectedInteriorId != 0)
                {
                    bennyIntID = detectedInteriorId;
                }

                if (BennysBlip == null || !BennysBlip.Exists())
                {
                    CreateBlip();
                }

                _startupNativeInitialized = true;
                Logger.Log("Bennys initialized. Game-native startup completed. interiorId=" + bennyIntID);
                return true;
            }
            catch (Exception ex)
            {
                _startupNativeInitializeAt = Game.GameTime + StartupNativeRetryDelayMs;
                Logger.Debug("Bennys game-native startup deferred. " + ex.Message);
                return false;
            }
        }

        private int GetCurrentPlayerInteriorId(bool isMenuVisible)
        {
            if (ply == null)
            {
                return 0;
            }

            bool shouldProbe = isCutscene
                || isMenuVisible
                || ply.Position.DistanceTo(WorkshopInteriorPosition) <= PlayerInteriorProbeRadius;

            if (!shouldProbe)
            {
                _cachedPlayerInteriorId = 0;
                _nextPlayerInteriorProbeTime = 0;
                return 0;
            }

            if (Game.GameTime >= _nextPlayerInteriorProbeTime)
            {
                _cachedPlayerInteriorId = GetInteriorID(ply.Position);
                _nextPlayerInteriorProbeTime = Game.GameTime + PlayerInteriorProbeDelayMs;
            }

            return _cachedPlayerInteriorId;
        }

        private static bool IsPauseMenuActive()
        {
            try
            {
                return Function.Call<bool>((Hash)IsPauseMenuActiveNative);
            }
            catch
            {
                return true;
            }
        }

        private static bool IsGarageDoorWorldTransitionActive()
        {
            try
            {
                return Function.Call<bool>((Hash)IsScreenFadingOutNative)
                    || Function.Call<bool>((Hash)IsScreenFadedOutNative)
                    || Function.Call<bool>((Hash)IsScreenFadingInNative);
            }
            catch
            {
                return true;
            }
        }

        private void ResetBennysGarageDoorManagedState()
        {
            _garageDoorNextAttempt = 0;
            _garageDoorModelNextRequest = 0;
            _garageDoorNextUpdate = 0;
            _garageDoorNextStateProbe = 0;
            _garageDoorSystemHash = 0;
            _garageDoorClosedApplied = false;
            _garageDoorUnstreamedClosedApplied = false;
            _garageDoorAssistActive = false;
            _garageDoorHoldOpen = false;
            _garageDoorUnlocked = false;
            _garageDoorOpeningRatio = 0.0f;
            _garageDoorOpeningLastUpdate = 0;
            _garageDoorModelRequested = false;
            _garageDoorInsideLatched = false;
            _garageDoorInsideExitOpening = false;
            _garageDoorDepthSampleValid = false;
            _garageDoorLastInteriorDepth = 0.0f;
            _wantedGarageClearSince = -1;
            _garageDoorPoliceBlocked = false;
        }

        private bool CanRunBennysGarageDoorRecovery(int now)
        {
            if (now < _garageDoorLastObservedGameTime)
            {
                _garageDoorWorldTransitionActive = true;
                _garageDoorWorldStableSince = 0;
                ResetBennysGarageDoorManagedState();
            }
            _garageDoorLastObservedGameTime = now;

            if (IsPauseMenuActive() || IsGarageDoorWorldTransitionActive())
            {
                if (!_garageDoorWorldTransitionActive)
                {
                    _garageDoorWorldTransitionActive = true;
                    ResetBennysGarageDoorManagedState();
                }

                _garageDoorWorldStableSince = 0;
                return false;
            }

            if (_garageDoorWorldTransitionActive)
            {
                _garageDoorWorldTransitionActive = false;
                _garageDoorWorldStableSince = now;
                return false;
            }

            if (_garageDoorWorldStableSince != 0)
            {
                if (now - _garageDoorWorldStableSince < GarageDoorPostTransitionSettleMs)
                {
                    return false;
                }

                _garageDoorWorldStableSince = 0;
            }

            return true;
        }

        private void RequestBennysGarageDoorModel()
        {
            Function.Call((Hash)RequestModelNative, BennysGarageDoorModel);
            _garageDoorModelRequested = true;
        }

        private bool IsBennysGarageDoorModelLoaded()
        {
            return Function.Call<bool>((Hash)HasModelLoadedNative, BennysGarageDoorModel);
        }

        private void ReleaseBennysGarageDoorModel()
        {
            if (!_garageDoorModelRequested)
            {
                return;
            }

            Function.Call((Hash)SetModelAsNoLongerNeededNative, BennysGarageDoorModel);
            _garageDoorModelRequested = false;
            _garageDoorModelNextRequest = 0;
        }

        private static void SetBennysGarageDoorUnstreamedLocked(bool locked)
        {
            Function.Call(
                (Hash)SetLockedUnstreamedInDoorOfTypeNative,
                BennysGarageDoorModel,
                BennysGarageDoorPosition.X,
                BennysGarageDoorPosition.Y,
                BennysGarageDoorPosition.Z,
                locked ? 1 : 0,
                0.0f,
                50.0f,
                0.0f);
        }

        private static void SetBennysGarageDoorUnstreamedOpen()
        {
            SetBennysGarageDoorUnstreamedLocked(false);
        }

        private static void SetBennysGarageDoorUnstreamedClosed()
        {
            SetBennysGarageDoorUnstreamedLocked(true);
        }

        private static bool TryFindBennysGarageDoor(out int doorHash)
        {
            doorHash = 0;

            OutputArgument resultDoorHash = new OutputArgument();
            bool found = Function.Call<bool>(
                (Hash)DoorSystemFindExistingDoorNative,
                BennysGarageDoorPosition.X,
                BennysGarageDoorPosition.Y,
                BennysGarageDoorPosition.Z,
                BennysGarageDoorModel,
                resultDoorHash);

            if (!found)
            {
                return false;
            }

            doorHash = resultDoorHash.GetResult<int>();
            return doorHash != 0;
        }

        private static bool IsBennysGarageDoorPhysicsLoaded(int doorHash)
        {
            return doorHash != 0
                && Function.Call<bool>((Hash)IsDoorRegisteredWithSystemNative, doorHash)
                && Function.Call<bool>((Hash)DoorSystemGetIsPhysicsLoadedNative, doorHash);
        }

        private static void SetBennysGarageDoorState(int doorHash, int state)
        {
            if (doorHash == 0)
            {
                return;
            }

            Function.Call(
                (Hash)DoorSystemSetDoorStateNative,
                doorHash,
                state,
                0,
                1);
        }

        private static void SetBennysGarageDoorOpenRatio(int doorHash, float ratio)
        {
            if (doorHash == 0)
            {
                return;
            }

            Function.Call(
                (Hash)DoorSystemSetOpenRatioNative,
                doorHash,
                ratio,
                0,
                1);
        }

        private static bool IsBennysGarageDoorStillClosed(int doorHash)
        {
            return Function.Call<int>((Hash)DoorSystemGetDoorStateNative, doorHash) == 1
                && Math.Abs(Function.Call<float>((Hash)DoorSystemGetOpenRatioNative, doorHash)) <= 0.05f;
        }

        private static bool IsBennysGarageDoorStillOpen(int doorHash)
        {
            return Function.Call<int>((Hash)DoorSystemGetDoorStateNative, doorHash) == 0
                && Function.Call<float>((Hash)DoorSystemGetOpenRatioNative, doorHash) >= 0.95f;
        }

        private static void SetBennysGarageDoorHoldOpen(int doorHash, bool holdOpen)
        {
            if (doorHash == 0)
            {
                return;
            }

            Function.Call(
                (Hash)DoorSystemSetHoldOpenNative,
                doorHash,
                holdOpen);
        }

        private static bool IsCurrentBennysGarageDoorVehicleAllowed()
        {
            Ped player = Game.Player.Character;
            if (player == null || !player.Exists())
            {
                return false;
            }

            Vehicle currentVehicle = player.CurrentVehicle;
            return Helper.IsWorkshopVehicleAllowed(currentVehicle);
        }

        private static bool TryGetBennysGarageDoorPlayerGeometry(
            out float distanceSquared,
            out float interiorDepth)
        {
            distanceSquared = 0.0f;
            interiorDepth = 0.0f;

            Ped player = Game.Player.Character;
            if (player == null || !player.Exists())
            {
                return false;
            }

            Vector3 playerPosition = player.Position;
            float dx = playerPosition.X - BennysGarageDoorPosition.X;
            float dy = playerPosition.Y - BennysGarageDoorPosition.Y;
            float dz = playerPosition.Z - BennysGarageDoorPosition.Z;

            distanceSquared = (dx * dx) + (dy * dy) + (dz * dz);
            interiorDepth =
                (dx * BennysGarageDoorInteriorDirX)
                + (dy * BennysGarageDoorInteriorDirY);

            return true;
        }

        private void MaintainBennysGarageDoorUnstreamedFallback()
        {
            if (!_garageDoorUnstreamedClosedApplied)
            {
                SetBennysGarageDoorUnstreamedClosed();
                _garageDoorUnstreamedClosedApplied = true;
            }
        }

        private void ReleaseBennysGarageDoorRecovery()
        {
            float distanceSquared;
            float interiorDepth;
            bool isDoorVicinity = TryGetBennysGarageDoorPlayerGeometry(
                out distanceSquared, out interiorDepth)
                && distanceSquared <= BennysGarageDoorInitialCloseDistance
                    * BennysGarageDoorInitialCloseDistance;

            int liveHash;
            if (isDoorVicinity
                && _garageDoorSystemHash != 0
                && TryFindBennysGarageDoor(out liveHash)
                && liveHash == _garageDoorSystemHash
                && IsBennysGarageDoorPhysicsLoaded(liveHash))
            {
                if (_garageDoorHoldOpen)
                {
                    SetBennysGarageDoorHoldOpen(liveHash, false);
                }

                SetBennysGarageDoorOpenRatio(liveHash, 0.0f);
                SetBennysGarageDoorState(liveHash, 1);
            }

            if (isDoorVicinity && !_garageDoorUnstreamedClosedApplied)
            {
                SetBennysGarageDoorUnstreamedClosed();
            }

            _garageDoorSystemHash = 0;
            _garageDoorAssistActive = false;
            _garageDoorHoldOpen = false;
            _garageDoorUnlocked = false;
            _garageDoorOpeningRatio = 0.0f;
            _garageDoorOpeningLastUpdate = 0;
            _garageDoorClosedApplied = false;
            _garageDoorUnstreamedClosedApplied = true;
            _garageDoorNextStateProbe = 0;
            _garageDoorNextAttempt = 0;
        }

        private void MaintainBennysGarageDoorOpenRecovery(int now)
        {
            if (!_garageDoorAssistActive)
            {
                _garageDoorAssistActive = true;
                _garageDoorClosedApplied = false;
                _garageDoorUnlocked = false;
                _garageDoorOpeningRatio = 0.0f;
                _garageDoorOpeningLastUpdate = now;
                _garageDoorNextAttempt = 0;
            }

            if (_garageDoorHoldOpen && now < _garageDoorNextStateProbe)
            {
                return;
            }
            if (now < _garageDoorNextAttempt)
            {
                return;
            }

            int doorHash = _garageDoorSystemHash;
            if (doorHash == 0 || !_garageDoorUnlocked || _garageDoorHoldOpen)
            {
                if (!TryFindBennysGarageDoor(out doorHash))
                {
                    MaintainBennysGarageDoorUnstreamedFallback();
                    _garageDoorSystemHash = 0;
                    _garageDoorUnlocked = false;
                    _garageDoorHoldOpen = false;
                    _garageDoorNextAttempt = now + BennysGarageDoorUnavailableRetryMs;
                    return;
                }
            }

            if (_garageDoorSystemHash != doorHash)
            {
                _garageDoorUnlocked = false;
                _garageDoorHoldOpen = false;
                _garageDoorOpeningRatio = 0.0f;
                _garageDoorOpeningLastUpdate = now;
            }
            _garageDoorSystemHash = doorHash;

            if (!IsBennysGarageDoorPhysicsLoaded(doorHash))
            {
                _garageDoorSystemHash = 0;
                _garageDoorUnlocked = false;
                _garageDoorHoldOpen = false;
                MaintainBennysGarageDoorUnstreamedFallback();
                _garageDoorNextAttempt = now + BennysGarageDoorUnavailableRetryMs;
                return;
            }

            if (_garageDoorHoldOpen)
            {
                if (IsBennysGarageDoorStillOpen(doorHash))
                {
                    _garageDoorNextStateProbe = now + BennysGarageDoorStateProbeMs;
                    return;
                }

                _garageDoorHoldOpen = false;
                _garageDoorUnlocked = false;
                _garageDoorOpeningRatio = 0.0f;
                _garageDoorOpeningLastUpdate = now;
            }

            if (!_garageDoorUnlocked)
            {
                SetBennysGarageDoorHoldOpen(doorHash, false);
                SetBennysGarageDoorState(doorHash, 0);
                _garageDoorUnlocked = true;
                _garageDoorUnstreamedClosedApplied = false;
            }

            int elapsed = now - _garageDoorOpeningLastUpdate;
            if (elapsed < 0) elapsed = 0;
            if (elapsed > 250) elapsed = 250;
            _garageDoorOpeningLastUpdate = now;
            _garageDoorOpeningRatio = Math.Min(
                1.0f,
                _garageDoorOpeningRatio
                    + (float)elapsed / BennysGarageDoorOpeningDurationMs);

            SetBennysGarageDoorOpenRatio(doorHash, _garageDoorOpeningRatio);
            if (_garageDoorOpeningRatio >= 1.0f)
            {
                SetBennysGarageDoorHoldOpen(doorHash, true);
                _garageDoorHoldOpen = true;
                _garageDoorNextStateProbe = now + BennysGarageDoorStateProbeMs;
            }

            _garageDoorNextAttempt = now + BennysGarageDoorUpdateMs;
        }

        private void MaintainBennysGarageDoorClosedRecovery(int now)
        {
            if (_garageDoorAssistActive || _garageDoorUnlocked || _garageDoorHoldOpen)
            {
                _garageDoorAssistActive = false;
                _garageDoorClosedApplied = false;
                _garageDoorNextAttempt = 0;
            }

            if (!_garageDoorUnstreamedClosedApplied)
            {
                SetBennysGarageDoorUnstreamedClosed();
                _garageDoorUnstreamedClosedApplied = true;
            }

            if (_garageDoorClosedApplied && now < _garageDoorNextStateProbe)
            {
                return;
            }
            if (now < _garageDoorNextAttempt)
            {
                return;
            }

            int doorHash;
            if (!TryFindBennysGarageDoor(out doorHash))
            {
                _garageDoorSystemHash = 0;
                _garageDoorClosedApplied = false;
                _garageDoorNextAttempt = now + BennysGarageDoorUnavailableRetryMs;
                return;
            }

            if (!IsBennysGarageDoorPhysicsLoaded(doorHash))
            {
                _garageDoorSystemHash = 0;
                _garageDoorClosedApplied = false;
                _garageDoorNextAttempt = now + BennysGarageDoorUnavailableRetryMs;
                return;
            }

            if (_garageDoorClosedApplied && _garageDoorSystemHash == doorHash
                && !IsBennysGarageDoorStillClosed(doorHash))
            {
                _garageDoorClosedApplied = false;
            }

            if (!_garageDoorClosedApplied || _garageDoorSystemHash != doorHash)
            {
                SetBennysGarageDoorHoldOpen(doorHash, false);
                SetBennysGarageDoorOpenRatio(doorHash, 0.0f);
                SetBennysGarageDoorState(doorHash, 1);
            }

            _garageDoorSystemHash = doorHash;
            _garageDoorClosedApplied = true;
            _garageDoorHoldOpen = false;
            _garageDoorUnlocked = false;
            _garageDoorOpeningRatio = 0.0f;
            _garageDoorOpeningLastUpdate = 0;
            _garageDoorNextAttempt = 0;
            _garageDoorNextStateProbe = now + BennysGarageDoorStateProbeMs;
        }

        private void MaintainBennysGarageDoorRecovery(int now)
        {
            if (now < _garageDoorNextUpdate)
            {
                return;
            }

            _garageDoorNextUpdate = now + BennysGarageDoorUpdateMs;

            if (fixDoor != 1)
            {
                if (_garageDoorAssistActive || _garageDoorInsideLatched
                    || _garageDoorClosedApplied)
                {
                    ReleaseBennysGarageDoorRecovery();
                }

                ReleaseBennysGarageDoorModel();
                _garageDoorInsideLatched = false;
                _garageDoorInsideExitOpening = false;
                _garageDoorDepthSampleValid = false;
                _wantedGarageClearSince = -1;
                _garageDoorPoliceBlocked = false;
                return;
            }

            float distanceSquared;
            float interiorDepth;
            if (!TryGetBennysGarageDoorPlayerGeometry(
                    out distanceSquared,
                    out interiorDepth))
            {
                return;
            }

            bool withinDoorManagementDistance = distanceSquared
                <= BennysGarageDoorInitialCloseDistance
                    * BennysGarageDoorInitialCloseDistance;
            if (!withinDoorManagementDistance)
            {
                _garageDoorInsideLatched = false;
                _garageDoorInsideExitOpening = false;
                _garageDoorDepthSampleValid = false;
                _wantedGarageClearSince = -1;
                _garageDoorPoliceBlocked = false;

                if (distanceSquared > BennysGarageDoorModelReleaseDistance
                    * BennysGarageDoorModelReleaseDistance)
                {
                    bool hadManagedDoorState = _garageDoorModelRequested
                        || _garageDoorSystemHash != 0 || _garageDoorClosedApplied
                        || _garageDoorUnstreamedClosedApplied || _garageDoorAssistActive
                        || _garageDoorHoldOpen || _garageDoorUnlocked;
                    if (hadManagedDoorState)
                    {
                        ReleaseBennysGarageDoorModel();
                        ResetBennysGarageDoorManagedState();
                        _garageDoorNextUpdate = now + BennysGarageDoorUpdateMs;
                    }
                }
                else if (distanceSquared <= BennysGarageDoorModelPreloadDistance
                    * BennysGarageDoorModelPreloadDistance)
                {
                    if (now >= _garageDoorModelNextRequest)
                    {
                        if (!_garageDoorModelRequested || !IsBennysGarageDoorModelLoaded())
                        {
                            RequestBennysGarageDoorModel();
                        }
                        _garageDoorModelNextRequest = now + BennysGarageDoorModelRetryMs;
                    }
                }
                return;
            }

            bool withinModelPreloadDistance =
                distanceSquared
                <= (BennysGarageDoorModelPreloadDistance
                    * BennysGarageDoorModelPreloadDistance);

            if (withinModelPreloadDistance)
            {
                if (now >= _garageDoorModelNextRequest)
                {
                    bool modelLoaded = _garageDoorModelRequested
                        && IsBennysGarageDoorModelLoaded();
                    if (!modelLoaded)
                    {
                        RequestBennysGarageDoorModel();
                    }

                    _garageDoorModelNextRequest = now + (modelLoaded
                        ? BennysGarageDoorStateProbeMs : BennysGarageDoorModelRetryMs);
                }
            }

            bool movingTowardOutside =
                _garageDoorDepthSampleValid
                && interiorDepth
                    < (_garageDoorLastInteriorDepth
                        - BennysGarageDoorDirectionEpsilon);

            bool movingDeeperInside =
                _garageDoorDepthSampleValid
                && interiorDepth
                    > (_garageDoorLastInteriorDepth
                        + BennysGarageDoorDirectionEpsilon);

            _garageDoorLastInteriorDepth = interiorDepth;
            _garageDoorDepthSampleValid = true;

            bool withinDoorRetryDistance =
                distanceSquared
                <= (BennysGarageDoorRetryDistance
                    * BennysGarageDoorRetryDistance);

            bool withinOutsideAssistDistance =
                distanceSquared
                <= (BennysGarageDoorOpeningDistance
                    * BennysGarageDoorOpeningDistance);

            bool withinInsideExitAssistDistance =
                distanceSquared
                <= (BennysGarageDoorInsideExitAssistDistance
                    * BennysGarageDoorInsideExitAssistDistance);

            bool currentVehicleAllowed =
                withinDoorRetryDistance
                && (bypassGarageDoor
                    || IsCurrentBennysGarageDoorVehicleAllowed());

            if (!_garageDoorInsideLatched
                && withinDoorRetryDistance
                && interiorDepth >= BennysGarageDoorInsideCloseDepth)
            {
                _garageDoorInsideLatched = true;
                _garageDoorInsideExitOpening = false;
                _wantedGarageClearSince = -1;

                MaintainBennysGarageDoorClosedRecovery(now);
                return;
            }

            if (_garageDoorInsideLatched)
            {
                if (!withinDoorRetryDistance)
                {
                    return;
                }

                if (interiorDepth <= BennysGarageDoorOutsideResetDepth)
                {
                    _garageDoorInsideLatched = false;
                    _garageDoorInsideExitOpening = false;
                    _garageDoorNextAttempt = 0;
                }
                else
                {
                    if (!currentVehicleAllowed)
                    {
                        _garageDoorInsideExitOpening = false;
                        MaintainBennysGarageDoorClosedRecovery(now);
                        return;
                    }

                    if (!_garageDoorInsideExitOpening
                        && movingTowardOutside
                        && withinInsideExitAssistDistance)
                    {
                        _garageDoorInsideExitOpening = true;
                        _garageDoorNextAttempt = 0;
                    }

                    if (_garageDoorInsideExitOpening
                        && movingDeeperInside
                        && interiorDepth >= BennysGarageDoorInsideCloseDepth)
                    {
                        _garageDoorInsideExitOpening = false;
                        _garageDoorNextAttempt = 0;
                    }

                    if (_garageDoorInsideExitOpening)
                    {
                        MaintainBennysGarageDoorOpenRecovery(now);
                    }
                    else
                    {
                        MaintainBennysGarageDoorClosedRecovery(now);
                    }

                    return;
                }
            }

            bool hasWantedLevel = Game.Player != null
                && Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Game.Player.Handle) > 0;
            bool policeTrackingPlayer = withinDoorRetryDistance
                && hasWantedLevel && IsWantedAndPoliceTrackingPlayer();
            if (policeTrackingPlayer)
            {
                _wantedGarageClearSince = -1;
                if (!_garageDoorPoliceBlocked)
                {
                    _garageDoorPoliceBlocked = true;
                    _garageDoorNextAttempt = 0;
                    Logger.Debug("Garage closed: police are actively tracking the wanted player.");
                }
                MaintainBennysGarageDoorClosedRecovery(now);
                return;
            }

            if (_garageDoorPoliceBlocked)
            {
                Logger.Debug("Garage entry unblocked: police lost sight or wanted level cleared.");
            }
            _garageDoorPoliceBlocked = false;
            bool waitingForPoliceClearConfirmation = false;
            if (withinDoorRetryDistance && currentVehicleAllowed && hasWantedLevel)
            {
                if (_wantedGarageClearSince < 0 || now < _wantedGarageClearSince)
                {
                    _wantedGarageClearSince = now;
                }
                waitingForPoliceClearConfirmation =
                    now - _wantedGarageClearSince < WantedGarageClearConfirmMs;
            }
            else
            {
                _wantedGarageClearSince = -1;
            }

            if (withinOutsideAssistDistance
                && currentVehicleAllowed
                && !waitingForPoliceClearConfirmation)
            {
                MaintainBennysGarageDoorOpenRecovery(now);
            }
            else if (_garageDoorAssistActive)
            {
                MaintainBennysGarageDoorClosedRecovery(now);
            }
            else if (distanceSquared <= (BennysGarageDoorInitialCloseDistance
                    * BennysGarageDoorInitialCloseDistance))
            {
                MaintainBennysGarageDoorClosedRecovery(now);
            }
        }

        private void UpdateWorkshopWantedEscape(bool inWorkshop)
        {
            if (!inWorkshop || Game.Player == null)
            {
                _wantedWorkshopWaiting = false;
                _wantedWorkshopWaitLevel = 0;
                return;
            }

            int level = Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Game.Player.Handle);
            if (level <= 0)
            {
                _wantedWorkshopWaiting = false;
                _wantedWorkshopWaitLevel = 0;
                return;
            }

            if (!_wantedWorkshopWaiting || level > _wantedWorkshopWaitLevel
                || Game.GameTime < _wantedWorkshopWaitSince)
            {
                _wantedWorkshopWaiting = true;
                _wantedWorkshopWaitSince = Game.GameTime;
                _wantedWorkshopWaitLevel = level;
                return;
            }

            int escapeMs = 30000 + (Math.Min(level, 5) - 1) * 15000;
            if (Game.GameTime - _wantedWorkshopWaitSince >= escapeMs)
            {
                Function.Call(Hash.CLEAR_PLAYER_WANTED_LEVEL, Game.Player.Handle);
                Logger.Debug("Wanted level cleared after waiting inside Benny's.");
                _wantedWorkshopWaiting = false;
                _wantedWorkshopWaitLevel = 0;
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                if (!EnsureGameNativeStartupInitialized())
                {
                    return;
                }

                if (optEnableMouse && Helper._menuPool != null && Helper._menuPool.AreAnyVisible)
                {
                    EnableWorkshopMenuMouseControls();
                }

                UpdateBennysDlcInterior();
                ply = Game.Player.Character;

                int gameTime = Game.GameTime;
                if (CanRunBennysGarageDoorRecovery(gameTime))
                {
                    MaintainBennysGarageDoorRecovery(gameTime);
                }

                veh = ply?.LastVehicle;

                if (veh != null && veh.IsVehicleAttachedToTrailer())
                {
                    tra = veh.GetVehicleTrailerVehicle();
                }

                if (veh == null || ply == null)
                {
                    _wantedWorkshopWaiting = false;
                    _wantedWorkshopWaitLevel = 0;
                    SetWorkshopPlayerControlSuppressed(false);
                    SetWorkshopCarModShopState(false);
                    return;
                }

                ProcessWorkshopCutscene();

                bool isMenuVisible = Helper._menuPool != null && Helper._menuPool.AreAnyVisible;
                UpdateWorkshopWantedEscape(isMenuVisible && !isCutscene
                    && ply.CurrentVehicle == veh
                    && veh.Position.DistanceToSquared(WorkshopInteriorPosition) <= 36.0f);
                int currentInteriorId = GetCurrentPlayerInteriorId(isMenuVisible);
                bool isInBennysInterior = bennyIntID != 0 && currentInteriorId == bennyIntID;
                string vehicleDenialMessage = GetWorkshopVehicleDenialMessage(veh);
                bool isWorkshopVehicleAllowed = !unWelcome.Contains(veh.ClassType) && vehicleDenialMessage == null;
                Vector3 approachOffset = veh.Position - BennysGarageDoorPosition;
                float garageApproachDepth =
                    approachOffset.X * BennysGarageDoorInteriorDirX
                    + approachOffset.Y * BennysGarageDoorInteriorDirY;
                bool isNearGarageDoor =
                    approachOffset.Length() <= BennysGarageDoorRetryDistance
                    && garageApproachDepth <= 1.5f;

                bool isInsideWorkshop = isCutscene
                    || isMenuVisible
                    || (isWorkshopVehicleAllowed && isInBennysInterior);

                SetWorkshopPlayerControlSuppressed(isMenuVisible);

                SetWorkshopCarModShopState(
                    isInsideWorkshop
                    && isWorkshopVehicleAllowed
                    && ply.CurrentVehicle == veh);

                if (isInsideWorkshop && (isCutscene || isMenuVisible || !isNearGarageDoor))
                {
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 10);
                }

                if (isInBennysInterior && !IsArenaWarDLCInstalled())
                {
                    Helper.DisplayHelpTextThisFrame("Unsupported GTA V version detected! SPB may not work properly on this version.");
                }

                if (isNearGarageDoor && ply.CurrentVehicle == veh
                    && !isCutscene && !isMenuVisible)
                {
                    if (!string.IsNullOrEmpty(vehicleDenialMessage))
                    {
                        Helper.DisplayHelpTextThisFrame(vehicleDenialMessage);
                    }
                    else if (isWorkshopVehicleAllowed
                        && (_garageDoorPoliceBlocked || IsWantedAndPoliceTrackingPlayer()))
                    {
                        Helper.DisplayHelpTextThisFrame("Lose wanted level.");
                    }
                }

                if (isInBennysInterior && isWorkshopVehicleAllowed)
                {
                    if (!isExiting)
                    {
                        if (CanTriggerEnterCutscene())
                        {
                            UpdateTitleName();
                            PlayEnterCutScene();
                        }
                        else if (veh.Position.DistanceTo(WorkshopInteriorPosition) <= 5.0f)
                        {
                            camera?.Update();
                            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 3);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 4);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 5);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 13);
                        }
                    }

                    if (isExiting)
                    {
                        Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                        Game.DisableAllControlsThisFrame();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }

            if (Helper._menuPool != null && Helper._menuPool.AreAnyVisible)
            {
                bool doorControlReleased = Game.IsControlJustReleased(doorKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, (int)doorKey);

                if (doorControlReleased)
                {
                    if (veh.Doors[VehicleDoorIndex.FrontLeftDoor].IsOpen)
                    {
                        for (int doorIndex = 0; doorIndex <= 5; doorIndex++)
                        {
                            Function.Call((Hash)0x62A456AA4769EF34UL, veh.Handle, doorIndex);
                        }
                        Function.Call(Hash.SET_VEHICLE_DOORS_SHUT, veh, false);
                    }
                    else
                    {
                        veh.OpenDoor(VehicleDoorIndex.BackLeftDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.BackRightDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.FrontLeftDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.FrontRightDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.Hood, false, false);
                        veh.OpenDoor(VehicleDoorIndex.Trunk, false, false);
                        for (int doorIndex = 0; doorIndex <= 5; doorIndex++)
                        {
                            Function.Call((Hash)0x3A539D52857EA82DUL, veh.Handle, doorIndex);
                        }
                    }
                }
                else if ((Game.IsControlJustPressed(roofKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)roofKey))
                    && MenuHelper.IsWorkshopVehicleConvertible())
                {
                    if (veh.RoofState == VehicleRoofState.Closed)
                    {
                        Function.Call(Hash.LOWER_CONVERTIBLE_ROOF, veh, false);
                    }
                    else
                    {
                        Function.Call(Hash.RAISE_CONVERTIBLE_ROOF, veh, false);
                    }
                }

                if ((Game.IsControlPressed(zinKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)zinKey))
                    && camera.MainCameraPosition != CameraPosition.Interior)
                {
                    PointF max = new PointF(6.0f, 3.0f);
                    if (camera.CameraZoom > max.Y)
                    {
                        camera.CameraZoom -= 0.1f;
                    }
                    else
                    {
                        camera.CameraZoom = max.Y;
                    }
                }

                if ((Game.IsControlPressed(zoutKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)zoutKey))
                    && camera.MainCameraPosition != CameraPosition.Interior)
                {
                    PointF max = new PointF(6.0f, 3.0f);
                    if (camera.CameraZoom < max.X)
                    {
                        camera.CameraZoom += 0.1f;
                    }
                    else
                    {
                        camera.CameraZoom = max.X;
                    }
                }

                bool firstPersonControlReleased = Game.IsControlJustReleased(fpcKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, (int)fpcKey);

                if (firstPersonControlReleased && camera != null)
                {
                    CameraPosition previousCameraPosition = lastCameraPos;
                    lastCameraPos = camera.MainCameraPosition;

                    if (camera.MainCameraPosition == CameraPosition.Interior)
                    {
                        camera.MainCameraPosition = previousCameraPosition == CameraPosition.Interior
                            ? CameraPosition.Car
                            : previousCameraPosition;
                    }
                    else
                    {
                        camera.MainCameraPosition = CameraPosition.Interior;
                    }
                }

            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            ResetBennysGarageDoorManagedState();

            SetWorkshopPlayerControlSuppressed(false);
            SetWorkshopCarModShopState(false);
            BennysBlip?.Delete();
            GTA.UI.Screen.FadeIn(1000);

            if (bennyPed != null)
            {
                bennyPed.Delete();
            }
        }
    }
}
