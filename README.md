
# Woodblock
[![openupm](https://img.shields.io/npm/v/com.rsherriff.woodblock?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.rsherriff.woodblock/)

Woodblock is a simple game manager and UI for Unity that allows the playing of games written in Ink.

It initially seeks to recreate the UI of the InkJS player allowing users with no scripting experience to customise and publish INK games with Unity.

It also introduces new functionality to this basis, audio and images. All controlled via tags in the Ink file rather than requiring scripting in Unity.

Finally, it also supports saving and loading (currently into one slot). Saving and Loading also works in web builds.

Woodblock is intended for student/hobbyist or small projects as its asset management of images and audio is done in a naive way. It could be extended to use more professional techniques such as asset bundles but this is not currently implemented.

## Use

Example Project available at: https://github.com/rSherriff/woodblock-example

### Basic Use
1. In your Unity project create an instance of the Prefab Woodblock game from the package. 
2. Create a Woodblock Data file via Assets -> Create -> Woodblock and place your INK json file in the socket for it (a font will also need to be set as this is not done by default). 
3. Put this data file in the the socket for it in the Ink Story Adaptor of the Woodblock Game gameobject (the root of the prefab). 
4. Hit play to start your story!


### Advanced Use
Woodblock has support for adding audio and images to your INK story.
#### Audio
Audio is added in two ways:

 - Claps - Claps are one off pieces of audio that play and do not loop or repeat until you call them again. 
 - Ambient - Ambient sounds are looping pieces of audio, think background music or soundscapes. Only of these can be playing at a time.
 
Sounds are added in INK like so:

	#CLAP sound_name 
	#AMBIENT sound_name 
	
Sounds are stored in Unity in the Resources/Audio/Claps or /Ambient directories.

#### Images
Images are added via INK tags like so:
	`#IMAGE image_name optional_width optional_height				`

Images are stored in Unity in the Resources/Images directory
If you do not specify a width or a height, Woodblock will detect whether your image is in portrait or landscape and use the default values specified in your games data file.

##### Dependencies

    com.unity.textmeshpro 3.0.1
    com.inklestudios.ink-unity-integration 1.0.2
