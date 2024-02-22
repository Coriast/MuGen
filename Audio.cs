using FMOD;
using System;

namespace MuGen;
public class Audio
{

	private static FMOD.System _system;
	private FMOD.Sound _sound;
	private FMOD.Channel _channel;

	public static void Initialize() 
	{ 
		FMOD.Factory.System_Create(out _system);

		Check(_system.init(512, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));
	}

	public void Load(string filePath)
	{
		Check(_system.createSound(filePath, FMOD.MODE.DEFAULT, out _sound));

		_sound.setMode(FMOD.MODE.LOOP_OFF);
	}

	public void Stop()
	{
		_channel.stop();
	}

	public void Play()
	{
		_system.playSound(_sound, new FMOD.ChannelGroup(), false, out _channel);
	}

	public static void Update()
	{
		_system.update();
	}

	internal static void Check(FMOD.RESULT result)
			=> System.Diagnostics.Debug.Assert(result == FMOD.RESULT.OK, $"FMOD failed: {result}");
}
