# WavHello
C#/.NET wav file processing library and player. (.NET Framework version)

```csharp
using com.okitoki.wavhello;
using com.okitoki.wavhello.events;

public static void main(string[] args) 
{
    //Create a wav file player.
    WavePlayer player = new WavePlayer();
    
    //Play a wav file. 
    //You can store the read WaveFile object returned by the Play() method in a 
    //variable to get information about the file being played.
    //Some files contain metadata information about artist, track name, etc., such 
    //as if a song comes from Amazon music.
    WaveFile file = player.Play("C:\\song.wav");
    
    //Register a callback method to be called whenever the progress of the currently 
    //played file changed. Note that this is triggered after every 1% change.
    player.PlayProgressChanged += OnPlayProgressChanged;
}

public static void OnPlayProgressChanged(object sender, PlayProgressChangedEventArgs args)
{
    //Do something with the event.
    float progress = args.Progress; //This the percentage progress of the playback.
}
```
