# Candy Dandy

Dit project is gemaakt als onderdeel van ons examen voor de opleiding Software/Game Development & Game Artist van Mediacollege Amsterdam. Het doel is om onze vaardigheden in game design te laten zien.

Group Members: 
Devs - Gael, Jahva, Jason

Artists - Taffie, Aurora, Esmee, Precious, Destiny

[Onze Trello](https://trello.com/b/QKXxfOid/pvb2025)

[Onze Wiki](https://github.com/j5x/PvB2025/wiki)





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

