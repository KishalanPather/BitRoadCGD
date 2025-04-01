# BitRoad

## Description
A low-poly endless runner-style driving game built in Unity 6. Players drive a car, avoid obstacles and make sure to not run out of fuel.

## Group Members
- Kishalan Pather
- Simphile Mkhize
- Mahir Ahammed

## Implemented Features
- **Endless Road Generation**: The road continuously generates ahead of the player through procedural generation.
- **Collectibles Generation**: Fuel packs and speed boosts are generated throughout the game.
- **Obstacle Generation**: Obstacles are generated randomly per section with no generation first to increasing generation over time.
- **Fuel System**: Players must collect fuel packs to keep driving.
- **Score System**: Earn points based on distance traveled.
- **Game Over Mechanic**: Running out of fuel or collision with obstacles triggers a game-over screen. (**NOTE**: Collisions are not working properly with obstacles and environment, the game ending mechanism is reliant on fuel depletion only)

## Controls
- **Arrow Keys / WASD**: Steer the car.

## Scenes in use
In Assets/Scenes the following are in use:
- Home.unity
- Tutorial.unity
- InitialGeneration.unity (The main game scene)

## Installation & Running the Game
1. Download project zip file or clone repository.
2. Open the project in Unity.
3. Open Assets/Scenes/Home.unity
4. Run the scene in the Unity Editor
5. Select Start Game or Tutorial to play the game

## Asset packs
1. KayKit: https://kaylousberg.itch.io/city-builder-bits
2. Kenney: https://kenney.nl/assets/city-kit-roads
