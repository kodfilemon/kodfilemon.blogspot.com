using Microsoft.SPOT.Hardware;

namespace DemoPWMSound
{
public class PwmSpeaker : ISpeaker
{
    private readonly PWM _pwm;

    public PwmSpeaker(PWM pwm)
    {
        _pwm = pwm;
        _pwm.Frequency = 50;
        _pwm.DutyCycle = 0;
        _pwm.Start();
    }
            
    public void Play(double frequency)
    {
        _pwm.Frequency = frequency;
        _pwm.DutyCycle = 0.5;
    }

    public void Pause()
    {
        _pwm.Frequency = 50;
        _pwm.DutyCycle = 0;
    }
}
}

