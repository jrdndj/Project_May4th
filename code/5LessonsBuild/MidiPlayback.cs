using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

public class MIDIPlayer : MonoBehaviour
{
    public TextAsset midiFile; // Assign the .mid file in the Inspector

    private MidiFile _midiFile;
    private Melanchall.DryWetMidi.Multimedia.Playback _playback;

 

    void Start()
    {
        _midiFile = MidiFile.Read("Assets/MusicXML/ImprovLicks/Lick03.mid");
       _playback = _midiFile.GetPlayback();

       _playback.Start();
    }

    void OnDestroy()
    {
        _playback.Dispose();
    }
}
