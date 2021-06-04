# Project_May4th
repository for Piano 2.0 MVP v1

### MVP version ready by 04 May 2021


#### hardware details ===
- MIDI prototype model: Kurzweil K2000 v3 [reference](https://kurzweil.com/k2000/#faqs), [manual](https://kurzweil.com/wp-content/uploads/2019/10/Setup_Mode.pdf).

#### software details ===

#### targets

##### hardware targets
- [x] setup MIDI input with laptop
- [ ] see how input data looks like
- [ ] \(Optional) set output in a target file destination
- [ ] allow user to press key
- [ ] identify which key is pressed (return keybinding) 

##### software targets 
- [x] setup unity environment for the piano roll engine
- [x] setup music xml with visual interface
- [x] connect music xml to unity engine
- [x] configure musicxml to generate viz
- [x] stream songs from musicxml to unity engine
- [x] check consistency and correctness of samplesongs
- [ ] design scalable SOLID facade for adaptation engine

##### integration targets

###### some notes from @wilmol

How to convert midi file into json string?
Looking at MidiFileSequencer:
We used a 3rd party library (DryWetMidi) which turns a midi file into List<Note> (See method: public void LoadMidiFile(string file))
Then we create NoteDuration objects from each note. (See method: private void SpawnNotesDropDown(List<Note> notes))
Then use another 3rd party library (JsonDotNet) to save the JSON from List<NoteDuration> This works because NoteDuration is serialisable with [DataContract], (see the bottom of that file).

###### @jrdndj

