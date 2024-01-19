## Integrating ABC Notation into Unity Piano 2.0 

Based on reference from https://github.com/matthewcpp/ABCUnity 

- [x] Add downloded repo extracted zip into Assets folder
- [x] TextMeshPro TMP has been added 
- [x] Checkout SampleScene 
- [x] Checkout BasicLayout.cs in Samples folder 
- [x] Have an area with rectransform such as ABCLayout to contain the sprites. This RectTransform area will be filled by the sprites. 
- [x] add whitebackground
- [ ] make the whitebackground work with the rendered sprites
- [ ] provide function that updtes the fie 
## Recreating Sample Scene
- [x] Add gameobject ABCLayout Prefab into the scene just below camera. 
- [x] ABCLayout has Layout script attached as a component. Set its recttransform as you wish it to be
- [x] check the following: NoteAtlas as sprite atlas, Note as Note material, staff line pdding defaults etc
- [x] some equivalent of gameobject Sample where Basic Layout script is attached. Resource name is the notation of the file we use. In this way ABCNotationHandler is added. 
- [x] Check out MainCamera configurations so far 
- [ ] find a way to subsitute the stringname here depending on the user choice and what is loaded
- [ ] In the same folder as the scene, the BasicLayout script is there and the Resources folder which has the abc notation files.

