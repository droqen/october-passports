using UnityEngine;
public static class Dj
{
    const string prefix = "[Dj] ";
    public static void Temp(string msg)
    {
        Debug.Log(prefix + msg);
    }
    public static void Tempf(string msg, params object[] args)
    {
        Debug.Log(prefix + string.Format(msg, args));
    }
    public static void Warn(string msg)
    {
        Debug.LogWarning(prefix + msg);
    }
    public static void Warnf(string msg, params object[] args)
    {
        Debug.LogWarning(prefix + string.Format(msg, args));
    }
    public static void Error(string msg)
    {
        Debug.LogError(prefix + msg);
    }
    public static void Errorf(string msg, params object[] args)
    {
        Debug.LogError(string.Format(prefix + msg, args));
    }
    public static Dj.Exception Crash(string msg)
    {
        return new Dj.Exception(prefix + msg);
    }
    public static Dj.Exception Crashf(string msg, params object[] args)
    {
        return new Dj.Exception(string.Format(prefix + msg, args));
    }
    public class Exception : System.Exception
    {
        public Exception(string errmsg) : base(errmsg) { }
    }
}