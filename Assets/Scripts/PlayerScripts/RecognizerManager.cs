using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecognizerManager : MonoBehaviour {

    public DollarRecognizer recognizer;
    private SpellCaster spellCaster;
    [SerializeField] private Object[] sampleResources;
    [SerializeField] private DollarRecognizer.Result result;

    private void Start() {
        recognizer = new DollarRecognizer();
        spellCaster = FindObjectOfType<SpellCaster>();
        LoadLineSamples();
    }
    public void Recognize(List<Vector3> input) {
        List<Vector2> translatedInput = input.Select(p => new Vector2(p.x, p.y)).ToList();
        result = recognizer.Recognize(translatedInput);
        Debug.Log(result.Match.Name);
        spellCaster.SpellCastRouter(result.Match.Name);
    }
    private void LoadLineSamples() {
        sampleResources = Resources.LoadAll("LineAssets", typeof(LineAsset));
        foreach (LineAsset s in sampleResources) {
            recognizer.SavePattern(s.sampleName, s.samplePoints);
        }
    }
}