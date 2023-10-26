using System.Collections;

public interface IUnit
{
    bool IsPlaying {get; set;}
    bool CanPlay {get; set;}
    bool IsUsingCard {get; set;}
    IEnumerator Play(float time);
}