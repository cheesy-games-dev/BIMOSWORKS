# Changelog
All notable changes to this package will be documented in this file.

## [2.0.0] - XX/08/2025

### BIMOS
- Added custom avatars
- Added haptics
  - Grabbing haptics
  - Hand collision haptics
  - Custom haptic events for grabbables
- Created a new physics body from scratch (imported the prototype BIPED rig)
  - Body colliders are now in a more accurate position (behind the head)
  - Replaced leg and torso collider with singular body collider
  - Added smoothing to snap turn (reduces large physical forces)
  - Improved locomotion sphere
    - Slope fighting uses impulse instead of force (more accuracy)
    - Target move speed is now met even on slopes
  - Removed fender collider (replaced with locomotion sphere friction contact modification)
  - Jumping now uses a reusable scriptable object-based state machine
- Overhauled grabbing system to be soft
- Added custom project validation rules to allow for rapid setup without a guide
- Improved hand pose editor
  - Added ability to load custom avatar hands
  - Made editor more intuitive by highlighting the currently used sub-pose
- Changed tracked pose drivers to use OpenXR's "palm pose"
  - Added the "Palm Pose Emulator" from BIPED for standalone
- Sounds now use Audio Random Containers
- Attachers detaching from sockets now keep their detach animation velocity
- Grabs are now called grabbables (for more consistent naming with interactables)
- Fixed issue where seated magazines would be counted, disallowing further summoning
- References to local player objects are now available via a singleton
- Updated grab rank calculation to factor in hand rotation
- Added basic holster/body slot support
- Added roomscale crouching
- Moved body colliders back so they more accurately reflect where the avatar's body is
- Added socket attacher cooldown
- Hands now follow attacher as it's inserted into a socket
- Added socket attacher alignment animation
  - Players are rewarded with quicker attachment times if they line them up correctly

### Samples
- Created a full modular gun system
- Added an assault rifle (model by Mason Mad, my love)
- Given the pistol a new model (model by Mason Mad, again (my love))
- Redone all hand poses to work with the new system
  - You will have to do this for your own custom hand poses
- Gun casings and bullet holes now despawn when they reach a limit, rather than over time

## [1.0.0] - 06/12/2024

### This is the first release of *BIMOS*.