# Unity Quick Material Creator
One-click solution to convert non-editable "ghost" materials from imported models into fully editable materials. Right-click any grayed-out material to instantly create, copy properties, and replace it - even with multiple objects selected.

![image](https://github.com/user-attachments/assets/0da23cf8-1ed5-404f-bd6d-53b5ef7b3970)

# The Solution
This tool adds a simple "Create New Material From This" option directly in the material's context menu. One click will:

- Create a new material with the Standard shader
- Copy the original texture and color settings
- Save it in your Materials folder
- Automatically replace the original material
- Works with multiple selected objects!

![image](https://github.com/user-attachments/assets/935c9259-3485-4e1f-ae7c-3ae1f31e84f2)
![image](https://github.com/user-attachments/assets/83a633d2-e4b7-4e34-9a49-871f46a54fb5)

# Installation
- Create an Editor folder in your Unity project if you don't have one
- Download MaterialMenuExtension.cs and place it in the Editor folder
- That's it! The option will appear in your material context menus

# Usage

- Select one or more objects with imported/existing materials
- In the Inspector, right-click on the material you want to make editable
- Select "Create New Material From This"

![image](https://github.com/user-attachments/assets/86eb35d9-a5f0-48ee-99e6-56982ee43926)

The script will create a new material and automatically replace it on all selected objects that use that material.
