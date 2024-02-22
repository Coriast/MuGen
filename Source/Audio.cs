using FMOD;
using System;

namespace MuGen.Source;
public class Audio
{

    private static FMOD.System _system;
    private Sound _sound;
    private Channel _channel;

    public static void Initialize()
    {
        Factory.System_Create(out _system);

        Check(_system.init(512, INITFLAGS.NORMAL, IntPtr.Zero));
    }

    public void Load(string filePath)
    {
        Check(_system.createSound(filePath, MODE.DEFAULT, out _sound));

        _sound.setMode(MODE.LOOP_OFF);
    }

    public void Stop()
    {
        _channel.stop();
    }

    public void Play()
    {
        _system.playSound(_sound, new ChannelGroup(), false, out _channel);
    }

    public static void Update()
    {
        _system.update();
    }

    internal static void Check(RESULT result)
            => System.Diagnostics.Debug.Assert(result == RESULT.OK, $"FMOD failed: {result}");
}
