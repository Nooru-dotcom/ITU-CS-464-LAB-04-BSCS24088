# ITU-CS-464-LAB-04-BSCS24088

Unity 6 (6000.6.0f1) project for CS-464 lab. A blockout level (Level1_SkyRun) was exported as a package from the previous lab and imported here. A player is placed in the level with a movement script and a camera that follows it.

## What is in this project

- Level1_SkyRun scene (imported from the previous lab's package)
- Player: capsule with a Character Controller and the PlayerController script
- Main Camera with the CameraFollow script following the player
- LevelWalkthrough.mp4: screen recording of the level walkthrough

## Controls

- W / Up Arrow: move forward
- S / Down Arrow: move backward
- A / Left Arrow: move left
- D / Right Arrow: move right
- Space: jump

## Scripts

- PlayerController.cs: handles movement, rotation toward the move direction, gravity and jumping using Character Controller.
- CameraFollow.cs: smoothly follows the player from a fixed offset and looks at the player.

## How to run

1. Open the project in Unity Hub with editor version 6000.6.0f1.
2. Open the Level1_SkyRun scene from the Assets folder.
3. Press Play.

## Author

Muhammad Noor ul Hassan (BSCS24088)
