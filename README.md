# CMPM121-Final: Smoke in the Shadows
A project by Randy Le and Mason Kubiak <br>
Completed in the span of 3 days.

A horror game where the player is trapped in a cabin and has to defend themselves from a smoke monster. <br>
Check out our project here: https://koobysnack.itch.io/smoke-in-the-shadows

### Sound Credits:
Ghoul Sound: ibm5155, https://freesound.org/people/ibm5155/sounds/174915/

## Grading Breakdown:
### Required of all scenes:
- Particle system <br>
  - The smoke monster and the gun in the scene both rely on particle systems.
- At least 5 meaningful imported objects
  - We have a lot of objects in the scene.
- Controllable Character
  - First-person camera that follows the character
### Required for our scene (NPC Chase Scene):
- At least one NPC (Monster, or friendly) that notices the player and moves and/or runs toward them
  - A hostile monster that follows a patrol path and moves faster at the player if detected or hears gunshot.
- 2 Interactions with NPC
  - On collision with the NPC the player gets caught and dies (triggers game over). 
  - Clicking with ammo consumes it, and can be used to hurt the monster.
- A few obstacles that get in the way of the player's movement or the path of the NPC
  - Walls and Furniture.
### Polish:
- Interesting objects/materials:
  - the map itself is created from a ProBuilder Cube
  - Selection of assets (from asset store, we're both programmers) for an interesting scene.
- Sensible lighting that fits the scene
  - A low light environment that allows for just enough vision.
  - Some extra lights added for flavor.
- No default skybox or basic grey materials
  - We've got a skybox. Not that it would be seen through normal means.
  - We had to make sure the materials worked in URP, so there's no gray/pink materials in the scene
- Use of post-processing effects
  - Volumetric fog for scene darkening
  - Vignetting with intensity based on distance from monster
- Compelling environment
  - We've got sounds!
