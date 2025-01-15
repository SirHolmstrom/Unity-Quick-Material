# Unity Quick Material Creator
One-click solution to convert non-editable "ghost" materials from imported models into fully editable materials. 
Right-click any grayed-out material to instantly create, copy properties, and replace it - even with multiple objects selected.

![image](https://github.com/user-attachments/assets/fd47d565-0c3c-4248-8328-a251d408961e)

# The Solution
This tool adds a simple "Create New Material From This" option directly in the material's context menu. One click will:

- Create a new material with the Standard shader
- Copy the original texture and color settings
- Save it in your Materials folder
- Automatically replace the original material
- Works with multiple selected objects!

![image](https://github.com/user-attachments/assets/97b71270-c9a9-459e-acb6-93c9f406f4ee)
![image](https://github.com/user-attachments/assets/6ce6a151-4435-4bfc-befd-bc7015ee180e)

# Installation
- Create an Editor folder anywhere in your Unity project if you don't have one
- Download MaterialMenuExtension.cs and place it in the Editor folder
- The option will appear in your material context menus

# Usage
- Select one or more objects with imported/existing materials
- In the Inspector, right-click on the material you want to make editable
- Select "Create New Material From This"

![image](https://github.com/user-attachments/assets/350a80b4-0826-4523-9fa7-de36b7f2a195)
The script can create a new material and automatically replace it on all selected objects that use that material.
