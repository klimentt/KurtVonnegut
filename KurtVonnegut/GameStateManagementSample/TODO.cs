using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class TODO
    {
        //TODO: IMPORTANT, look at some of the implimentations ( like hit delay on melee attacks for the enemy classes ), look the class inheritance, and if you make new classes, always inherit the existing ones. Best ones to inherit are "Solid", "Player", "FlyingEnemy", and "Enemy".
        //TODO: Integrate the classes in folder "GameObject" so they inherit "Player", or "FlyingEnemy" or "WalkingEnemy". Use the existing interfaces, and impliment everything so it works in the environment.
        //TODO: Make the player use melee weapons, and have an inventory. 
        //TODO: Make the player and enemies "Top down ( so they work well with rotation and do not look upside down ( like "http://gamefroot.com/wp-content/files/2013/12/topdown-sheet3.png" or http://www.synapsegaming.com/cfs-filesystemfile.ashx/__key/CommunityServer.Components.ImageFileViewer/CommunityServer.Discussions.Components.Files.16/6864.dude_5F00_sheet.jpg_2D00_550x0.jpg )
        //TODO: Integrate weapons to the player, and make it so they have an animation when he uses them
        //TODO: Make levels larger ( and increase screen size ) so we can have the level on the screen
        //TODO: Create a class that will hold initial positions of each enemy, player, wall, turret etc. Make all necessary classes Serializable and use files for level control
        //TODO: Move XP gain to the player class, and impliment levels
        //TODO: Impliment player skills ( like a shield, he can change color while shielded for a duration and not take damage )
        //TODO: Impliment an AOE skill for the player
        //TODO: Fix the hitboxes. Currently I have implimented rectangle hit boxing that works badly with rotation. It can be fixed if the player and enemies hit detect with a radius instead.
        //TODO: The shoot projectile must be a skill with larger cooldown, and main attacks must be melee
        //TODO: Impliment a "Box" class that inherits "Solid" but can be moved if the player pushes it, and it has HP and can die from melee attacks
        //TODO: Fix music so its looping, and use different tracks for menu, and inside the game. Also impliment audio control in the menu
        //TODO: Character creating in the menu, choosing skills and weapons from there.
    }
}
