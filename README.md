# Project_May4th
repository for Piano 2.0 MVP v1

### MVP version ready by 12 Aug 2022


#### hardware details ===

- MIDI prototype model: Kurzweil K2000 v3 [reference](https://kurzweil.com/k2000/#faqs), [manual](https://kurzweil.com/wp-content/uploads/2019/10/Setup_Mode.pdf).

#### software details ===

#### latest updates as of 12/07/2022

target completion of MVP 12/08/2022  ~~13/11/2021~~

~~#### latest updates as of 16/11/2021~~

~~target completion of MVP 13/11/2021 30/11/2021~~


#### targets for an minimum viable product (MVP) 

##### hardware targets
- [x] setup MIDI input with laptop
- [x] see how input data looks like
- [ ] ~~\(Optional) set output in a target file destination~~
- [x] allow user to press key
- [x] ~~play sound via garageband~~
- [x] identify which key is pressed (return keybinding) 
- [x] projector setup
- [ ] laser cutting of projection board
- [x] applying white paint/tape on black keys to assist projection visuals
- [x] applying subtle key labels for new users
- [x] solve issues with keys C6 to G6 having a delayed startup (significant delay)
- [ ] setup back stand of white screen for stability 

##### software targets 
- [x] setup unity environment for the piano roll engine
- [x] setup keybindings in unity
- [x] setup new input system in unity
- [x] parse midi into chord information and load them in unity environment (at least one midi file) 14.07.2022
- [ ] convert chord information generated into piano roll sequence 
- [x] setup virtual piano environment (keys, finger colors, aspect ratio)
- [ ] calibrate virtual piano environment with the existing setup
- [ ] overlay piano roll sequence onto virtual piano environment
- [ ] synthesize sound (using FMOD perhaps?) 
- [ ] tracking of keypress along with piano roll
- [ ] scoring 
- [ ] logging of spatiotemporal data 
- [ ] export spatio temporal data with musicxml into csv 



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
- [ ] ~~synthesize sounds for C2 to B3 and C4 to C7. include sharp/flat keys~~ 
- [ ] MIDIFileSequencer (to convert .mid into List<Node> which can be used by LoadMIDIFile.cs)
- [ ] NoteDurationObject for use of SpawnNotesDropDown 
- [ ] integrate DryWetMidi
- [x] port to v 2017.4.8.f1 

##### integration targets
- [ ] setup external speakers to play synthesized sound
- [ ] run pilot user study 
- [ ] measure projection board hind legs
- [ ] acquire materials for projection board hind legs 

###### some notes from @wilmol

How to convert midi file into json string?
Looking at MidiFileSequencer:
We used a 3rd party library (DryWetMidi) which turns a midi file into List<Note> (See method: public void LoadMidiFile(string file))
Then we create NoteDuration objects from each note. (See method: private void SpawnNotesDropDown(List<Note> notes))
Then use another 3rd party library (JsonDotNet) to save the JSON from List<NoteDuration> This works because NoteDuration is serialisable with [DataContract], (see the bottom of that file).

 
#### revised schedule ===
- [ ] ~~MVP ready for initial spatiotemporal data collection ~~13/11/2021~~ 30/11/2021~~
- [ ] ~~spatiotemporal data collection until ~~13/12/2021~~ 23/12/2021~~
- [ ] ~~data cleaning and analysis~~
- [ ] ~~regression modeling~~
- [ ] ~~UIST paper ~~13/01/2021~~ 23/01/2021~~
  
###### @jrdndj

