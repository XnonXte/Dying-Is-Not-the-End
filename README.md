# Dying Is Not the End

A creative puzzle platformer game built with Unity where time manipulation and clone replays are core gameplay mechanics.

## 🎮 Game Overview

**Dying Is Not the End** is a 2D puzzle platformer that challenges players to think creatively about time and space. The core mechanic revolves around creating clones of your previous actions, allowing you to solve puzzles by coordinating multiple versions of yourself across time.

### Key Concept

The game operates on a **time loop system**:
- Each level has a fixed loop duration (default: 20 seconds)
- Record your movements and interactions within this time window
- When time expires, a clone spawns and replays your recorded actions
- Create up to 2 clones simultaneously
- Use the coordination between past and present selves to solve environmental puzzles

## 🎯 Core Game Mechanics

- **Player Control**: Move and interact with the environment using standard platformer controls
- **Clone Replay System**: Your movements and button interactions are recorded and replayed by clones
- **Puzzle Elements**:
  - **Buttons/Pedestals**: Activate mechanisms by standing on them
  - **Doors**: Progress through levels by unlocking doors
  - **Platforms**: Both static and moving platforms that respond to button activation
  - **Exit Points**: Reach the exit to complete a level

- **Time Management**: Manage actions within each loop cycle to efficiently solve puzzles
- **Spawn Points**: Respawn at designated spawn points if needed

## 📁 Project Structure

```
Assets/
├── Scripts/              # Core game mechanics
│   ├── Player.cs        # Player movement, loop timer, clone creation
│   ├── CloneReplay.cs   # Clone replay and interaction logic
│   ├── Button.cs        # Button/pedestal interaction
│   ├── Door.cs          # Door mechanics
│   ├── Exit.cs          # Level exit logic
│   ├── Platform.cs      # Moving platform behavior
│   ├── Plate.cs         # Plate/trigger mechanics
│   └── FrameData.cs     # Records position and interaction state
├── Scenes/
│   ├── TestingScene.unity
│   └── Levels/
│       └── Level_XX.unity  # Individual puzzle levels
├── Prefabs/             # Reusable game objects
├── Sprites/             # 2D graphics and sprites
├── Palettes/            # Color palettes for UI/graphics
└── Settings/            # Game configuration files
```

## 🛠️ Technical Details

- **Engine**: Unity 6000.4.5f1
- **Scripting**: C# with modern features
- **Rendering**: 2D with URP support
- **UI**: TextMesh Pro for timer and clone display
- **Physics**: Rigidbody2D for character and clone physics

## 🎮 How to Play

1. **Move**: Use directional input to move your character
2. **Jump**: Activate jump while on the ground
3. **Interact**: Stand on buttons/plates to trigger mechanisms
4. **Time Loops**: Each level has a set time duration
5. **Use Clones**: After creating clones, they repeat your actions from previous loops
6. **Solve Puzzles**: Coordinate your movement with clone actions to activate doors and reach exits
7. **Progress**: Complete all levels to progress through the game

## 💡 Design Philosophy

The game encourages:
- **Creative Problem-Solving**: Think about how to use past selves to influence the present
- **Spatial Reasoning**: Understand positioning and timing of clone interactions
- **Experimentation**: Try different approaches to optimize puzzle solutions
- **Strategic Planning**: Plan your movements within the time loop constraint

## 📋 Game Features

- Multiple puzzle levels with increasing complexity
- UI feedback for timer and clone count
- Responsive player controls
- Smooth clone replay mechanics
- Environmental interaction system

## 🚀 Getting Started

1. Clone or download the project
2. Open with Unity version 6000.4.5f1 or compatible
3. Load a scene from `Assets/Scenes/Levels/`
4. Press Play to test
5. Use the arrow keys or WASD to move, Space to jump
6. Explore and solve puzzles!

## 📝 Development Notes

- The `Player.cs` script manages the main loop timer and clone spawning
- `FrameData.cs` records each frame's position and interaction state for replay
- `CloneReplay.cs` handles the replay logic and clone interactions
- Puzzle design uses button states and platform activation patterns
- The spawn point system ensures players can restart from designated locations

## 🎓 Learning Resources

This project demonstrates:
- Time-based recording and replay systems
- 2D platformer physics and movement
- UI/UX implementation with TextMesh Pro
- Prefab and object pool management
- Event-driven design patterns
- Puzzle design mechanics

---

**Status**: In Development  
**Platform**: PC (WebGL export possible)  
**Genre**: Puzzle Platformer
