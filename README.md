# Candy Dandy

Dit project is gemaakt als onderdeel van ons examen voor de opleiding Software/Game Development & Game Artist van Mediacollege Amsterdam. Het doel is om onze vaardigheden in game design te laten zien.
[Onze Trello](https://trello.com/b/QKXxfOid/pvb2025)
[Onze Wiki](https://github.com/j5x/PvB2025/wiki)

# Geproduceerde Game Onderdelen


### Student Jason Siegersma:
  * [Match 3 System](https://github.com/j5x/PvB2025/tree/feature/grid/Assets/Script)
 
### Student Jahvairo Monkau:
  * [Character](https://github.com/j5x/PvB2025/blob/develop/Assets/Script/Character.cs)
  * [Player class](https://github.com/j5x/PvB2025/blob/develop/Assets/Script/Player.cs)
  * [Enemy class](https://github.com/j5x/PvB2025/blob/develop/Assets/Script/Enemy.cs)
  * [Enemy AI](https://github.com/j5x/PvB2025/blob/develop/Assets/Script/AttackComponent.cs)


### Student Gael Griffith:
  * [Health-System Script](https://github.com/j5x/PvB2025/blob/develop/Assets/Script/HealthComponent.cs)
  * [Round-Timer Script](https://github.com/j5x/PvB2025/blob/feature/round-timer/Assets/Script/RoundTimer.cs)
  * [Damage-System (Player) Script](https://github.com/j5x/PvB2025/blob/feature/Damage-System/Assets/Script/Player.cs)
  * [Damage-System (Enemy) Script](https://github.com/j5x/PvB2025/blob/feature/Damage-System/Assets/Script/Enemy.cs)


# Match 3 System - Jason Siegersma

## Overview
This Match-3 system is built using Unity and written in C#. It allows tiles to be swapped, matched, cleared, and refilled dynamically. The system ensures that tiles fall into empty spaces rather than respawning in the same spot. 

A special **white tile** is included, which can match with any color. When matched, it clears all tiles of that color from the grid.

## Features
✅ No pre-existing matches at the start.  
✅ Swappable tiles with valid match detection.  
✅ Tile gravity ensures tiles fall into empty spaces.  
✅ Grid automatically refills after matches.  
✅ Special **white tile** that clears all of one color when matched.  

---

## 🔹 Scripts Included

### 1. `GridManager.cs`
**Responsibilities:**
- Initializes the grid with random tiles.
- Ensures no matches are present at the start.
- Handles tile swapping and match detection.
- Applies gravity so tiles fall after matches.
- Refills the grid when tiles are cleared.

**Key Variables:**
```csharp
[SerializeField] private int width = 8;
[SerializeField] private int height = 8;
[SerializeField] private float tileSize = 1.0f;
```
width and height: Define the grid size.

tileSize: Controls the spacing between tiles.

### 2. Tile.cs
**Responsibilities:**

- Represents individual tiles in the game.
- Handles tile selection, visual feedback, and position updates.
flowchart for Match 3
```mermaid
graph TD;
start((Start)) -->|Initialize Grid| setup[Setup Grid];
setup -->|Wait for Player Input| playerTurn[Player's Turn];
playerTurn --> |Swap Tiles| swap[Swap Tiles];
swap --> |Check for Matches| checkMatches{Match Found?};
checkMatches -- No --> playerTurn;
checkMatches -- Yes --> removeTiles[Remove Matched Tiles];
removeTiles --> applyGravity[Apply Gravity];
applyGravity --> refillGrid[Refill Grid];
refillGrid --> |Check for New Matches| checkNewMatches{New Matches?};
checkNewMatches -- Yes --> removeTiles;
checkNewMatches -- No --> playerTurn;
```
```mermaid
classDiagram

class GridManager{
    +int width
    +int height
    +void SwapTiles()
    +void CheckMatches()
    +void ApplyGravity()
}

class Tile{
    +int tileType
    +bool isWhiteTile
    +void Select()
    +void Match()
}
GridManager --> Tile
```



### flowchart voor gameplay:
```mermaid
graph TD;
start((Start)) -->|you go to the character selection screen| characterselection(Select a Character);
characterselection  --> |After selecting character the Match3 Game Starts| match3(Match 3 Game);
match3 --> |Start with a turn| matching(Attempt to match a card);
match3 -->|EnemyAI gets a turn| matching;
matching -->|Matched a card? no| reset;
matching --> |Matched a card? yes| ability(Debuff, Buff, Combat Move);
matching -->|AI Matched a card? no| reset;
matching --> |AI Matched a card? yes| ability-ai(AI-Debuff, AI-Buff, AI-Combat Move);
reset --> |Try again to get a match| match3;
ability-ai -->|AI Random Buff?| AI-Buff(The enemy gets stronger for his next move);
ability-ai -->|AI Random Combat Move?| AI-CombatMove(AI gets a random combat move he dangerous now);
ability-ai -->|AI Random Debuff?| AI-Debuff(AI gets a random debuff);
AI-Buff -->|The enemy is stronger now| AI-Buffed(The enemy gets a buff for his next move);
AI-CombatMove -->|The enemy gets a combat move| AI-Damage(Deal damage to the player);
AI-Debuff -->|The enemy gets buffed| AI-Debuffed(The enemy is debuffed now);
AI-Buffed -->|Oh yo FUHHED NOW BOI HE GOT BUFFED| enemybuffed(The enemy stronger now);
AI-Debuffed -->|trash| enemydebuffed(The enemy weaker now);
AI-Damage -->|trash| enemydoesdamage(Damages the player);
enemydoesdamage -->|The player dies| match3;
enemydoesdamage -->|The player takes damage| HealthPlayer(The player health);
HealthPlayer -->|Is player death? no| nextturn(Now its the players turn again)
ability --> |Random Combat move| damage(Deal damage to the enemy);
ability --> |Random debuff?| debuff(you are debuffed);
ability --> |Random buff?| buff(You got buffed for the next move you do);
buff --> |You got buffed now| buffed(You stronger now for the next combat move used);
damage --> |You can do damage to the enemy now| enemy(Takes damage from hit);
debuff --> |You got debuffed| debuffed(The enemy is gonna do more damage);
enemy --> |Oh he sick you did damage he gon fuh yo ahh now| matching;
buffed --> |oh he sick | matching;

```

## Some other Mechanic Grid Match 3 by Jason Siegersma

### 🔹 Match-3 System Summary
A dynamic tile-matching system made in Unity (C#). Tiles fall into empty spaces, refill automatically, and include a special white tile that clears all tiles of one color when matched.


![kandy](https://github.com/user-attachments/assets/0d69b7d6-a6bd-48d3-8c93-a9903f299f70)
![candy](https://github.com/user-attachments/assets/922ef085-1909-4fc0-ab47-0eca6e34bace)
![ingame phone](https://github.com/user-attachments/assets/81a2cebb-4afe-4e75-947e-8036ea774778)


## Some other Mechanic Y by Jason Siegersma

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Character classes by Jahvairo Monkau

### All characters in the game (Player and Enemy) inherit from a shared Character base class. This class contains core functionality and components that every character needs, such as:

* Character name

* Health and attack handling

* Animator reference

* Basic actions: attack, defend, take damage, and die

* This structure ensures consistent behavior and reduces code duplication across different character types.


### class diagram voor game entities:

```mermaid
classDiagram

Character --|> Enemy
Character --|> Player
Character : +string name
Character : +HealthComponent
Character : +AttackComponent
Character: +Attack()
Character: +Defend()
Character: TakeDamage(float damage)
Character: Die()

class Player{
+OnMatchMade()
}
class Enemy{
+float attackInterval
}
```

## Attack System by Jahvairo Monkau

### The AttackComponent handles character attacks using a list of AttackConfig ScriptableObjects. Each AttackConfig defines a unique attack (name, damage, animation trigger, and delay). You can assign any number of attacks via the Inspector.

### Key features:

* Flexible attack setup using ScriptableObjects

* Optional AI-controlled attack loop

* Easily switch or trigger specific attacks by index

* This setup allows you to mix and match attack behaviors per character without hardcoding logic.

![boom](https://github.com/user-attachments/assets/5bc5f6f0-6095-44fa-aeec-f0d4c918300b)


## Modular Health System Script by Gael Griffith


* ✅ Initializes health based on HealthConfig.
* ✅ Allows an entity to take damage and apply damage multipliers.
* ✅ Allows an entity to heal without exceeding max health.
* ✅ Enables health regeneration if configured.
* ✅ Stops regeneration upon death and triggers the OnDeath event.
* ✅ Supports temporary buffs for max health, damage resistance, and regen rate.
* ✅ Uses events to notify other scripts (e.g., UI updates).
* This script makes health modular, meaning you can apply it to different characters (players, enemies, bosses) with unique stats simply by assigning a different HealthConfig. 💪🔥

![Screenshot](https://github.com/user-attachments/assets/64937a8a-185f-4db1-bd6c-4fd7ea339f8a)

# Visual Sheet

## 📊 System Diagram

```mermaid
flowchart TD
    A[Character - abstract class] --> B[HealthComponent]
    A --> C[Animator]
    A --> D[HealthConfig - ScriptableObject]

    B --> E[Takes Damage]
    B --> F[Invokes OnDeath Event]
    B --> G[Initializes from HealthConfig]

    H[Player - inherits Character] --> A
    I[Enemy - inherits Character] --> A

    H --> E
    I --> E

```

## Round-Timer Script by Gael Griffith

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Damage-System (Player) Script by Gael Griffith

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Damage-System (Enemy) Script by Gael Griffith

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## BackgroundMusicManager Script by Gael Griffith

### How It Works (In Simple Terms)
* When the game starts, it checks if there’s already background music playing.

* If music is already playing, it doesn’t start a new one (so there’s no overlap).

* If no music is playing, it starts the background music.

* The script is set up so that the music continues playing across different scenes instead of restarting every time the player moves to a new area.

### Why It’s Important
* ✅ Prevents annoying music overlaps.
* ✅ Makes the game feel smoother by keeping music consistent.
* ✅ Ensures music doesn't restart every time the player moves to a new scene.

This makes the game's sound experience more immersive and polished! 🎶

```mermaid
    flowchart TD
    A[BackgroundMusicManager] --> B[Awake]
    A --> C[StartMusic]
    A --> D[StopMusic]
    A --> E[PlayMusicClip]
    A --> F[FadeInMusic]
    A --> G[FadeOutMusic]

    B --> H[Checks if MusicSource exists]
    C --> I[Plays music with a specified clip]
    D --> J[Stops music immediately]
    E --> K[Plays music clip with fading effect]
    F --> L[Gradually increases music volume]
    G --> M[Gradually decreases music volume]

```
