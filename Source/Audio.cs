using FMOD;
using System;
using System.Runtime.InteropServices;

namespace MuGen.Source;
public class Audio
{

    private static FMOD.System _system;
	private static FMOD.DSP_FFT_WINDOW _windowShape = FMOD.DSP_FFT_WINDOW.RECT;
	private FMOD.DSP _dsp;
	private ChannelGroup _group;
	private Sound _sound;
	private Channel _MusicChannel;
	private Channel _DspChannel;
    private float[] _samples = new float[512];
    
	public static void Initialize()
    {
        Factory.System_Create(out _system);

		Check(_system.init(512, INITFLAGS.NORMAL, IntPtr.Zero));
    }

    public void Load(string filePath)
    {
        Check(_system.createSound(filePath, MODE.DEFAULT, out _sound));

        _sound.setMode(MODE.LOOP_OFF);

		Check(_system.createDSPByType(FMOD.DSP_TYPE.FFT, out _dsp));
		_dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)_windowShape);
		_dsp.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, 512 * 2);
	}

    public float[] GetSpectrumSample()
    {
        return _samples;
    }

    public void Spectrum()
    {
        IntPtr data;
        uint length;

        Check(_dsp.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out data, out length)); //(int)FMOD.DSP_FFT.SPECTRUMDATA

		var spectrumBuffer = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(data, typeof(FMOD.DSP_PARAMETER_FFT));

        if(spectrumBuffer.numchannels == 0 )
        {
            _group.addDSP(0, _dsp);
        } else if (spectrumBuffer.numchannels >= 1 ) 
        {
            for (int i = 0; i < 512; i++)
            {
                float _totalChannelData = 0f;
                for (int j = 0; j < spectrumBuffer.numchannels; j++)
                {
                    _totalChannelData += spectrumBuffer.spectrum[j][i];
                    _samples[i] = _totalChannelData / spectrumBuffer.numchannels;
                }
            }
        }
    }

    public void Stop()
    {
        _group.stop();
        _dsp.reset();
    }

    public void Play()
	{
		_system.createChannelGroup("main", out _group);
		_group.addDSP(0, _dsp);
		Check(_system.playSound(_sound, _group, false, out _MusicChannel));
        Check(_system.playDSP(_dsp, _group, false, out _DspChannel));
    }

    public static void Update()
    {
        _system.update();
    }

    internal static void Check(RESULT result)
            => System.Diagnostics.Debug.Assert(result == RESULT.OK, $"FMOD failed: {result}");
}
