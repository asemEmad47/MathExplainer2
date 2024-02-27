using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TwoEqsParent 
{
    public abstract void Solve();
    public abstract List<string> SolveStepByStep();
    public abstract IEnumerator SpeakAndWait(string written, int index);
    public abstract IEnumerator PlayVoiceClipAndWait(int index);
    public abstract void LoadAllAudioClips();
}
