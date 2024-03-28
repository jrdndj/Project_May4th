# Project_May4th
repository for Piano 2.0 MVP v1

<img src="eva-title.png">

### April Updates
- [x] Implement 4B-4B-4P-4P protocol
- [x] Test Mode for Lessons 1-3
- [ ] Compose Mode for Lessons 4-5
- [ ] Expand sheets add harmony Fclef
- [ ] update music info linked

### All lessons received from Sandi. Newer tasks as of 12.01.2024
- [x] Change guidances toggles to checkboxes
- [x] Add guidance features: metronome and rhythm
- [x] music sheets work! 
- [x] Music sheets as display and condition 2

### All features working as of 30.11.2023 ~~Just pending Lesson 3-5 from Sandi~~ ALREADY ADDED THEM! 

### Post Study Release Targets 
- [x] Lesson 1: Play Swing Through Modes
- [x] Lesson 2: Learn Sequences Through Repetition
- [x] Lesson 3: Learn Motifs Through Phrasing
- [x] Lesson 4: Learn Variations
- [x] Lesson 5: Ques-Ans

### open issues and targets
- [x] MAJOR cleanup
- [x] new layout
- [x] deploy webserver for receiving inputs
- [x] capture logs into MIDI-able format
- [x] handled rests
- [x] add Show Piano Roll and Play Audio feature toggles OR
~- [ ] integrate DAW for music synthesis live OR (all below)~
~- [ ] show staff for Play audio feature ~
~- [ ] use ParseMIDI to convert logs into .mid files~
- [x] drop .mid files with same ID for server to send to synthesizer
- [x] sync timing with recorded elements
- [x] sync timing of highlighting
- [x] process sibelius add tempo
- [x] syncing of midi and aiff
- [x] time x pixelmap
- [x] multiple loading of several audio assets
      

#### for each lessons, the following modes will be designed and implemented
Modes and Features
- [x] Watch and Listen Mode
- [x] Try Mode
- [x] Test Mode
- [x] Toggles for Content (C) only, Content and Rhythm (CR) only, Content and Rhythm and Harmony (CHR) only and Content and Rhythm and Harmony and Metronome (CHRM) only

New Assets Needed
- [x] recordings from iRealPro
- [x] recordings of the different C, R, H, M

Newer classes will be implemented to apply these changes
- [x] ModeMgr - manages the different Modes namely WaL, Try and Test
- [x] LessonMgr - manages the content of the lesson based on the toggled C,R, H, M
- [x] VizMgr - will be revised to reflect the new version of toggles in the prototype. 

### reBuild requirements as of 11.04.2023

#### design elements
Controllers 
 * RollScript - responsible for spawning and rolling piano roll
 * TimeMgr - for time control, timebar generation, tempo management, time signature etc
 * ImprovMgr - manages all highlighted keys that produces viz 
 * InputMgr - manages scoring, timing in relation to user press and chord information
 * VizMgr - contains all toggle-able checkboxes that produces "adaptive" dynamic options for the user

Models
 * ChordMgr - contains all chord-key piano information 
 * CollectionMgr - contains all chord-improv-pairing information. 
 
Views are integrated into the design of the classes. In principle we can see Builder, Factory, Wrapper and Iterator elements among our design. 

More information about dependencies and relationships will be revealed soon as we finalise the design. 

#### forms of visualisation
- [x] falling piano roll with recommended improv licks
- [x] press-detected improvisation (and valid while still on press)
- [x] highlighting of sample improvisation licks 

#### new targets 
- [x] create classes
- [x] integrate them with the piano canvas environment
- [x] migrate existing functions from now-decommissioned RollScript.cs
- [x] implement TimeMgr 
- [x] implement ImprovMgr
- [x] implement ChordMgr
- [x] implement InputMgr
- [x] cleanup RollScript to its new intended purpose
- [?] find a way to implement MapLines() - discussed with bosses. 
- [x] rework RollScript.cs specifically SpawnKeys(), RollKeys() and ~~MapLines()~~ functions
- [x] implement CollectionMgr
- [x] implement passing of information based on targetted dependencies
- [x] implement VizMgr and toggle 
- [x] demo to bosses
- [x] Study 1 with Improv Teachers
- [ ] Develop Post-Study Version - ONGOING 
- [ ] Study 2 with Improv Students - per lesson
- [ ] Study 3 on Spatiotemporal Data collection
- [ ] Graduate
 
### MVP version ready by ~22 Aug~ ~I DONT EVEN KNOW ANYMORE~ AY WE HERE BRUH 

### v1 requirements

#### hardware details ===

- MIDI prototype model: Kurzweil K2000 v3 [reference](https://kurzweil.com/k2000/#faqs), [manual](https://kurzweil.com/wp-content/uploads/2019/10/Setup_Mode.pdf).

#### software details ===

#### latest updates as of ~~12/07/2022~~ 07.02.2023

target completion of MVP 12/08/2022  ~~13/11/2021~~

~~#### latest updates as of 16/11/2021~~

~~target completion of MVP 13/11/2021 30/11/2021~~

#### targets for an minimum viable product (MVP) - ALL DONE

##### hardware targets
- [x] setup MIDI input with laptop
- [x] see how input data looks like
- [ ] ~~\(Optional) set output in a target file destination~~
- [x] allow user to press key
- [x] ~~play sound via garageband~~
- [x] identify which key is pressed (return keybinding) 
- [x] projector setup
- [x] laser cutting of projection board
- [x] applying white paint/tape on black keys to assist projection visuals
- [x] applying subtle key labels for new users
- [x] solve issues with keys C6 to G6 having a delayed startup (significant delay)
- [x] setup back stand of white screen for stability 

##### software targets 
- [x] setup unity environment for the piano roll engine
- [x] setup keybindings in unity
- [x] setup new input system in unity
- [x] parse midi into chord information and load them in unity environment (at least one midi file) 14.07.2022
- [x] convert chord information unity applicable assets and information (yscale, location, etc)
- [x] setup virtual piano environment (keys, finger colors, aspect ratio)
- [x] calibrate virtual piano environment with the existing setup
- [x] overlay piano roll sequence onto virtual piano environment
- [x] synthesize sound - used garageband
- [x] tracking of keypress along with piano roll
- [x] black layout
- [x] piano roll melody
- [x] improv suggestive based on bars
- [x] error checking - melody
- [x] error checking - licks 
- [x] logging of spatiotemporal data - user presses
- [ ] logging of spatiotemporal data - correct presses
- [ ] scoring 
- [ ] export spatio temporal data with musicxml into csv 
- [ ] export everything as functions 
- [ ] toggle feature feature (yes this is not a typo)

###### single chord
- [x] spawn a key from this chord information
- [x] move piano roll key
- [x] destroy upon collision with green line
- [x] test for timing and accuracy 
- [x] integrate with piano midi input
- [x] destroy object upon click

###### multiple chords
- [x] add timer - 
- [x] generate co-routines to time multiple spawns - 
- [x] generate co-routines for spawn movement (transform position) - 
- [x] generate data structures (list) to store piano key coordinates
- [x] test for timing and accuracy 
- [ ] ~~integrate with piano midi input~~
- [x] destroy object upon click

##### some other tasks due to restructuring
- [ ] ~~setup music xml with visual interface~~
- [x] ~~connect music xml to unity engine~~
- [ ] ~~configure musicxml to generate viz~~
- [ ] ~~stream songs from musicxml to unity engine~~
- [x] followed the steps from https://answers.unity.com/questions/1434686/how-to-integrate-drywetmidi-net-46-library-in-unit.html 
- [x] check consistency and correctness of samplesongs
- [x] design scalable SOLID facade for adaptation engine
- [x] file browser integration for song selection
- [x] skeletal menu from home to practice song
- [x] logging of keypresss
- [x] acquiring temporal information from the midi
- [x] ~~synthesize sounds for C2 to B3 and C4 to C7. include sharp/flat keys~~ 
- [x] MIDIFileSequencer (to convert .mid into List<Node> which can be used by LoadMIDIFile.cs)
- [x] NoteDurationObject for use of SpawnNotesDropDown 
- [x] integrate DryWetMidi
- [x] port to v 2017.4.8.f1 

##### integration targets
- [x] setup external speakers to play synthesized sound
- [ ] run pilot user study 
- [x] measure projection board hind legs
- [z] acquire materials for projection board hind legs 

 
#### revised schedule ===
- [ ] ~~MVP ready for initial spatiotemporal data collection ~~13/11/2021~~ 30/11/2021~~
- [ ] ~~spatiotemporal data collection until ~~13/12/2021~~ 23/12/2021~~
- [ ] ~~data cleaning and analysis~~
- [ ] ~~regression modeling~~
- [ ] ~~UIST paper ~~13/01/2021~~ 23/01/2021~~
  
###### @jrdndj

