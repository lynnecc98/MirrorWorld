I. QUICK START

Welcome to Fast Cutscene Producer!

The app is quite simple to operate. In an usual workflow, you would:
 
1. Add a "Fast Cutscene Producer" prefab to your scene.

2. Select the characteristics you want in the Director component and add an Actor by clicking the "Add Actor To Cast" button.

4. Rename the "Actor Name" to your liking,  and drag a 3D model,  a camera or a script to the "Actor Transform" field.  

If the object you just dragged has animations in it, the Actor component will automatically show a "Use Animations" checkbox, where you can select the animations you wanna use.

5. In the Actor component, click the "Add Line to Cutscene Script" button to add a new line to it.

6. Enter the information and characteristics you want in the Line component.

And that's it! By doing that you have a cutscene with a Director, several Actors, and several Lines for each Actor.




II. ADVANCED FEATURES - HANDLING LARGE CUTSCENE SCRIPTS


With more experience, your cutscenes will likely become more complex and longer. 

When you feel you're taking too long to scroll through all the Line components of your script, there are two solutions:

II.1 One solution is to stretch the Inspector window horizontally. The script layout will automatically adjust itself to show more lines in each screen.

II.2 The second solution is to subdivide your cutscene in chapters by clicking "Add New Chapter to Cutscene Script" in the Director component. 

The "Add New Chapter to Cutscene Script" merely adds a new empty game object under the Director where you can add more Lines to it. It is merely organizational, as you can even manually add an empty game object to put more Lines. The Director component reads all the Line components under it, regardless of what game object they are on.


III. IMPORTING AND EXPORTING SCRIPTS (PRO VERSION)

Fast Cutscene Producer can import or export your cutscene scripts for backup purposes or to keep different versions of the same cutscene. When you export a script, it also creates a ".txt" file where you can verify the saved script in a readable format.


IV. FAST FORWARDING CUTSCENES (PRO VERSION)

While in Play mode, the Director shows a new toolbar with speeds from 2x to 16x to fast forward the cutscene.


V. INSERTING AND REORDERING LINES IN THE MIDDLE OF A SCRIPT (PRO VERSION)

As scripts become longer, it can become cumbersome to add new lines in the middle of it. To solve this, you can click the "Insert Line Before..." button in the Actor component or the "Move Line To Before..." button in the Line component, and it will insert or move a Line to that position.


