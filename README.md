# Hammer-a-Mole VR

A Whac-A-Mole style game in virtual reality for Meta Quest headsets. The player grabs a rubber hammer and whacks moles popping out of an arcade machine. As the game goes on, the moles hide faster and faster.

### Gameplay

- Press the **Start** button on the machine to begin.
- Moles pop up at random. Hitting a mole gives points, missing one costs a life.
- The game ends when all lives are lost.
- The score and game time are saved to the scoreboard (top 10 results per difficulty, stored locally).

### Settings

- **Difficulty Settings**: choose the difficulty level and turn controller vibrations on or off.
- **Audio Settings**: choose the lobby and game music, and set music and effects volume.

### Difficulty levels

| Level | Lives | Mole up time | Score multiplier |
|---|---|---|---|
| Easy | 3 | longest | ×1 |
| Medium | 2 | shorter | ×1.5 |
| Hard | 1 | shortest | ×2 |

On every level the up time keeps decreasing during the game.

### Special moles

| Mole | Hit | Missed |
|---|---|---|
| Normal | points | −1 life |
| Golden | ×5 points | no penalty |
| Bomb | −1 life | no penalty |
| Heart | +1 life and points | no penalty |

The chance of a special mole appearing depends on the difficulty level.
