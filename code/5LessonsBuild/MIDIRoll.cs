//this script was written with the help of GPT 
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MIDIRoll : MonoBehaviour
{
    public static MidiFile midiFile; // MIDI file asset
    public GameObject notePrefab; // Prefab for representing MIDI notes
    public float pixelsPerBeat = 40.0f; // Width of one beat in pixels
    public float beatHeight = 25.0f; // Height of one beat in pixels

    private void Start()
    {
        GeneratePianoRoll();
    }

    private void GeneratePianoRoll()
    {
        MidiFile midi = MidiFile.Read("some filename here"); // Read the MIDI file
        var tempoMap = midi.GetTempoMap();

        var notes = midi.GetNotes(); // Get all the MIDI notes

        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {
            var noteTime = note.TimeAs<MetricTimeSpan>(tempoMap);
            var noteDuration = note.LengthAs<MetricTimeSpan>(tempoMap);

            int noteNumber = note.NoteNumber;

            // Calculate X position in pixels based on note.Time
            float xPosition = (float)noteTime.TotalMicroseconds / 1000.0f * pixelsPerBeat; // Convert microseconds to milliseconds

            // Calculate Y position (height) based on note.NoteNumber
            float yPosition = (noteNumber - 24) * beatHeight; // Assuming MIDI note C2 is note 24

            // Calculate the width of the object based on note.Duration
            float width = (float)noteDuration.TotalMicroseconds / 1000.0f * pixelsPerBeat; // Convert microseconds to milliseconds

            // Create a new instance of the prefab and set its position and size
            GameObject noteObject = Instantiate(notePrefab);
            noteObject.transform.position = new Vector3(xPosition, yPosition, 0.0f);
            noteObject.transform.localScale = new Vector3(width, beatHeight, 1.0f);
        }
    }
}
