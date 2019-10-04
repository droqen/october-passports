namespace passport.crunch {

using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

///<summary>Serialize/Deserialize to/from byte arrays. Dead simple class with a ridiculous name.</summary>
public static class Capn {
	public static byte[] Crunchatize(object o)
    {
        MemoryStream stream = new MemoryStream();
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, o);
        return stream.ToArray();
    }
	public static T Decrunchatize<T>(byte[] buffer)
    {
        MemoryStream stream = new MemoryStream(buffer);
        IFormatter formatter = new BinaryFormatter();
		return (T)formatter.Deserialize(stream);
    }
	public static T Copy<T>(object o) {
		return Decrunchatize<T>(Crunchatize(o));
	}
}
	
}