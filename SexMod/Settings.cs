using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace SexMod
{

		public class Settings : UnityModManager.ModSettings
		{
        
		public bool male = true;
		public bool female = false;
        public bool dev = false;



        public Settings()
		{
		}

		public override void Save(UnityModManager.ModEntry modEntry)
		{
			UnityModManager.ModSettings.Save<Settings>(this, modEntry);
		}


	}
}
