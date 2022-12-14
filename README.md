# 3dSpriteBillboard
Create a threedimensional billboard sprite texture

This is a unity project, you need to download the entire folder and load it as a project in unity.
This is still work in progress, so many features might still not work or be incomplete.
With this project you can transform a 3d object into a teture with all the sprites necessary to create a billboard effect.
Billboarding is when a plane will always face towards the camera, normally is used to decrease the number of triangles rendered especially for far away object
(like trees or mountains) while maintaining a good level of realism.
You can also use Billboard to simulate 3D with by just changing sprites based at what angle are you looking a object or person (Like in DOOM (1993)).

How to use:
1-Open the project
2-Enter playmode
3-Select Main Camera in the hierarchy, attached to it there is a "Resolution Manager" script.
  List of what each option do:
  
  Type: Still work in progress, only "Plane 3D" work. The general idea is that it let you choose for what kind of billboard the texture will be used
  
  Sides: How many sides the future billboard will have (DOOM had for example 8 sides, means that the texture change every 45° of rotation,
         with 16 sides it would change after 22,5° of rotation and ect.)
         
  Resolution: The widht and Height of each sprites in pixels.
  
  Change Resolution: Check this when you want to change the beforementioned option
  
  BackGround active: Check this if you want to change background color
  
  Background Color: Change background color (if it doesn't change, it might have alpha to 0, just set it to 255)
  
  Grid active: Check this if you want a grid showing the dimension and location of each sprites
  
  Grid Color: Change grid color
  
  Texture FileName: The prefix that the texture generated will have (for example if the models name is "player" and Texture FileName is "Texture_" the final
                    texture name will be "Texture_player.png")
                    
  Material FileName: The program other than the texture will create also an unity material for testing with some shaders I've been working on,
                     you can delete the material or ignore it
                     
  Single: if you loaded more that one model but you want to save only the model showing on the screen, check this box, otherwise the program will make a texture
          for each model
          
  Create Living Plane: check this to create Texture
  
  Models: here you will load all the model that you want to create a texture (Load some models on the project assets and then drag and drop in this array)
  
  All the transform beneath (all world position, all world rotation...) are still work in progress and might not work as intended, you use them to
  move, rotate and scale the model in case it doesn't show properly on the screen
  
4-Drag and drop the model (or models) that you want to create the texture in "Models"
5-To see the model or switch use the keyboard key "M" or "N" (YOU NEED TO SET UP THE RESOLUTION FIRST, OTHERWISE IT WONT SHOW ANYTHING)
  if the model is to big, change "All Local Scale" option.
6-Check "Create Living Plane" to take a screenshoot of each model you loaded and save it as a png (might take some seconds)

