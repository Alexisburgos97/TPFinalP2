public interface ISettingsHandler
{
    void ApplySettings();
    float GetMasterVolume();
    void SetMasterVolume(float volume);
}