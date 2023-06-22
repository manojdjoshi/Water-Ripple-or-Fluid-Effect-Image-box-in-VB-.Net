# Water Ripple or Fluid Effect Image box in VB.Net

Introduction
Add a splash to your splash screen - literally!

Finally a VB.NET implementation of the classic ripple / water / splash / fluid effect! This is a port and a enhancement of Christian Tratz's C# implementation.

Background
Even VB.NET users should be able to have awesome effects. There are many fluid effect routines out there, most in C. This is a VB.NET port of the C# implementation. I've also opened up a few more properties so you can have greater control over the effect programatically. You can make some really neat looks by fiddling with the parameters - make your image appear to be rubber, or under an oil surface, or use regular old water physics. Just check out the sample app and the documentation in the code itself.

See http://www.x-caiver.com/Software for additional versions.

Using the code
In just a few steps you will be set up:

Add fluidEffect.vb to your project
On your form, add a Panel object. Position/size/name appropriately. Don't add a graphic to it yet.
Save your form, then switch to the Code View and close the Designer View.
Search for the Panel name, make sure 'Search hidden text' option is selected. There are two instances you are looking for. Both are within the "Windows Form Designer generated code" region. The 2 examples below assume you named your panel myFluidPanel.
Watch out, VS.NET likes to change the two lines to be <yournamespace>.fluidEffect.fluidEffectControl and then the Designer gets confused, and won't display it at all until you go back to the Code and fix it.
VB.NET
'This was the original line. Comment it out and add the line below
'Friend WithEvents myFluidPanel As System.Windows.Forms.Panel
Friend WithEvents myFluidPanel As fluidEffect.fluidEffectControl
VB.NET
'This was the original line. Comment it out and add the line below
'Me.myFluidPanel = New System.Windows.Forms.Panel()
Me.myFluidPanel = New fluidEffect.fluidEffectControl()
Save again. Now you can open up the form in Designer View
Select your panel (now a fluidEffect), and scroll to the "Misc" section of its property sheet, and set the ImageBitmap entry to your bitmap. (the fluidEffect only works with bitmaps for now. it will display a jpg (or any format a Panel can show) in the Designer but won't work when you try to run it)
Configure any of the other settings that you want, Save, and you're done!
Settings you can change:

ImageBitmap - this is the image displayed. Bitmaps only for now
dropHeight - larger values make the initial drops higher
dropRadius - larger values make the radius of the initial ripple larger
Dampener - larger values let the ripples last longer. 4 is normal. 2 & 1 make the fluid very thick so the ripples die fast. A value of 0 has basically no ripples
TheScale - Value of 0 is a ripple bitmap the same size as the image, value 1 is half the size, etc. Larger values are useful if you have a big image to make the processing take less time. Also if you scale the image on your form you should try values 1 and up to get the drops to be correctly located in relation to the mouse
StopRipples - This value should be set to False for normal use. Setting it to True will freeze the ripples. Clicking on the image again will start a new ripple, and un-freeze all the existing ripples.
Gotchas:

Only works with bitmaps for now, but you can assign a jpg/icn/etc to the panel from the Designer View
Gets bogged down quickly with larger images. A 500x400 image of mine could only get ~ 2 fps with the settings I had selected
There are error cases that aren't trapped yet. Launch the demo app, and click a lot, quickly, and you will crash the app. (I was able to get 30 drops going at once clicking slowly, but if I clicked quickly I could crash the app after 5 drops)
When 2 & 3 are addressed I will reactivate the drag event handler. This lets you "draw" on the water, like running your finger over the water.
Points of Interest
VB.NET is not the mathematical, memory swapping powerhouse that C# is. This code bogs down quickly under load. One of my apps has a 500 x 400 pixel splash screen, with transparent sections. I wasn't able to get good frame rates out of it. Smaller images work great though. I selected an interesting spot in one of my splash screens that is only 100x100, and it can ripple in realtime.
