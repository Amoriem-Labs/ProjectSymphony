using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.Events;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;

namespace RhythMidi
{
    class Ref<T>
    {
        public T value;
    }

    public class ChartResource
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Mapper { get; set; }

        public string MidiFilepath { get; set; }
        public byte[] MidiFileData { get; set; }
        public AudioClip BackingTrack { get; set; }
        public Dictionary<string, AudioClip> Tracks { get; set; }

        public ChartResource()
        {
            Tracks = new Dictionary<string, AudioClip>();
        }
    }

    public class NoteNotifier
    {
        public float TimeInAdvance { get; private set; }
        public UnityAction<Note> OnNote { get; set; }
        public Queue<Note> Notes { get; private set; }
        RhythMidiController.NoteFilter noteFilter;

        public NoteNotifier(float timeInAdvance, RhythMidiController.NoteFilter noteFilter = null)
        {
            TimeInAdvance = timeInAdvance;
            Notes = new Queue<Note>();
            OnNote = delegate { };
            this.noteFilter = noteFilter;
        }

        public void EnqueueNotes(IEnumerable<Note> noteList)
        {
            Notes.Clear();
            foreach (Note note in noteList)
            {
                if (noteFilter != null && !noteFilter(note)) continue;
                Notes.Enqueue(note);
            }
        }

        public void Clear()
        {
            Notes.Clear();
        }
    }

    public class RhythMidiController : MonoBehaviour
    {
        public static RhythMidiController Instance { get; private set; }

        [SerializeField]
        [Tooltip("The path, relative to StreamingAssets, of the directory that contains all chart directories. Leave blank to not load any charts on Start.")]
        private string chartsPath = "Charts";

        List<AudioSource> audioSources;

        [Tooltip("Finished when LoadAllFromStreamingAssets finishes.")]
        public UnityEvent onFinishedLoading = new UnityEvent();

        public List<NoteNotifier> noteNotifiers = new List<NoteNotifier>();
        public bool IsPlaying { get; private set; }

        private List<ChartResource> loadedCharts = new List<ChartResource>();
        public ChartResource currentChart;
        private MidiFile midiData;
        public TempoMap CurrentTempoMap { get; private set; }
        public AudioSource backingTrackSource => audioSources.Count > 0 ? audioSources[0] : null;
        
        public delegate bool NoteFilter(Note note);

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            if (chartsPath.Length > 0) LoadAllFromStreamingAssets(chartsPath);
            audioSources = gameObject.GetComponents<AudioSource>().ToList();
        }

        public void ClearCallbacks()
        {
            noteNotifiers.Clear();
        }

        /// <summary>
        /// Loads a single chart from a specified filepath to the chart directory.
        /// </summary>
        /// <param name="path">The absolute filepath to the chart.</param>
        /// <param name="onChartLoaded">An optional UnityEvent to invoke when the chart is loaded.</param>
        public void LoadChart(string path, UnityEvent onChartLoaded = null)
        {
            StartCoroutine(LoadChart_Coroutine(path, onChartLoaded));
        }

        /// <summary>
        /// Loads all charts from a specified directory inside StreamingAssets.
        /// 
        /// NOTE: This method is only available on platforms with System.IO, i.e. not WebGL.
        /// </summary>
        /// <param name="chartsPath">The path, relative to StreamingAssets, of the directory that contains all chart directories.</param>
        public void LoadAllFromStreamingAssets(string chartsPath)
        {
            StartCoroutine(LoadAllFromStreamingAssets_Coroutine(chartsPath));
        }


        private IEnumerator LoadAllFromStreamingAssets_Coroutine(string chartsPath)
        {
            string chartDirPath = Path.Combine(Application.streamingAssetsPath, chartsPath);
            // string[] chartDirectories = Directory.GetDirectories(chartDirPath); this was causing a WebGL error
            string[] chartDirectories = new string[] { "Daylo", "Prelude", "Sam", "Week1", "Week2", "Week3" };
            foreach (string directory in chartDirectories)
            {
                string path = Path.Combine(chartDirPath, directory); //added line to fix WebGL error
                yield return LoadChart_Coroutine(path);
                // yield return LoadChart_Coroutine(directory);
            }
            onFinishedLoading.Invoke();
        }

        private IEnumerator LoadChart_Coroutine(string directory, UnityEvent onChartLoaded = null)
        {
            string manifestPath = Path.Combine(directory, "manifest.json");

            Ref<string> manifestData = new Ref<string>(); // Ref? it's how you do out params in coroutines
            yield return ReadFile(manifestPath, manifestData);

            JSONNode manifest = JSON.Parse(manifestData.value);
            string title = manifest["title"];
            string artist = manifest["artist"];
            string mapper = manifest["mapper"];
            string midi = manifest["midi"];
            JSONNode tracks = manifest["tracks"];

            ChartResource chart = new ChartResource();
            chart.Title = title;
            chart.Artist = artist;
            chart.Mapper = mapper;

            chart.MidiFilepath = Path.Combine(directory, midi);
            yield return LoadMidiFile(chart.MidiFilepath, chart);

            foreach (string key in tracks.Keys)
            {
                string trackPath = Path.Combine(directory, tracks[key].Value);
                yield return LoadTrackAudioClip(trackPath, key, chart);
            }

            string backingTrackPath = Path.Combine(directory, manifest["backingTrack"]);
            yield return LoadTrackAudioClip(backingTrackPath, "__BACKING", chart);

            loadedCharts.Add(chart);
            if (onChartLoaded != null) onChartLoaded.Invoke();
        }

        private IEnumerator ReadFile(string path, Ref<string> data)
        {
            string platformCorrectedPath = "file://" + path;
#if UNITY_WEBGL && !UNITY_EDITOR
            platformCorrectedPath = path;
#endif

            Debug.Log("Reading file from path: " + platformCorrectedPath); // Print the file path
            using (UnityWebRequest www = UnityWebRequest.Get(platformCorrectedPath))
            {
                yield return www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception("Error reading file: " + www.error);
                }
                else
                {
                    data.value = www.downloadHandler.text;
                }
            }
        }

        private IEnumerator LoadTrackAudioClip(string path, string key, ChartResource chart)
        {
            string platformCorrectedPath = "file://" + path;
#if UNITY_WEBGL && !UNITY_EDITOR
            platformCorrectedPath = path;
#endif
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(platformCorrectedPath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception("Error loading audio clip: " + www.error);
                }
                else
                {
                    if (key == "__BACKING")
                    {
                        chart.BackingTrack = DownloadHandlerAudioClip.GetContent(www);
                    }
                    else
                    {
                        chart.Tracks[key] = DownloadHandlerAudioClip.GetContent(www);
                    }
                }
            }
        }

        private IEnumerator LoadMidiFile(string path, ChartResource chart)
        {
            string platformCorrectedPath = "file://" + path;
#if UNITY_WEBGL && !UNITY_EDITOR
            platformCorrectedPath = path;
#endif
            using (UnityWebRequest www = UnityWebRequest.Get(platformCorrectedPath))
            {
                yield return www.SendWebRequest();
                while (!www.isDone)
                {
                    yield return null;
                }
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception("Error loading MIDI file: " + www.error);
                }
                else
                {
                    chart.MidiFileData = www.downloadHandler.data;
                }
            }
        }

        /// <summary>
        /// Gets the information about a chart by its name. FYI, O(n) complexity.
        /// </summary>
        /// <param name="title">The name of the song</param>
        /// <returns>The chart info</returns>
        public ChartResource GetChartByName(string title)
        {
            return loadedCharts.Find(x => x.Title == title);
        }

        /// <summary>
        /// Loads a chart's notes into memory. This must be called before PlayChart.
        /// </summary>
        /// <param name="title">The name of the song</param>
        /// <exception cref="Exception"></exception>
        public void PrepareChart(string title)
        {
            currentChart = GetChartByName(title);
            if (currentChart == null) throw new Exception("Chart not found.");

            using (MemoryStream memoryStream = new MemoryStream(currentChart.MidiFileData))
            {
                midiData = MidiFile.Read(memoryStream);
            }

            IEnumerable<Note> allNotes = midiData.GetNotes();
            CurrentTempoMap = midiData.GetTempoMap();

            if (noteNotifiers.Count == 0)
            {
                Debug.LogWarning("There are no note notifiers. This script will do nothing. Call CreateNoteNotifier before calling PrepareChart.");
            }

            foreach (NoteNotifier noteNotifier in noteNotifiers)
            {
                noteNotifier.EnqueueNotes(allNotes);
            }

            // Add the backing track
            if (audioSources.Count == 0)
            {
                audioSources.Add(gameObject.AddComponent<AudioSource>());
                audioSources[0].clip = currentChart.BackingTrack;
                audioSources[0].Stop();
            }

            int i = 1;
            foreach (string key in currentChart.Tracks.Keys)
            {
                AudioClip track = currentChart.Tracks[key];
                if (i >= audioSources.Count)
                {
                    audioSources.Add(gameObject.AddComponent<AudioSource>());
                }
                audioSources[i].clip = track;
                audioSources[i].Stop();
                i++;
            }

            IsPlaying = false;
        }

        /// <summary>
        /// Creates a NoteNotifier that will invoke a UnityAction `timeInAdvance` seconds before the note is played.
        /// </summary>
        /// <param name="timeInAdvance">The number of seconds to look ahead. This can be negative.</param>
        /// <returns>A note notifier. Add a listener to the OnNote property.</returns>
        public NoteNotifier CreateNoteNotifier(float timeInAdvance, NoteFilter noteFilter = null)
        {
            NoteNotifier noteNotifier = new NoteNotifier(timeInAdvance, noteFilter);
            noteNotifiers.Add(noteNotifier);
            return noteNotifier;
        }

        /// <summary>
        /// Plays the chart that was loaded with PrepareChart.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void PlayChart()
        {
            if(currentChart == null) throw new Exception("No chart loaded.");

            //foreach(AudioSource source in audioSources) source.Play(); //changing this because audios are not synced

            double offset = AudioSettings.dspTime;

            foreach(AudioSource source in audioSources)
            {
                source.Stop();
                source.PlayScheduled(offset);
            }
            // SyncPlayAllTracks();
            IsPlaying = true;
        }
        
        private void SyncPlayAllTracks(double delay = 0.1)
        {
            double dspStartTime = AudioSettings.dspTime;

            foreach (AudioSource source in audioSources)
            {
                source.Stop(); // ensure clean start
                source.PlayScheduled(dspStartTime);
            }

            Debug.Log($"Scheduled all audio sources for DSP time: {dspStartTime}");
        }

        public void LogAllTrackTimes()
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                AudioSource src = audioSources[i];
                string name = src.clip != null ? src.clip.name : "Unnamed";
                Debug.Log($"Track {i} - '{name}': time = {src.time:F4} / {src.clip?.length:F4}");
            }
        }


        /// <summary>
        /// Halts playback of a chart.
        /// </summary>
        public void StopChart()
        {
            foreach (AudioSource source in audioSources)
            {
                Destroy(source);
            }
            audioSources.Clear();
            IsPlaying = false;
            foreach (NoteNotifier noteNotifier in noteNotifiers) noteNotifier.Clear();
        }


        void Update()
        {
            if (!IsPlaying) return;

            foreach (NoteNotifier noteNotifier in noteNotifiers)
            {
                while (noteNotifier.Notes.Count > 0)
                {
                    Note note = noteNotifier.Notes.Peek();
                    MetricTimeSpan metricTime = note.TimeAs<MetricTimeSpan>(CurrentTempoMap);

                    if (metricTime.TotalMilliseconds > (audioSources[0].time + noteNotifier.TimeInAdvance) * 1000) break;

                    noteNotifier.Notes.Dequeue();
                    noteNotifier.OnNote.Invoke(note);
                }
            }
        }

        public bool IsAudioStillPlaying
        {
            get
            {
                return audioSources != null && audioSources.Count > 0 && audioSources[0].isPlaying;
            }
        }

        public List<ChartResource> GetAllCharts()
        {
            return loadedCharts;
        }
    }
}