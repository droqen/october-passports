namespace passport.crunch {

[System.Serializable]
public class KeyedData {
	public readonly ushort key;
	object[] data;
	public KeyedData(ushort key, params object[] data) {
		this.key = key;
		this.data = data;
	}
	public T Read<T>(int index) {
		if(index<0||index>=data.Length) { Dj.Errorf("KeyedData.Data got bad index {0}", index); return default(T); }
		if(!(data[index] is T)) { Dj.Errorf("KeyedData.Data cannot cast index {0} to bad type {1}", index, typeof(T).ToString()); return default(T); }
		return (T)data[index];
	}
}
}