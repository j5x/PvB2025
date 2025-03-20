# VoorbeeldExamenRepo
Een voorbeeld repository voor het examenwerk

In deze repository vind je de informatie over het examen project.

Omschrijf de examenopdracht evt de klant en wat het doel voor de klant is.
Omschrijf ook beknopt wat het idee van je game is. 
Een complete en uitgebreide beschrijving komt in het functioneel ontwerp (onderdeel van de [wiki](https://github.com/erwinhenraat/VoorbeeldExamenRepo/wiki))

# Geproduceerde Game Onderdelen

Geef per teammember aan welke game onderdelen je hebt geproduceerd. Doe dit met behulp van omschrijvingen visual sheets en screenshots.
Maak ook een overzicht van alle onderdelen met een link naar de map waarin deze terug te vinden zijn.

Bijv..

Student X:
  * [Wave System](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/src/some)
  * [Some other mechanic X](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/src/mechanic_x)
  * [Some other mechanic Y](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/src/mechanic_y)
Student Y:
  * Water Shader
  * [Some textured and rigged model](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/assets/monsters)

Student Z:
  * [Some beautifull script](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/src/beautifull)
  * Some other Game object


## Wave System by Student X

Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line.

![Animation](https://user-images.githubusercontent.com/1262745/217570184-90dc4701-d60d-4816-80d0-5007fdd3f6be.gif)

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

### class diagram voor game entities:

```mermaid
classDiagram

Unit <|-- Enemy
Unit <|-- Player
Unit : +int life
Unit : +int speed
Unit : +bool alive
Unit: +isMovable()
Unit: +Destroy()
class Tower{
+String turretType
+target()
+shoot()
}
class Enemy{
-int dmg+
-regenerates()
}
class Player{
+bool dmg+
+specialSkill()
}
```


## Some other Mechanic X by Student X

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some other Mechanic Y by Student X

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Water Shader by Student Y

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some textured and rigged model by Student Y

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some beautifull script by Student Z

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some other Game object by Student Z

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)
