public interface IUnit
{
    bool IsPlaying {get; set;}
    bool CanPlay {get; set;}
    void Play();
}