# State Pattern

## Overview
The State Pattern is a behavioral design pattern that allows an object to alter its behavior when its internal state changes. The object will appear to change its class. It encapsulates state-dependent behavior and makes state transitions explicit.

## Real-World Example: Media Player
Our implementation demonstrates a media player that can be in different states:
- Playing
- Paused
- Stopped
- Locked

### Key Components

1. **Context (MediaPlayer)**
   - Maintains a reference to the current state
   - Delegates state-specific behavior to the current state object
   - Handles time tracking and playback speed

2. **State Interface (IMediaPlayerState)**
   - Defines the interface for all concrete states
   - Methods: Play, Pause, Stop, Lock, Speed

3. **Concrete States**
   - **PlayingState**: Normal playback with speed control
   - **PausedState**: Temporarily halted playback
   - **StoppedState**: Complete halt, resets progress
   - **LockedState**: Prevents all operations until unlocked

### Benefits
1. **Single Responsibility Principle**: Each state handles its own behavior
2. **Open/Closed Principle**: New states can be added without changing existing code
3. **Eliminates Complex Conditionals**: No need for multiple if/else statements
4. **State Transitions are Explicit**: Clear and maintainable state changes

### Implementation Details

#### State Transitions
```
Playing ←→ Paused
   ↑↓       ↑↓
Stopped ←→ Locked
```

#### Special Features
- Accurate time tracking with playback speed
- State preservation when locked/unlocked
- Validation for playback speed (0.1x to 4.0x)
- Async operation support
- Proper error handling

### Usage Example
```csharp
var player = new MediaPlayer();
await player.Play();         // Starts playing
await player.Speed(2.0);     // Changes speed to 2x
await player.Lock();         // Locks the player
await player.Lock();         // Unlocks the player
await player.Stop();         // Stops playback
```

## Best Practices
1. Keep state transitions explicit and well-documented
2. Maintain state-specific validation
3. Ensure proper cleanup when changing states
4. Use async/await for potentially long-running operations
5. Preserve state information when needed (e.g., locked state)

## When to Use
- Complex state-dependent behavior exists
- State transitions need to be explicit
- Object behavior changes completely based on its state
- State-specific validation is required
