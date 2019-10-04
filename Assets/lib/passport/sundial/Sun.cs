namespace passport.sundial {
	using UnityEngine;
	public static class Sun {
		public static ISundial dial;
		public static SD Dial<SD>() where SD : ISundial {
			return (SD)dial;
		}

	}
}